using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Roles : IdentityRole
    {
        public string RoleCode { get; set; }
        public DateTimeOffset Create_At { get; set; }
        public DateTimeOffset Update_At { get; set; }
        public string Update_By { get; set; }
        public string Create_By { get; set; }
    }
}
