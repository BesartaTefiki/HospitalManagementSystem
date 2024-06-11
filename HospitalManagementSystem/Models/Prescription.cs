using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Models
{
    public class Prescription
    {
        [Key]
        public int Id { get; set; }
        public string PatientId { get; set; }
        public string DoctorId { get; set; }
        public string Details { get; set; }
        public DateTime Date { get; set; }

        public IdentityUser Patient { get; set; }
        public IdentityUser Doctor { get; set; }
    }
}
