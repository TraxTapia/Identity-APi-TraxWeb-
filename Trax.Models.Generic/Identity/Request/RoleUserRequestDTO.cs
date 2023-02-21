using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trax.Models.Generic.Identity.Request
{
    public partial class RoleUserRequestDTO
    {
        public RoleUserRequestDTO()
        {
            this.Roles = new List<string>();
        }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }
        //[Required]
        //[stirngl]
        //public string  Name { get; set; }
        [Required]
        [StringLength(50)]
        public string Application { get; set; }

        public List<string> Roles { get; set; }


    }
}
