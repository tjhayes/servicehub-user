using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ServiceHub.User.Library.Models
{
    /// <summary>
    /// Stores data for an address model.
    /// </summary>
    /// <remarks>
    /// The residential address of a user.
    /// </remarks>
    public class Address
    {
        /// <value> The unique ID of an address. </value>
        public Guid AddressId { get; set; }
        ///<value> Address line one </value>
        public string Address1 { get; set; }
        ///<value> Address line two </value>
        public string Address2 { get; set; }
        /// <value> The city. </value>
        public string City { get; set; }
        /// <value> The state. </value>
        public string State { get; set; }
        /// <value> The zip code.. </value>
        public string PostalCode { get; set; }
        /// <value> The country. </value>
        public string Country { get; set; }

        /// <summary>
        /// Check whether the address is valid.
        /// </summary>
        /// <remarks>
        /// AddressId, Address1, City, State, PostalCode and Country are all required.
        /// Address2 is not required but cannot be an empty string.
        /// If the country is the United States, postal codes must follow the
        /// 5-digit convention (with or without the 4-digit extension code).
        /// Country must follow the ISO Alpha-2 country code format, 
        /// e.g. US for United States and GB for United Kingdom.
        /// </remarks>
        /// <returns>True if the address is valid and false otherwise</returns>
        public Boolean Validate()
        {
            if (AddressId == Guid.Empty) { return false; }
            if (Address1 == null || Address1 == "") { return false; }
            if (Address2 == "") { return false; }
            if (City == null || City == "") { return false; }
            if (State == null || State == "") { return false; }
            if (PostalCode == null || PostalCode == "") { return false; }
            if (Country == null || ValidateCountry() == false) { return false; }
            if (Country.ToUpper() == "US" && ValidateAmericanState() == false) { return false; }
            if (Country.ToUpper() == "US" && ValidateAmericanPostalCode() == false) { return false; }
            return true;
        }

        /// <summary>
        /// Check whether Country is a valid ISO Alpha-2 country code.
        /// </summary>
        /// <remarks>
        /// Try to construct a RegionInfo object with the Country string.
        /// If RegionInfo constructor throws ArgumentException,
        /// then Country is not a valid ISO Alpha-2 country code.
        /// Thus Country is invalid. If no exception is thrown, Country is valid.
        /// </remarks>
        /// <returns>True if Country is valid, and false otherwise</returns>
        public Boolean ValidateCountry()
        {
            bool validCountry = true;
            try { RegionInfo region = new RegionInfo(Country); }
            catch (ArgumentException) { validCountry = false; }
            return validCountry;
        }

        /// <summary>
        /// Check whether the string is one of the valid 2-digit American
        /// State codes. If so, it is valid, otherwise invalid.
        /// </summary>
        /// <returns>True if the state code is valid, and false otherwise.</returns>
        public Boolean ValidateAmericanState()
        {
            switch (State)
            {
                case "AL":
                case "AK":
                case "AR":
                case "AZ":
                case "CA":
                case "CO":
                case "CT":
                case "DE":
                case "FL":
                case "GA":
                case "HI":
                case "ID":
                case "IL":
                case "IN":
                case "IA":
                case "KS":
                case "KY":
                case "LA":
                case "ME":
                case "MD":
                case "MA":
                case "MI":
                case "MN":
                case "MS":
                case "MO":
                case "MT":
                case "NE":
                case "NV":
                case "NH":
                case "NJ":
                case "NM":
                case "NY":
                case "NC":
                case "ND":
                case "OH":
                case "OK":
                case "OR":
                case "PA":
                case "RI":
                case "SC":
                case "SD":
                case "TN":
                case "TX":
                case "UT":
                case "VT":
                case "VA":
                case "WA":
                case "WV":
                case "WI":
                case "WY":
                case "AS":
                case "DC":
                case "GU":
                case "MH":
                case "FM":
                case "MP":
                case "PW":
                case "PR":
                case "VI":
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Check whether the postal code is in the 5-digit ZIP or the ZIP+4
        /// postal code format for American postal codes. 
        /// If it is, then it is valid. Otherwise it is invalid.
        /// </summary>
        /// <returns>True if postal code is in a valid format and false otherwise.</returns>
        public Boolean ValidateAmericanPostalCode()
        {
            if(PostalCode == null) { return false; }
            Regex regex = new Regex(@"^\d{5}(?:-\d{4})?$");
            return regex.Match(PostalCode).Success;
        }
    }
}
