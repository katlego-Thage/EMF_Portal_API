using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EMF_Portal_API.Model
{
    public class Race
    {
        public Race()
        {
            UserDetails = new HashSet<UserDetails>();
        }

        [Key]
        
        public int RaceID { get; set; }

        public string Discription { get; set; }

        public ICollection<UserDetails> UserDetails { get; set; }
    }
}
