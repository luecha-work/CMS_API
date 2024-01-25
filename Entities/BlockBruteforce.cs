using System;
using System.Collections.Generic;

namespace Entities;

public partial class BlockBruteforce
{
    public Guid BlockForceId { get; set; }

    public string Username { get; set; } = null!;

    public int Count { get; set; }

    /// <summary>
    /// L (Locked): ถูกล็อก
    /// U (UnLock): ไม่ล็อก
    /// </summary>
    public string Status { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public DateTime? LockedTime { get; set; }

    public DateTime? UnLockTime { get; set; }
}
