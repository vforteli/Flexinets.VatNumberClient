using System;

namespace Flexinets.VatNumberClient
{
    public class VatNumberValidationResponse
    {
        /// <summary>
        /// True if the Vat number was successfully validated
        /// </summary>
        public Boolean valid;

        /// <summary>
        /// Typed validation result
        /// </summary>
        public VatNumberValidationResult result;

        /// <summary>
        /// A parsed version of the Vat number
        /// </summary>
        public String parsedVatNumber;

        /// <summary>
        /// Status or error message
        /// </summary>
        public String message;

        /// <summary>
        /// If successful, returns the name of the entity associated with the Vat number
        /// </summary>
        public String name;
    }
}
