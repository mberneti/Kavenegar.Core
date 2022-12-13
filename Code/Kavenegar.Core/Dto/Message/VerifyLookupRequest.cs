using Kavenegar.Core.Enums;

namespace Kavenegar.Core.Dto.Message;

public class VerifyLookupRequest
{
    public VerifyLookupRequest(
        string receptor,
        string template,
        string token1)
    {
        Receptor = receptor;
        Template = template;
        Token1 = token1;
    }

    public string Receptor { get; set; }
    public string Template { get; set; }
    public string Token1 { get; set; }
    public string? Token2 { get; set; }
    public string? Token3 { get; set; }
    public string? Token4 { get; set; }
    public string? Token5 { get; set; }
    public VerifyLookupType? VerifyLookupType { get; set; } = null!;
}