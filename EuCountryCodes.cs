using System;
using System.Collections.Generic;
using System.Text;

namespace Flexinets.VatNumberClient
{
    public static class EuCountryCodes
    {
        /// <summary>
        /// Returns an upper case list of eu vat country codes
        /// </summary>
        /// <returns></returns>
        public static List<String> GetEuCountryCodes()
        {
            return new List<String>
            {
                "AT",
                "BE",
                "BG",
                "HR",
                "CY",
                "CZ",
                "DK",
                "EE",
                "FI",
                "FR",
                "DE",
                "EL",
                "HU",
                "IE",
                "IT",
                "LV",
                "LT",
                "LU",
                "MT",
                "NL",
                "PL",
                "PT",
                "RO",
                "SK",
                "SI",
                "ES",
                "SE",
                "UK"
            };
        }
    }
}
