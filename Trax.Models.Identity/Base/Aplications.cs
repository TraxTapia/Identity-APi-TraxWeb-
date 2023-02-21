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
    [Table("Aplications")]
    public class Aplications
    {
        public Aplications()
        {
            this.Visible = true;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public bool Visible { get; set; }

        public List<ApplicationRole> Roles { get; set; }
    }
}
