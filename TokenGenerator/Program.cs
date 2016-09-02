namespace TokenGenerator
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                throw new Exception("Invalid arguments!");
            }

            var tokenObjects = ReadCsv(args[0]);
            var tokens = EncryptObjects(tokenObjects);
            WriteCsv(args[1], tokens);
        }

        private static IEnumerable<string> EncryptObjects(IEnumerable<Token> tokenObjects)
        {
            var ciptherUtility = new CipherUtility();
            return tokenObjects.Select(token => ciptherUtility.Encrypt(token.ToString())).ToList();
        }

        private static void WriteCsv(string fileName, IEnumerable<string> tokens)
        {
            var path = Path.GetFullPath(fileName);

            using (var writer = new StreamWriter(path))
            {
                const string firstLine = "token";

                writer.WriteLine(firstLine);

                foreach (var token in tokens)
                {
                    writer.WriteLine(token);
                }

                writer.Flush();
            }
        }

        private static IEnumerable<Token> ReadCsv(string fileName)
        {
            var result = new List<Token>();

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

                    var token = new Token
                    {
                        AuthenticationType = AuthenticationType.CepAuthentication.ToString(),
                        CreatedDate = DateTime.Now,
                        Culture = "en-US",
                        ClientId = splitted[clientIdNumber.Value],
                        ClientKey = int.Parse(splitted[clientKeyNumber.Value]),
                        GroupId = splitted[groupIdNumber.Value].ToUpperInvariant(),
                        Country = splitted[countryNumber.Value].ToEnum(UserCountry.None)
                    };

                    var country = splitted[countryNumber.Value];
                    int countryInt;
                    token.Country = int.TryParse(country, out countryInt)
                        ? countryInt.ToEnum(UserCountry.None)
                        : splitted[countryNumber.Value].ToEnum(UserCountry.None);

                    result.Add(token);
                }
            }

            return result;
        }
    }
}
