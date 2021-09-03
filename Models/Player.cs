using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PlayersApi.Models
{

    /* Player info
     */
    public class Player
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public String Name { get; set; }
        public int Height { get; set; }
        public DateTime Birthday { get; set; }

        public string AvatarURL { get; set; }

        public long? TeamId { get; set; }
    }
}
