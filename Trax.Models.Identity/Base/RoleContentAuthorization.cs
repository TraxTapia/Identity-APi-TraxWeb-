using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trax.Models.Identity.Identity;

namespace Trax.Models.Identity.Base
{
    [Table("RoleContentAuthorization")]
    public class RoleContentAuthorization
    {
        public RoleContentAuthorization()
        {
            this.Visible = true;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Module { get; set; }

        [Required]
        [StringLength(50)]
        public string Action { get; set; }

        public bool Visible { get; set; }

        public ApplicationRole Role { get; set; }
    }
}
