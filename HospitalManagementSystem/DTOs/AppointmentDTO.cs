namespace HospitalManagementSystem.DTOs
{
    public class AppointmentDTO
    {
        public int Id { get; set; }
        public string PatientEmail { get; set; }
        public string DoctorEmail { get; set; }
        public DateTime AppointmentDate { get; set; }
        public bool IsCancelled { get; set; }
    }
}
