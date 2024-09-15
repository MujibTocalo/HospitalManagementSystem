using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Models
{
    public class Patient
    {
        [Key]
        public int? Id { get; set; }
        [Display(Name = "First Name")]
        public required string Name { get; set; }
        public required int Age { get; set; }
        [Display(Name = "Date of Birth")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public required DateTime DateOfBirth { get; set; }
        public required string Address { get; set; }
        public required string Gender { get; set; }
        [Display(Name = "Contact Number")]
        public required string ContactNumber { get; set; }

    }
}
