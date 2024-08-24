namespace HospitalManagementSystem.Models
{
    public class Morbidity
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public int ReportId { get; set; }
        public string NotificationDate { get; set; }
        public virtual Report? Report { get; set; }
    }
}
