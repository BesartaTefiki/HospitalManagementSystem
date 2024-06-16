namespace HospitalManagementSystem.DTOs
{
    public class PrescriptionDTO
    {
        public int Id { get; set; }
        public string PatientEmail { get; set; }
        public string DoctorEmail { get; set; }
        public string Details { get; set; }
        public DateTime Date { get; set; }
    }


}

