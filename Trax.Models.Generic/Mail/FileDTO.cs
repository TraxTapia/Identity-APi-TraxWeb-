using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trax.Models.Generic.Mail
{
    public class FileDTO
    {
        [StringLength(100)]
        public string FileName { get; set; }
        public List<byte> Content { get; set; }
    }
}
