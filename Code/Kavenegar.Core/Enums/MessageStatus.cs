namespace Kavenegar.Core.Enums;

public enum MessageStatus
{
    Unknown = 0,
    Queued = 1,
    Scheduled = 2,
    SentToCenter = 4,
    Delivered = 10,
    Undelivered = 11,
    Canceled = 13,
    Filtered = 14,
    Received = 50,
    Incorrect = 100
}