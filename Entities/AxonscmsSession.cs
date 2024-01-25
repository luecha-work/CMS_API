using System;
using System.Collections.Generic;

namespace Entities;

public partial class AxonscmsSession
{
    public Guid SessionId { get; set; }

    public string AccountId { get; set; } = null!;

    public DateTime LoginAt { get; set; }

    public string? Platform { get; set; }

    public string? Os { get; set; }

    public string? Browser { get; set; }

    public string LoginIp { get; set; } = string.Empty;

    public string? Token { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? CreateAt { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdateAt { get; set; }

    public DateTime IssuedTime { get; set; }

    public DateTime ExpirationTime { get; set; }

    /// <summary>
    /// B (Blocked): Session ยังไม่ได้ใช้งาน
    /// A (Active): Session กำลังใช้งานอยู่
    /// E (Expired): Session หมดอายุแล้ว
    /// </summary>
    public string SessionStatus { get; set; } = null!;
}
