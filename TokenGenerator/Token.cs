namespace TokenGenerator
{
    using System;
    using ServiceStack;

    /// <summary>
    /// The token class.
    /// </summary>
    public class Token
    {
        /// <summary>
        /// Gets or sets the client ID.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the user group id.
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// Gets or sets the token creation date.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the user country.
        /// </summary>
        public UserCountry Country { get; set; }

        /// <summary>
        /// Gets or sets the user culture identifier string.
        /// </summary>
        public string Culture { get; set; }

        /// <summary>
        /// Gets or sets the authentication type.
        /// </summary>
        public string AuthenticationType { get; set; }

        /// <summary>
        /// Gets or sets the user's client key.
        /// </summary>
        public int ClientKey { get; set; }

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
