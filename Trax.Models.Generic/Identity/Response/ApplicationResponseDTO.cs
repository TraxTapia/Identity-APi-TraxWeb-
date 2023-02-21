using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trax.Models.Generic.Identity.Response
{
    public class ApplicationResponseDTO
    {
        public ApplicationResponseDTO()
        {
            this.Roles = new List<RoleDTO>();
        }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public List<RoleDTO> Roles { get; set; }
    }
}
