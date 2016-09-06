namespace TokenGenerator
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;

    /// <summary>
    /// The main Program class.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The application start main method.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        public static void Main(string[] args)
        {
            // validate the input parameters count
            if (args.Length != 3)
            {
                throw new Exception("Invalid arguments!");
            }

            var isCepToken = args[2].Equals("cep", StringComparison.OrdinalIgnoreCase);

            var tokenObjects = ReadCsv(args[0], isCepToken);
            var tokens = EncryptObjects(tokenObjects, isCepToken);
            WriteCsv(args[1], tokens, isCepToken);
        }

        /// <summary>
        /// Encrypt the Token objects.
        /// </summary>
        /// <param name="tokenObjects">The list of tokens.</param>
        /// <param name="isCepToken">The token type.</param>
        /// <returns>A list of encrypted token objects as strings.</returns>
        private static IEnumerable<string> EncryptObjects(IEnumerable<IToken> tokenObjects, bool isCepToken)
        {
            var ciptherUtility = new CipherUtility();
            return tokenObjects.Select(token => ciptherUtility.Encrypt(token.ToString(), isCepToken)).ToList();
        }

        /// <summary>
        /// Write the strings to a CSV file.
        /// </summary>
        /// <param name="fileName">The output file name.</param>
        /// <param name="tokens">The list of tokens.</param>
        /// <param name="isCepToken">The token type.</param>
        private static void WriteCsv(string fileName, IEnumerable<string> tokens, bool isCepToken)
        {
            var path = Path.GetFullPath(fileName);

            using (var writer = new StreamWriter(path))
            {
                const string firstLine = "token";

                writer.WriteLine(firstLine);

                foreach (var token in tokens)
                {
                    writer.WriteLine(isCepToken ? WebUtility.UrlEncode(token) : token);
                }

                writer.Flush();
            }
        }

        /// <summary>
        /// Read from a CSV file.
        /// </summary>
        /// <param name="fileName">The input file name.</param>
        /// <param name="isCepToken">A value indicating whether the token is from CEP.</param>
        /// <returns>A list of token objects.</returns>
        private static IEnumerable<IToken> ReadCsv(string fileName, bool isCepToken)
        {
            var result = new List<IToken>();

            var path = Path.GetFullPath(fileName);

            if (!File.Exists(path))
            {
                throw new Exception("The file does not exists.");
            }

            using (var reader = new StreamReader(path))
            {
                string currentLine;
                var isFirst = true;
                int? clientIdNumber = null;
                int? clientKeyNumber = null;
                int? groupIdNumber = null;
                int? countryNumber = null;

                while ((currentLine = reader.ReadLine()) != null)
                {
                    var splitted = currentLine.Split(',');

                    if (splitted.Length < 4)
                    {
                        throw new Exception("Unexpected CSV header number. Columns are missing.");
                    }

                    if (isFirst)
                    {
                        for (var i = 0; i < splitted.Length; i++)
                        {
                            switch (splitted[i].ToLowerInvariant())
                            {
                                case "clientid":
                                    clientIdNumber = i;
                                    break;
                                case "clientkey":
                                    clientKeyNumber = i;
                                    break;
                                case "groupid":
                                    groupIdNumber = i;
                                    break;
                                case "country":
                                    countryNumber = i;
                                    break;
                            }
                        }

                        if (!clientIdNumber.HasValue || !clientKeyNumber.HasValue || !groupIdNumber.HasValue ||
                            !countryNumber.HasValue)
                        {
                            throw new Exception("Mandatory headers missing.");
                        }

                        isFirst = false;
                        continue;
                    }

                    IToken token;

                    var country = splitted[countryNumber.Value];
                    int countryInt;

                    if (isCepToken)
                    {
                        token = new CepToken
                        {
                            ImpersonatedDealer = splitted[clientIdNumber.Value],
                            GroupId = splitted[groupIdNumber.Value].ToUpperInvariant(),
                            Culture = "en-US",
                            CreatedDate = DateTime.Now,
                            CountryCode = int.TryParse(country, out countryInt)
                                ? countryInt.ToEnum(EUserCountry.None)
                                : country.ToEnum(EUserCountry.None)
                        };
                    }
                    else
                    {
                        token = new Token
                        {
                            AuthenticationType = AuthenticationType.CepAuthentication.ToString(),
                            CreatedDate = DateTime.Now,
                            Culture = "en-US",
                            ClientId = splitted[clientIdNumber.Value],
                            ClientKey = int.Parse(splitted[clientKeyNumber.Value]),
                            GroupId = splitted[groupIdNumber.Value].ToUpperInvariant(),
                            Country = int.TryParse(country, out countryInt)
                                ? countryInt.ToEnum(UserCountry.None)
                                : country.ToEnum(UserCountry.None)
                        };
                    }
                    
                    result.Add(token);
                }
            }

            return result;
        }
    }
}
