using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trax.Models.Generic.Identity.Request
{
    public class SaveChangesRolesByUserRequestDTO
    {
        [Required]
        [StringLength(250)]
        public string Email { get; set; }

        [Required]
        public List<string> Items { get; set; }
    }
}
