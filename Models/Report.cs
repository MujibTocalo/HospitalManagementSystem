using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Models
{
    public class Report
    {
        [Key]
        public int Id { get; set; }
        public int BaranggayId { get; set; }
        public string ReportDate { get; set; }
        public string SubmittedBy { get; set; }
        public int ServiceId { get; set; }
        public string Summary { get; set; }
        public string ReportFile { get; set; }

        public virtual Baranggay? Baranggay { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }
        public virtual Service? Service { get; set; }
    }
}
