using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Models.ConfigurationModels
{
    public class SmtpConfiguration
    {
        public string Section { get; set; } = "SmtpSettings";
        public string From { get; set; } = string.Empty;
        public string Host { get; set; } = string.Empty;
        public string Port { get; set; } = string.Empty;
        public string DomainURL { get; set; } = string.Empty;
    }
}
