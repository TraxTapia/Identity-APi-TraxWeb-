using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trax.Models.Generic.Identity.Request
{
    public class AddRoleRequest
    {
        public AddRoleRequest()
        {
            this.IsNewRole = false;
        }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        public string Application { get; set; }
        [Required]
        [StringLength(50)]
        public string Action { get; set; }
        [Required]
        [StringLength(50)]
        public string Module { get; set; }
        public bool IsNewRole { get; set; }
    }
}
