namespace HospitalManagementSystem.DTOs
{
    public class CreateAppointmentDTO
    {
        public string PatientEmail { get; set; }
        public string DoctorEmail { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string? PatientId { get; set; }
        public string? DoctorId { get; set; }
    }
}
