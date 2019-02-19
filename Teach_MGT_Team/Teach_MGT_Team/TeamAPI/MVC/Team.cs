using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Teach_MGT_Team.TeamAPI.MVC
{
    public class Team
    {
        [Key]        
        public int TeamId { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        // 1 to Many - Steven Sandersons
        public virtual List<Player> Players { get; set; }
    }
}
