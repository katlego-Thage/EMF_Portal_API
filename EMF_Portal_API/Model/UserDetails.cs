using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EMF_Portal_API.Model
{
    public partial class UserDetails
    {
        public UserDetails()
        {
            Titles = new HashSet<Title>();
            Races = new HashSet<Race>();
            Genders = new HashSet<Gender>();
            Qualifications = new HashSet<Qualification>();
            Disabilities = new HashSet<Disability>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }

        [Required]
        [DisplayName("FirstName")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("LastName")]
        public string LastName { get; set; }

        [Required]
        [DisplayName("Title")]
        [ForeignKey(nameof(TitleID))]
        public int TitleID { get; set; }

        [Required]
        [DisplayName("Initials")]
        public string Initials { get; set; }

        [Required]
        [Range(typeof(Int64), "0", "9999999999999", ErrorMessage = "{0} not valid")]
        [DisplayName("IDNumber")]
        public string IDNumber { get; set; }

        [Required]
        [DisplayName("Disability")]
        [ForeignKey(nameof(DisabilityID))]
        public int DisabilityID { get; set; }

        [Required]
        [DisplayName("Gender")]
        [ForeignKey(nameof(GenderID))]
        public int GenderID { get; set; }

        [Required]
        [DisplayName("Race")]
        [ForeignKey(nameof(RaceID))]
        public int RaceID { get; set; }

        [Required]
        [DisplayName("CellNumber")]
        public string CellNumber { get; set; }

        [Required]
        [DisplayName("Email Address")]
        public string Email { get; set; }

        [Required]
        [DisplayName("Qualification")]
        [ForeignKey(nameof(QualificationID))]
        public int QualificationID { get; set; }

        [Required]
        [DisplayName("HomeAddress1")]
        public string HomeAddress1 { get; set; }

        [Required]
        [DisplayName("HomeAddress2")]
        public string HomeAddress2 { get; set; }

        [Required]
        [DisplayName("HomeAddress3")]
        public string HomeAddress3 { get; set; }

        [Required]
        [DisplayName("Code")]
        public string Code { get; set; }

        public ICollection<Title> Titles { get; set; }

        public ICollection<Race> Races { get; set; }

        public ICollection<Gender> Genders { get; set; }

        public ICollection<Qualification> Qualifications { get; set; }

        public ICollection<Disability> Disabilities { get; set; }
    }
}
