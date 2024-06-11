namespace HospitalManagementSystem.DTOs
{
    public class AppointmentDTO
    {
        public int Id { get; set; }
        public string PatientId { get; set; }
        public string DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public bool IsCancelled { get; set; }
    }
}
