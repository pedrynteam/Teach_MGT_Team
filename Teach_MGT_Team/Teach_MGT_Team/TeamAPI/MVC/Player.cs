using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Teach_MGT_Team.TeamAPI.MVC
{
    public class Player
    {
        [Key]
        public int PlayerId { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        // 1 to Many - Steven Sandersons
        public int TeamId { get; set; }
        [ForeignKey("TeamId")]
        [JsonIgnore] // To avoid circular calls. Team -> Player -> Team -> Player
        public virtual Team Team { get; set; }

    }
}
