using log4net;
using System;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Flexinets.VatNumberClient
{
    public class VatNumberValidatorClient
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(VatNumberValidatorClient));

        // Regex from https://www.safaribooksonline.com/library/view/regular-expressions-cookbook/9781449327453/ch04s21.html
        private static readonly String _euVatRegex = @"^((AT)?U[0-9]{8}|(BE)?0[0-9]{9}|(BG)?[0-9]{9,10}|(CY)?[0-9]{8}L|(CZ)?[0-9]{8,10}|(DE)?[0-9]{9}|(DK)?[0-9]{8}|(EE)?[0-9]{9}|(EL|GR)?[0-9]{9}|(ES)?[0-9A-Z][0-9]{7}[0-9A-Z]|(FI)?[0-9]{8}|(FR)?[0-9A-Z]{2}[0-9]{9}|(GB)?([0-9]{9}([0-9]{3})?|[A-Z]{2}[0-9]{3})|(HU)?[0-9]{8}|(IE)?[0-9]S[0-9]{5}L|(IT)?[0-9]{11}|(LT)?([0-9]{9}|[0-9]{12})|(LU)?[0-9]{8}|(LV)?[0-9]{11}|(MT)?[0-9]{8}|(NL)?[0-9]{9}B[0-9]{2}|(PL)?[0-9]{10}|(PT)?[0-9]{9}|(RO)?[0-9]{2,10}|(SE)?[0-9]{12}|(SI)?[0-9]{8}|(SK)?[0-9]{10})$";

        /// <summary>
        /// Validate a Vat number using EU api
        /// </summary>
        /// <param name="vatnumber"></param>
        /// <returns></returns>
        public async static Task<VatNumberValidationResponse> ValidateAsync(String vatnumber)
        {
            if (String.IsNullOrEmpty(vatnumber))
            {
                return new VatNumberValidationResponse { valid = true, result = VatNumberValidationResult.Empty };
            }

            vatnumber = TrimVatNumber(vatnumber);

            // Check if this appears to be an EU vat number
            if (!EuCountryCodes.GetEuCountryCodes().Contains(vatnumber.Substring(0, 2)))    // yes yes, it is more efficient to check if the code exists...
            {
                return new VatNumberValidationResponse { valid = true, result = VatNumberValidationResult.NonEuVatNumber };
            }


            // Validate the format
            var r = new Regex(_euVatRegex);
            if (!r.IsMatch(vatnumber))
            {
                return new VatNumberValidationResponse { valid = false, result = VatNumberValidationResult.EuVatNumberInvalidFormat, parsedVatNumber = vatnumber, message = "Invalid Vat Number format" };
            }


            // This appears to be an EU vat number, check API
            var vatClient = new EU.checkVatPortTypeClient(new BasicHttpBinding(), new EndpointAddress("http://ec.europa.eu/taxation_customs/vies/services/checkVatService"));
            try
            {
                var response = await vatClient.checkVatAsync(vatnumber.Substring(0, 2), vatnumber.Substring(2));
                if (response.Body.valid)
                {
                    return new VatNumberValidationResponse { valid = true, result = VatNumberValidationResult.EuVatNumberValid, parsedVatNumber = vatnumber, name = response.Body.name };
                }
            }
            catch (Exception ex)
            {
                _log.Warn($"invalid input to vat client: {vatnumber}", ex);
            }
            return new VatNumberValidationResponse { valid = false, result = VatNumberValidationResult.EuVatNumberInvalid, parsedVatNumber = vatnumber };
        }


        /// <summary>
        /// Trim a vat number to A-Z0-9 or something similar and convert to upper case
        /// </summary>
        /// <param name="vatNumber"></param>
        /// <returns></returns>
        private static String TrimVatNumber(String vatNumber)
        {
            // todo maybe use regex to strip everything except A-Z0-9 instead...
            return vatNumber.Replace(" ", "").Replace("-", "").Replace(".", "").ToUpperInvariant();
        }
    }
}