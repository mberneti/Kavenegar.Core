using Kavenegar.Core;
using Kavenegar.Core.Dto.Message;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationTest.Controllers;

public class MessageSenderController : BaseAPiController
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
        return HandleValue(await _kavenegarMessageSender.Send(singleMessageRequest));
    }

    [HttpPost]
    public async Task<IActionResult> SendMultiMessage(
        SendMultiMessageRequest sendMultiMessageRequest)
    {
        return HandleValue(await _kavenegarMessageSender.Send(sendMultiMessageRequest));
    }

    [HttpPost]
    public async Task<IActionResult> VerifyLookUp(
        VerifyLookupRequest verifyLookupRequest)
    {
        return HandleValue(await _kavenegarMessageSender.VerifyLookup(verifyLookupRequest));
    }
}