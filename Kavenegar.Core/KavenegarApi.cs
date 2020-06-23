using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Kavenegar.Core.Exceptions;
using Kavenegar.Core.Models;
using Kavenegar.Core.Models.Enums;
using Kavenegar.Core.Utils;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Kavenegar
{
    internal class ReturnResult
    {
        public Result @Return { get; set; }
        public object entries { get; set; }
    }

    internal class Result
    {
        public int status { get; set; }
        public string message { get; set; }
    }

    internal class ReturnSend
    {
        public Result @Return { get; set; }
        public List<SendResult> entries { get; set; }
    }

    internal class ReturnStatus
    {
        public Result result { get; set; }
        public List<StatusResult> entries { get; set; }
    }

    internal class ReturnStatusLocalMessageId
    {
        public Result result { get; set; }
        public List<StatusLocalMessageIdResult> entries { get; set; }
    }

    internal class ReturnReceive
    {
        public Result result { get; set; }
        public List<ReceiveResult> entries { get; set; }
    }

    internal class ReturnCountOutbox
    {
        public Result result { get; set; }
        public List<CountOutboxResult> entries { get; set; }
    }

    internal class ReturnCountInbox
    {
        public Result result { get; set; }
        public List<CountInboxResult> entries { get; set; }

    }

    internal class ReturnCountPostalCode
    {
        public Result result { get; set; }
        public List<CountPostalCodeResult> entries { get; set; }
    }

    internal class ReturnAccountInfo
    {
        public Result result { get; set; }
        public AccountInfoResult entries { get; set; }
    }

    internal class ReturnAccountConfig
    {
        public Result result { get; set; }
        public AccountConfigResult entries { get; set; }
    }

    public class KavenegarApi
    {
        private string _apikey;
        private static HttpClient _client;
        private int _returnCode = 200;
        private string _returnMessage = "";
        private const string Apipath = "{0}/{1}.{2}";
        private const string BaseUrl = "https://api.kavenegar.com/v1";
        public KavenegarApi(string apikey)
        {
            _apikey = apikey;

            _client = new HttpClient
            {
                BaseAddress = new Uri($"{BaseUrl}/{_apikey}/")
            };

        }

        public KavenegarApi(string apikey, HttpClient client)
        {
            _apikey = apikey;

            var baseUri = new Uri($"{BaseUrl}/{_apikey}/");
            if (client.BaseAddress != baseUri)
                client.BaseAddress = baseUri;
            _client = client;

        }

        public string ApiKey
        {
            set => _apikey = value;
            get => _apikey;
        }

        public int ReturnCode
        {
            get { return _returnCode; }

        }

        public string ReturnMessage
        {
            get { return _returnMessage; }

        }

        private string GetApiPath(string _base, string method, string output)
        {
            return string.Format(Apipath, _base, method, output);
        }

        private async Task<string> Execute(string path, Dictionary<string, object> _params)
        {
            var nvc = _params?.Select(x => new KeyValuePair<string, string>(x.Key, x.Value?.ToString()));

            var postdata = new FormUrlEncodedContent(nvc);

            var response = await _client.PostAsync(path, postdata);
            var responseBody = await response.Content.ReadAsStringAsync();

            // System.Diagnostics.Debug.WriteLine(responseBody);

            try
            {
                var result = JsonConvert.DeserializeObject<ReturnResult>(responseBody);

                if (response.StatusCode != HttpStatusCode.OK)
                    throw new ApiException(result.Return.message, result.Return.status);

                return responseBody;
            }
            catch (HttpException)
            {
                throw;
            }
        }
        public async Task<List<SendResult>> Send(string sender, List<string> receptor, string message)
        {
            return await Send(sender, receptor, message, MessageType.MobileMemory, DateTime.MinValue);
        }

        public async Task<SendResult> Send(string sender, String receptor, string message)
        {
            return await Send(sender, receptor, message, MessageType.MobileMemory, DateTime.MinValue);
        }
        public async Task<SendResult> Send(string sender, string receptor, string message, MessageType type, DateTime date)
        {
            List<String> receptors = new List<String> { receptor };
            return (await Send(sender, receptors, message, type, date))[0];
        }
        public async Task<List<SendResult>> Send(string sender, List<string> receptor, string message, MessageType type, DateTime date)
        {
            return await Send(sender, receptor, message, type, date, null);
        }
        public async Task<SendResult> Send(string sender, string receptor, string message, MessageType type, DateTime date, string localid)
        {
            var receptors = new List<String> { receptor };
            var localids = new List<String> { localid };
            return (await Send(sender, receptors, message, type, date, localids))[0];
        }
        public async Task<SendResult> Send(string sender, string receptor, string message, string localid)
        {
            return await Send(sender, receptor, message, MessageType.MobileMemory, DateTime.MinValue, localid);
        }
        public async Task<List<SendResult>> Send(string sender, List<string> receptors, string message, string localid)
        {
            List<String> localids = new List<String>();
            for (var i = 0; i <= receptors.Count - 1; i++)
            {
                localids.Add(localid);
            }
            return await Send(sender, receptors, message, MessageType.MobileMemory, DateTime.MinValue, localids);
        }
        public async Task<List<SendResult>> Send(string sender, List<string> receptor, string message, MessageType type, DateTime date, List<string> localids)
        {
            var path = GetApiPath("sms", "send", "json");
            var param = new Dictionary<string, object>
        {
            {"sender", System.Net.WebUtility.HtmlEncode(sender)},
            {"receptor", System.Net.WebUtility.HtmlEncode(StringHelper.Join(",", receptor.ToArray()))},
            {"message", System.Net.WebUtility.HtmlEncode(message)},
            {"type", (int) type},
            {"date", date == DateTime.MinValue ? 0 : DateHelper.DateTimeToUnixTimestamp(date)}
        };
            if (localids != null && localids.Count > 0)
            {
                param.Add("localid", StringHelper.Join(",", localids.ToArray()));
            }
            var responseBody = await Execute(path, param);
            var l = JsonConvert.DeserializeObject<ReturnSend>(responseBody);
            return l.entries;
        }

        public async Task<List<SendResult>> SendArray(List<string> senders, List<string> receptors, List<string> messages)
        {
            var types = new List<MessageType>();
            for (var i = 0; i <= senders.Count - 1; i++)
            {
                types.Add(MessageType.MobileMemory);
            }
            return await SendArray(senders, receptors, messages, types, DateTime.MinValue, null);
        }

        public async Task<List<SendResult>> SendArray(string sender, List<string> receptors, List<string> messages, MessageType type, DateTime date)
        {
            var senders = new List<string>();
            for (var i = 0; i < receptors.Count; i++)
            {
                senders.Add(sender);
            }
            var types = new List<MessageType>();
            for (var i = 0; i <= senders.Count - 1; i++)
            {
                types.Add(MessageType.MobileMemory);
            }
            return await SendArray(senders, receptors, messages, types, date, null);
        }

        public async Task<List<SendResult>> SendArray(string sender, List<string> receptors, List<string> messages, MessageType type, DateTime date, string localmessageids)
        {
            var senders = new List<String>();
            for (var i = 0; i < receptors.Count; i++)
            {
                senders.Add(sender);
            }
            List<MessageType> types = new List<MessageType>();
            for (var i = 0; i <= senders.Count - 1; i++)
            {
                types.Add(MessageType.MobileMemory);
            }
            return await SendArray(senders, receptors, messages, types, date, new List<String>() { localmessageids });
        }

        public async Task<List<SendResult>> SendArray(string sender, List<string> receptors, List<string> messages, string localmessageid)
        {
            List<String> senders = new List<String>();
            for (var i = 0; i < receptors.Count; i++)
            {
                senders.Add(sender);
            }

            return await SendArray(senders, receptors, messages, localmessageid);
        }

        public async Task<List<SendResult>> SendArray(List<string> senders, List<string> receptors, List<string> messages, string localmessageid)
        {
            var types = new List<MessageType>();
            for (var i = 0; i <= receptors.Count - 1; i++)
            {
                types.Add(MessageType.MobileMemory);
            }
            var localmessageids = new List<string>();
            for (var i = 0; i <= receptors.Count - 1; i++)
            {
                localmessageids.Add(localmessageid);
            }
            return await SendArray(senders, receptors, messages, types, DateTime.MinValue, localmessageids);
        }

        public async Task<List<SendResult>> SendArray(List<string> senders, List<string> receptors, List<string> messages, List<MessageType> types, DateTime date, List<string> localmessageids)
        {
            String path = GetApiPath("sms", "sendarray", "json");
            var jsonSenders = JsonConvert.SerializeObject(senders);
            var jsonReceptors = JsonConvert.SerializeObject(receptors);
            var jsonMessages = JsonConvert.SerializeObject(messages);
            var jsonTypes = JsonConvert.SerializeObject(types);
            var param = new Dictionary<string, object>
        {
            {"message", jsonMessages},
            {"sender", jsonSenders},
            {"receptor", jsonReceptors},
            {"type", jsonTypes},
            {"date", date == DateTime.MinValue ? 0 : DateHelper.DateTimeToUnixTimestamp(date)}
        };
            if (localmessageids != null && localmessageids.Count > 0)
            {
                param.Add("localmessageids", StringHelper.Join(",", localmessageids.ToArray()));
            }

            var responsebody = await Execute(path, param);
            var l = JsonConvert.DeserializeObject<ReturnSend>(responsebody);
            if (l.entries == null)
            {
                return new List<SendResult>();
            }
            return l.entries;
        }

        public async Task<List<StatusResult>> Status(List<string> messageids)
        {
            string path = GetApiPath("sms", "status", "json");
            var param = new Dictionary<string, object>
        {
            {"messageid", StringHelper.Join(",", messageids.ToArray())}
        };
            var responsebody = await Execute(path, param);
            var l = JsonConvert.DeserializeObject<ReturnStatus>(responsebody);
            if (l.entries == null)
            {
                return new List<StatusResult>();
            }
            return l.entries;
        }

        public async Task<StatusResult> Status(string messageid)
        {
            var ids = new List<String> { messageid };
            var result = await Status(ids);
            return result.Count == 1 ? result[0] : null;
        }

        public async Task<List<StatusLocalMessageIdResult>> StatusLocalMessageId(List<string> messageids)
        {
            string path = GetApiPath("sms", "statuslocalmessageid", "json");
            var param = new Dictionary<string, object> { { "localid", StringHelper.Join(",", messageids.ToArray()) } };
            var responsebody = await Execute(path, param);
            var l = JsonConvert.DeserializeObject<ReturnStatusLocalMessageId>(responsebody);
            return l.entries;
        }

        public async Task<StatusLocalMessageIdResult> StatusLocalMessageId(string messageid)
        {
            List<StatusLocalMessageIdResult> result = await StatusLocalMessageId(new List<String>() { messageid });
            return result.Count == 1 ? result[0] : null;
        }

        public async Task<List<SendResult>> Select(List<string> messageids)
        {
            var path = GetApiPath("sms", "select", "json");
            var param = new Dictionary<string, object> { { "messageid", StringHelper.Join(",", messageids.ToArray()) } };
            var responsebody = await Execute(path, param);
            var l = JsonConvert.DeserializeObject<ReturnSend>(responsebody);
            if (l.entries == null)
            {
                return new List<SendResult>();
            }
            return l.entries;
        }

        public async Task<SendResult> Select(string messageid)
        {
            var ids = new List<String> { messageid };
            var result = await Select(ids);
            return result.Count == 1 ? result[0] : null;
        }

        public async Task<List<SendResult>> SelectOutbox(DateTime startdate)
        {
            return await SelectOutbox(startdate, DateTime.MaxValue);
        }

        public async Task<List<SendResult>> SelectOutbox(DateTime startdate, DateTime enddate)
        {
            return await SelectOutbox(startdate, enddate, null);
        }

        public async Task<List<SendResult>> SelectOutbox(DateTime startdate, DateTime enddate, String sender)
        {
            String path = GetApiPath("sms", "selectoutbox", "json");
            var param = new Dictionary<string, object>
         {
             {"startdate", startdate == DateTime.MinValue ? 0 : DateHelper.DateTimeToUnixTimestamp(startdate)},
             {"enddate", enddate == DateTime.MinValue ? 0 : DateHelper.DateTimeToUnixTimestamp(enddate)},
             {"sender", sender}
         };
            var responsebody = await Execute(path, param);
            var l = JsonConvert.DeserializeObject<ReturnSend>(responsebody);
            return l.entries;
        }

        public async Task<List<SendResult>> LatestOutbox(long pagesize)
        {
            return await LatestOutbox(pagesize, "");
        }

        public async Task<List<SendResult>> LatestOutbox(long pagesize, String sender)
        {
            var path = GetApiPath("sms", "latestoutbox", "json");
            var param = new Dictionary<string, object> { { "pagesize", pagesize }, { "sender", sender } };
            var responsebody = await Execute(path, param);
            var l = JsonConvert.DeserializeObject<ReturnSend>(responsebody);
            return l.entries;
        }

        public async Task<CountOutboxResult> CountOutbox(DateTime startdate)
        {
            return await CountOutbox(startdate, DateTime.MaxValue, 10);
        }

        public async Task<CountOutboxResult> CountOutbox(DateTime startdate, DateTime enddate)
        {
            return await CountOutbox(startdate, enddate, 0);
        }

        public async Task<CountOutboxResult> CountOutbox(DateTime startdate, DateTime enddate, int status)
        {
            string path = GetApiPath("sms", "countoutbox", "json");
            var param = new Dictionary<string, object>
         {
             {"startdate", startdate == DateTime.MinValue ? 0 : DateHelper.DateTimeToUnixTimestamp(startdate)},
             {"enddate", enddate == DateTime.MinValue ? 0 : DateHelper.DateTimeToUnixTimestamp(enddate)},
             {"status", status}
         };
            var responsebody = await Execute(path, param);
            var l = JsonConvert.DeserializeObject<ReturnCountOutbox>(responsebody);
            if (l.entries == null || l.entries[0] == null)
            {
                return new CountOutboxResult();
            }
            return l.entries[0];
        }

        public async Task<List<StatusResult>> Cancel(List<String> ids)
        {
            string path = GetApiPath("sms", "cancel", "json");
            var param = new Dictionary<string, object>
        {
            {"messageid", StringHelper.Join(",", ids.ToArray())}
        };
            var responsebody = await Execute(path, param);
            var l = JsonConvert.DeserializeObject<ReturnStatus>(responsebody);
            return l.entries;
        }

        public async Task<StatusResult> Cancel(String messageid)
        {
            var ids = new List<String> { messageid };
            var result = await Cancel(ids);
            return result.Count == 1 ? result[0] : null;
        }

        public async Task<List<ReceiveResult>> Receive(string line, int isread)
        {
            String path = GetApiPath("sms", "receive", "json");
            var param = new Dictionary<string, object> { { "linenumber", line }, { "isread", isread } };
            var responsebody = await Execute(path, param);
            var l = JsonConvert.DeserializeObject<ReturnReceive>(responsebody);
            if (l.entries == null)
            {
                return new List<ReceiveResult>();
            }
            return l.entries;
        }

        public async Task<CountInboxResult> CountInbox(DateTime startdate, string linenumber)
        {
            return await CountInbox(startdate, DateTime.MaxValue, linenumber, 0);
        }

        public async Task<CountInboxResult> CountInbox(DateTime startdate, DateTime enddate, String linenumber)
        {
            return await CountInbox(startdate, enddate, linenumber, 0);
        }

        public async Task<CountInboxResult> CountInbox(DateTime startdate, DateTime enddate, String linenumber, int isread)
        {
            var path = GetApiPath("sms", "countoutbox", "json");
            var param = new Dictionary<string, object>
        {
            {"startdate", startdate == DateTime.MinValue ? 0 : DateHelper.DateTimeToUnixTimestamp(startdate)},
            {"enddate", enddate == DateTime.MinValue ? 0 : DateHelper.DateTimeToUnixTimestamp(enddate)},
            {"linenumber", linenumber},
            {"isread", isread}
        };
            var responsebody = await Execute(path, param);
            var l = JsonConvert.DeserializeObject<ReturnCountInbox>(responsebody);
            return l.entries[0];
        }

        public async Task<List<CountPostalCodeResult>> CountPostalCode(long postalcode)
        {
            String path = GetApiPath("sms", "countpostalcode", "json");
            var param = new Dictionary<string, object> { { "postalcode", postalcode } };
            var responsebody = await Execute(path, param);
            var l = JsonConvert.DeserializeObject<ReturnCountPostalCode>(responsebody);
            return l.entries;
        }

        public async Task<List<SendResult>> SendByPostalCode(long postalcode, String sender, String message, long mcistartIndex, long mcicount, long mtnstartindex, long mtncount)
        {
            return await SendByPostalCode(postalcode, sender, message, mcistartIndex, mcicount, mtnstartindex, mtncount, DateTime.MinValue);
        }

        public async Task<List<SendResult>> SendByPostalCode(long postalcode, String sender, String message, long mcistartIndex, long mcicount, long mtnstartindex, long mtncount, DateTime date)
        {
            var path = GetApiPath("sms", "sendbypostalcode", "json");
            var param = new Dictionary<string, object>
        {
            {"postalcode", postalcode},
            {"sender", sender},
            {"message", System.Net.WebUtility.HtmlEncode(message)},
            {"mcistartIndex", mcistartIndex},
            {"mcicount", mcicount},
            {"mtnstartindex", mtnstartindex},
            {"mtncount", mtncount},
            {"date", date == DateTime.MinValue ? 0 : DateHelper.DateTimeToUnixTimestamp(date)}
        };
            var responsebody = await Execute(path, param);
            var l = JsonConvert.DeserializeObject<ReturnSend>(responsebody);
            return l.entries;
        }

        public async Task<AccountInfoResult> AccountInfo()
        {
            var path = GetApiPath("account", "info", "json");
            var responsebody = await Execute(path, null);
            var l = JsonConvert.DeserializeObject<ReturnAccountInfo>(responsebody);
            return l.entries;
        }

        public async Task<AccountConfigResult> AccountConfig(string apilogs, string dailyreport, string debugmode, string defaultsender, int? mincreditalarm, string resendfailed)
        {
            var path = GetApiPath("account", "config", "json");
            var param = new Dictionary<string, object>
        {
            {"apilogs", apilogs},
            {"dailyreport", dailyreport},
            {"debugmode", debugmode},
            {"defaultsender", defaultsender},
            {"mincreditalarm", mincreditalarm},
            {"resendfailed", resendfailed}
        };
            var responsebody = await Execute(path, param);
            var l = JsonConvert.DeserializeObject<ReturnAccountConfig>(responsebody);
            return l.entries;
        }

        public async Task<SendResult> VerifyLookup(string receptor, string token, string template)
        {
            return await VerifyLookup(receptor, token, null, null, template, VerifyLookupType.Sms);
        }
        public async Task<SendResult> VerifyLookup(string receptor, string token, string template, VerifyLookupType type)
        {
            return await VerifyLookup(receptor, token, null, null, template, type);
        }
        public async Task<SendResult> VerifyLookup(string receptor, string token, string token2, string token3, string template)
        {
            return await VerifyLookup(receptor, token, token2, token3, template, VerifyLookupType.Sms);
        }
        public async Task<SendResult> VerifyLookup(string receptor, string token, string token2, string token3, string token10, string template)
        {
            return await VerifyLookup(receptor, token, token2, token3, token10, template, VerifyLookupType.Sms);
        }
        public async Task<SendResult> VerifyLookup(string receptor, string token, string token2, string token3, string template, VerifyLookupType type)
        {
            return await VerifyLookup(receptor, token, token2, token3, null, template, type);
        }

        public async Task<SendResult> VerifyLookup(string receptor, string token, string token2, string token3, string token10, string template, VerifyLookupType type)
        {
            return await VerifyLookup(receptor, token, token2, token3, token10, null, template, type);
        }

        public async Task<SendResult> VerifyLookup(string receptor, string token, string token2, string token3, string token10, string token20, string template, VerifyLookupType type)
        {
            var path = GetApiPath("verify", "lookup", "json");
            var param = new Dictionary<string, object>
            {
                {"receptor", receptor},
                {"template", template},
                {"token", token},
                {"token2", token2},
                {"token3", token3},
                {"token10", token10},
                {"token20", token20},
                {"type", type},
            };
            var responsebody = await Execute(path, param);
            var l = JsonConvert.DeserializeObject<ReturnSend>(responsebody);
            return l.entries[0];
        }


        #region << CallMakeTTS >>

        public async Task<SendResult> CallMakeTTS(string message, string receptor)
        {
            return (await CallMakeTTS(message, new List<string> { receptor }, null, null))[0];
        }
        public async Task<List<SendResult>> CallMakeTTS(string message, List<string> receptor)
        {
            return await CallMakeTTS(message, receptor, null, null);
        }

        public async Task<List<SendResult>> CallMakeTTS(string message, List<string> receptor, DateTime? date, List<string> localid)
        {
            var path = GetApiPath("call", "maketts", "json");
            var param = new Dictionary<string, object>
            {
                {"receptor", StringHelper.Join(",", receptor.ToArray())},
                {"message", System.Net.WebUtility.HtmlEncode(message)},
            };
            if (date != null)
                param.Add("date", DateHelper.DateTimeToUnixTimestamp(date.Value));
            if (localid != null && localid.Count > 0)
                param.Add("localid", StringHelper.Join(",", localid.ToArray()));
            var responseBody = await Execute(path, param);

            return JsonConvert.DeserializeObject<ReturnSend>(responseBody).entries;
        }

        #endregion << CallMakeTTS >>

    }
}