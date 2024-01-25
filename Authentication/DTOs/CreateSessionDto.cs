using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.DTOs
{
    public class CreateSessionDto
    {
        public string AccountId { get; set; } = null!;

        public DateTime LoginAt { get; set; }

        public string Platform { get; set; } = null!;

        public string Os { get; set; } = null!;

        public string Browser { get; set; } = null!;

        public string LoginIp { get; set; } = null!;

        public string CreateBy { get; set; } = null!;

        public DateTime CreateAt { get; set; }

        public string UpdateBy { get; set; } = null!;

        public DateTime UpdateAt { get; set; }

        public DateTime IssuedTime { get; set; }

        public DateTime ExpirationTime { get; set; }
        public string SessionStatus { get; set; } = null!;
    }
}
