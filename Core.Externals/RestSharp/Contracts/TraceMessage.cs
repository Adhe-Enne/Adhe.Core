using System;

namespace Core.Externals
{
    public class TraceMessage
    {
        public TraceMessage()
        {
            Date = DateTime.Now;
        }

        public DateTime Date { get; set; }
        public Guid TraceId { get; set; }
        public string Source { get; set; }
        public string URI { get; set; }
        public string Method { get; set; }
        public string Type { get; set; }
        public string ContentType { get; set; }
        public string ContentValue { get; set; }
        public bool Error { get; set; }
        public string Exception { get; set; }
    }
}
