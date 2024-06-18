namespace HospitalManagementSystem.DTOs
{
    public class AuthResponseDTO
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;

        public IList<string> Roles { get; set; }
    }
}
