using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trax.Models.Generic.Mail
{
    public class EmailRequestDTO
    {
        [StringLength(65)]
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsBodyHtml { get; set; }
        public List<FileDTO> Attached { get; set; }
        public List<string> To { get; set; }
        public List<string> CC { get; set; }
        public List<string> CCO { get; set; }
    }
}
