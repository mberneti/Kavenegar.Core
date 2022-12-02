using System.Linq;
using System.Net;
using AutoMapper;
using Kavenegar.Core.Models;
using Kavenegar.Core.Utils;
using Shared.Infrastructure;

namespace Kavenegar.Core.Core;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<SendSingleMessageRequest, SendSingleMessageRequestDto>()
            .ForMember(
                sendSingleMessageRequestDto => sendSingleMessageRequestDto.Receptors,
                memberConfigurationExpression => memberConfigurationExpression.MapFrom(
                    sendSingleMessageRequest => string.Join(',', sendSingleMessageRequest.Receptors)))
            .ForMember(
                sendSingleMessageRequestDto => sendSingleMessageRequestDto.Sender,
                memberConfigurationExpression => memberConfigurationExpression.MapFrom(
                    sendSingleMessageRequest => sendSingleMessageRequest.MessageInfo.Sender))
            .ForMember(
                sendSingleMessageRequestDto => sendSingleMessageRequestDto.Message,
                memberConfigurationExpression => memberConfigurationExpression.MapFrom(
                    sendSingleMessageRequest => WebUtility.HtmlEncode(sendSingleMessageRequest.MessageInfo.Message)))
            .ForMember(
                sendSingleMessageRequestDto => sendSingleMessageRequestDto.Date,
                memberConfigurationExpression => memberConfigurationExpression.MapFrom(
                    sendSingleMessageRequest => sendSingleMessageRequest.Date.ToUnixTimestamp()))
            .ForMember(
                sendSingleMessageRequestDto => sendSingleMessageRequestDto.Type,
                memberConfigurationExpression => memberConfigurationExpression.MapFrom(
                    sendSingleMessageRequest => (int)sendSingleMessageRequest.MessageInfo.Type))
            .ForMember(
                sendSingleMessageRequestDto => sendSingleMessageRequestDto.LocalMessageId,
                memberConfigurationExpression => memberConfigurationExpression.MapFrom(
                    sendSingleMessageRequest => sendSingleMessageRequest.MessageInfo.LocalMessageId))
            .ForMember(
                sendSingleMessageRequestDto => sendSingleMessageRequestDto.Hide,
                memberConfigurationExpression => memberConfigurationExpression.MapFrom(
                    sendSingleMessageRequest => sendSingleMessageRequest.Hide ? 1 : 0));

        CreateMap<SendMultiMessageRequest, SendMultiMessageRequestDto>()
            .ForMember(
                sendMultiMessageRequestDto => sendMultiMessageRequestDto.Receptors,
                memberConfigurationExpression => memberConfigurationExpression.MapFrom(
                    sendSingleMessageRequest => sendSingleMessageRequest.Messages.Keys.ToList().Serialize(default)))
            .ForMember(
                sendMultiMessageRequestDto => sendMultiMessageRequestDto.Sender,
                memberConfigurationExpression => memberConfigurationExpression.MapFrom(
                    sendMultiMessageRequest => sendMultiMessageRequest.Messages.Values
                        .Select(messageInfo => messageInfo.Sender)
                        .ToList()
                        .Serialize(default)
                        .Result))
            .ForMember(
                sendMultiMessageRequestDto => sendMultiMessageRequestDto.Message,
                memberConfigurationExpression => memberConfigurationExpression.MapFrom(
                    sendMultiMessageRequest => sendMultiMessageRequest.Messages.Values
                        .Select(messageInfo => WebUtility.HtmlEncode(messageInfo.Message))
                        .ToList()
                        .Serialize(default)))
            .ForMember(
                sendMultiMessageRequestDto => sendMultiMessageRequestDto.Date,
                memberConfigurationExpression => memberConfigurationExpression.MapFrom(
                    sendMultiMessageRequest => sendMultiMessageRequest.Date.ToUnixTimestamp()))
            .ForMember(
                sendMultiMessageRequestDto => sendMultiMessageRequestDto.Type,
                memberConfigurationExpression => memberConfigurationExpression.MapFrom(
                    sendMultiMessageRequest => sendMultiMessageRequest.Messages.Values
                        .Select(messageInfo => (int)messageInfo.Type)
                        .ToList()
                        .Serialize(default)))
            .ForMember(
                sendMultiMessageRequestDto => sendMultiMessageRequestDto.LocalMessageId,
                memberConfigurationExpression => memberConfigurationExpression.MapFrom(
                    sendMultiMessageRequest => sendMultiMessageRequest.Messages.Values
                        .Select(messageInfo => messageInfo.LocalMessageId)
                        .ToList()
                        .Serialize(default)))
            .ForMember(
                sendMultiMessageRequestDto => sendMultiMessageRequestDto.Hide,
                memberConfigurationExpression => memberConfigurationExpression.MapFrom(
                    sendMultiMessageRequest => sendMultiMessageRequest.Hide ? 1 : 0));
    }
}