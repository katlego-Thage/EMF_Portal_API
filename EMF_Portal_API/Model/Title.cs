using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EMF_Portal_API.Model
{
    public class Title
    {
        public Title()
        {
            Userdetails = new HashSet<UserDetails>();
        }

        [Key]
        public int TitleID { get; set; }

        public string Discription { get; set; }

        public ICollection<UserDetails> Userdetails { get; set; }
    }
}
