using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Loadability.Models
{
    public class FileModel
    {
        public int FileModelId { get; set; }
        public string FileTitle { get; set; }
        public string FileLocation { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}