using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trax.Models.Generic.Identity.Enum;

namespace Trax.Models.Generic.Identity.Request
{
    public class AddRolesRequestDTO
    {
        public ApplicationRequestDTO Application { get; set; }
        public EnumActionRole ActionRole { get; set; }
    }
}
