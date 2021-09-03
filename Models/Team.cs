using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlayersApi.Models
{
    public class Team
    {
        [Key]
        public long Id { get; set; }

        
        [Required]
        public string Name { get; set; }
        public string AvatarURL { get; set; }

        public virtual ICollection<Player>? Players { get; set; }
        
    }
}
