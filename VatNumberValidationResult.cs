namespace Flexinets.VatNumberClient
{
    public enum VatNumberValidationResult
    {
        EuVatNumberValid,
        EuVatNumberInvalid,
        EuVatNumberInvalidFormat,
        EuVatNumberUnknown,
        NonEuVatNumber,
        Empty
    }
}
