using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EMF_Portal_API.Model
{
    public class DocumentUpload
    {
         
        [Key]
        public int DocumentID { get; set; }
        public string FileName { get; set; }
        public string ContentType {get;set;}
        public long? Filesize { get; set; }

        //[ForeignKey(nameof(UserID))]
        //public int UserID { get; set; }

        //public ICollection<UserDetails> UserDetails { get; set; }
    }
}
