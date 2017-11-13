using Flexinets.VatNumberClient.EU;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Flexinets.VatNumberClient
{
    public class VatNumberValidationResponse
    {
        /// <summary>
        /// True if the Vat number was successfully validated
        /// </summary>
        public Boolean valid { get; set; }

        /// <summary>
        /// Typed validation result
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public VatNumberValidationResult result { get; set; }

        /// <summary>
        /// A parsed version of the Vat number
        /// </summary>
        public String parsedVatNumber { get; set; }

        /// <summary>
        /// Status or error message
        /// </summary>
        public String message { get; set; }

        /// <summary>
        /// If successful, returns the name of the entity associated with the Vat number
        /// </summary>
        public String name { get; set; }


        public checkVatResponseBody checkVatResponseBody { get; set; }
    }
}
