namespace HospitalManagementSystem.DTOs
{
    public class CreatePrescriptionDTO
    {
        public string PatientId { get; set; }
        public string DoctorId { get; set; }
        public string Details { get; set; }
        public DateTime Date { get; set; }
    }
}
