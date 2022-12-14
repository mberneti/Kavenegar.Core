using Kavenegar.Core;
using Kavenegar.Core.Dto.Message;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationTest.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class MessageSenderController : ControllerBase
{
    private readonly IKavenegarMessageSender _kavenegarMessageSender;

    public MessageSenderController(
        IKavenegarMessageSender kavenegarMessageSender)
    {
        _kavenegarMessageSender = kavenegarMessageSender;
    }

    [HttpPost]
    public async Task<IActionResult> SendSingleMessage(
        SendSingleMessageRequest singleMessageRequest)
    {
        return Ok(await _kavenegarMessageSender.Send(singleMessageRequest));
    }

    [HttpPost]
    public async Task<IActionResult> SendMultiMessage(
        SendMultiMessageRequest sendMultiMessageRequest)
    {
        return Ok(await _kavenegarMessageSender.Send(sendMultiMessageRequest));
    }

    [HttpPost]
    public async Task<IActionResult> VerifyLookUp(
        VerifyLookupRequest verifyLookupRequest)
    {
        return Ok(await _kavenegarMessageSender.VerifyLookup(verifyLookupRequest));
    }
}