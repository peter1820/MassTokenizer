namespace TokenGenerator
{
    using System;

    /// <summary>
    /// Enumerable containing all the possible user countries.
    /// </summary>
    public enum UserCountry
    {
        /// <summary>
        /// The user country is unknown.
        /// </summary>
        None,

        /// <summary>
        /// Australia country code.
        /// </summary>
        Aus,

        /// <summary>
        /// Canada country code.
        /// </summary>
        Can,

        /// <summary>
        /// Great Britain country code.
        /// </summary>
        Gbr,

        /// <summary>
        /// Ireland country code.
        /// </summary>
        Irl,

        /// <summary>
        /// New Zealand country code.
        /// </summary>
        Nzl,

        /// <summary>
        /// United States of America country code.
        /// </summary>
        Usa
    }

    /// <summary>
    /// The old user country list.
    /// </summary>
    public enum EUserCountry
    {
        /// <summary>
        /// The default value.
        /// </summary>
        None,

        /// <summary>
        /// The USA.
        /// </summary>
        USA,

        /// <summary>
        /// Canada code.
        /// </summary>
        CAN,

        /// <summary>
        /// Australia code.
        /// </summary>
        AUS,

        /// <summary>
        /// New-Zealand code.
        /// </summary>
        NZL,

        /// <summary>
        /// United kingdom code.
        /// </summary>
        GBR,

        /// <summary>
        /// Ireland code.
        /// </summary>
        IRL
    }

    /// <summary>
    /// Enumerable containing all possible authentication types.
    /// </summary>
    public enum AuthenticationType
    {
        /// <summary>
        /// The user is not authenticated.
        /// </summary>
        None,

        /// <summary>
        /// The user is authenticated from CEP application by a token.
        /// </summary>
        CepAuthentication,

        /// <summary>
        /// The user is authenticated over the LDAP server.
        /// </summary>
        LdapAuthentication
    }

    /// <summary>
    /// Class containing enumerable extension methods.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated constants to an equivalent enumerated object.
        /// </summary>
        /// <typeparam name="TEnum">The type value.</typeparam>
        /// <param name="value">The value to be converted.</param>
        /// <param name="defaultValue">The enumerable default value.</param>
        /// <returns>An object of whose value is represented by value or the default value.</returns>
        public static TEnum ToEnum<TEnum>(this string value, TEnum defaultValue)
        {
            TEnum returnVal;

            try
            {
                returnVal = (TEnum)Enum.Parse(typeof(TEnum), value, true);
            }
            catch (ArgumentException)
            {
                returnVal = defaultValue;
            }

            return returnVal;
        }

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated constants to an equivalent enumerated object.
        /// </summary>
        /// <typeparam name="TEnum">The type value.</typeparam>
        /// <param name="value">The value to convert.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>An object whose value is represented by value or the default value.</returns>
        public static TEnum ToEnum<TEnum>(this int value, TEnum defaultValue)
        {
            if (value < 0 || !Enum.IsDefined(typeof(TEnum), value))
            {
                return defaultValue;
            }

            return (TEnum)Enum.Parse(typeof(TEnum), value.ToString());
        }
    }
}
