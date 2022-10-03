using System.IO;

namespace Email.Core.Models
{
    public class Attachment
    {
        public string Filename { get; set; }
        public Stream Data { get; set; }
        public string ContentType { get; set; }
        public string ContentId { get; set; }
    }
}