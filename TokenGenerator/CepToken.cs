namespace TokenGenerator
{
    using System;
    using ServiceStack;

    /// <summary>
    /// Object containing token fields with the CEP naming.
    /// </summary>
    public class CepToken : IToken
    {
        /// <summary>
        /// Gets or sets the client ID.
        /// </summary>
        public string ImpersonatedDealer { get; set; }

        /// <summary>
        /// Gets or sets the user country.
        /// </summary>
        public EUserCountry CountryCode { get; set; }

        /// <summary>
        /// Gets or sets the user group ID.
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// Gets or sets the token creation date in CEP.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the user's culture.
        /// </summary>
        public string Culture { get; set; }

        /// <summary>
        /// Serializes the object as JSON.
        /// </summary>
        /// <returns>A JSON string representing the serialized object.</returns>
        public override string ToString()
        {
            return this.SerializeToString();
        }
    }
}
