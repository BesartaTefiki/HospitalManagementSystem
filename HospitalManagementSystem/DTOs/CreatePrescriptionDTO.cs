namespace HospitalManagementSystem.DTOs
{
    public class CreatePrescriptionDTO
    {
        public string PatientEmail { get; set; }
        public string DoctorEmail { get; set; }
        public string Details { get; set; }
        public DateTime Date { get; set; }
        public string? PatientId { get; set; }
        public string? DoctorId { get; set; }
    }

}
