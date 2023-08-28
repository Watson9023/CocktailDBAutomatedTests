
using Microsoft.Extensions.Logging;
using System;
using Newtonsoft.Json;

namespace CocktailDBTests.Logging
{
    public class LogEntry
    {
        public LogLevel LogLevel { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }

        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(this); // Use Newtonsoft.Json for serialization
        }
    }

}
