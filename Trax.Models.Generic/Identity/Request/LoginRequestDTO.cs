using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Trax.Models.Generic.Identity.Request
{
    public class LoginRequestDTO
    {
        [Required]
        [StringLength(250)]
        public string username { get; set; }
        [Required]
        [StringLength(50)]
        public string password { get; set; }
        [Required]
        [StringLength(50)]
        public string grant_type { get; set; }
    }
}
