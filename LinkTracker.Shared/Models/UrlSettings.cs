using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LinkTracker.Shared.Models
{
    public class UrlSettings
    {
        public string[] BaseAPIURLs { get; set; }
        public string[] BaseWASMURLs { get; set; }
    }
}
