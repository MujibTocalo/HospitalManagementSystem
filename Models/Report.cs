using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Models
{
    public class Report
    {
        [Key]
        public int Id { get; set; }
        public DateTime ReportDate { get; set; }
        public int PatientId { get; set; }
        public string SubmittedBy { get; set; }
        public int ServiceId { get; set; }
        public string Summary { get; set; }

        public virtual ApplicationUser? ApplicationUser { get; set; }
        public virtual Service? Service { get; set; }
        public virtual Patient? Patient { get; set; }
    }
}
