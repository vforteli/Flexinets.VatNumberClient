using log4net;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Flexinets.VatNumberClient
{
    public class VatNumberValidatorClient
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(VatNumberValidatorClient));


        /// <summary>
        /// Validate a Vat number using EU api
        /// </summary>
        /// <param name="vatnumber"></param>
        /// <returns></returns>
        public async static Task<VatNumberResponse> ValidateAsync(String vatnumber)
        {
            if (String.IsNullOrEmpty(vatnumber))
            {
                return new VatNumberResponse { valid = true };
            }

            vatnumber = vatnumber.Replace(" ", "").Replace("-", "").ToUpperInvariant();

            // country code + minimum 8 characters
            if (vatnumber.Length < 10)
            {
                return new VatNumberResponse { valid = false, parsedVatNumber = vatnumber, message = "Vat number too short" };
            }

            var regex = new Regex("[a-z]{2}.*", RegexOptions.IgnoreCase);
            if (!regex.IsMatch(vatnumber))
            {
                return new VatNumberResponse { valid = false, parsedVatNumber = vatnumber, message = "Please enter VAT Number in international format" };
            }

            var vatClient = new EU.checkVatPortTypeClient(new BasicHttpBinding(), new EndpointAddress("http://ec.europa.eu/taxation_customs/vies/services/checkVatService"));
            try
            {
                var response = await vatClient.checkVatAsync(vatnumber.Substring(0, 2), vatnumber.Substring(2));
                if (response.Body.valid)
                {
                    return new VatNumberResponse { valid = true, parsedVatNumber = vatnumber, name = response.Body.name };
                }
            }
            catch (Exception ex)
            {
                _log.Warn($"invalid input to vat client: {vatnumber}", ex);
            }
            return new VatNumberResponse { valid = false, parsedVatNumber = vatnumber };
        }
    }


    public class VatNumberResponse
    {
        public Boolean valid;
        public String parsedVatNumber;
        public String message;
        public String name;
    }
}