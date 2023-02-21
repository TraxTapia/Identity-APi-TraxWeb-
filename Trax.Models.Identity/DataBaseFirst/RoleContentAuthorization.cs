using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trax.Models.Identity.DataBaseFirst
{
    [Table("RoleContentAuthorization")]
    public partial class RoleContentAuthorization
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Module { get; set; }

        [Required]
        [StringLength(50)]
        public string Action { get; set; }

        public bool Visible { get; set; }

        [StringLength(128)]
        public string Role_Id { get; set; }

        public virtual AspNetRoles AspNetRoles { get; set; }
    }
}
