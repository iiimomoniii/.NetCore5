using System.ComponentModel.DataAnnotations;

namespace Hero_Project.NetCore5.DTOs.Account
{
    public class RegisterRequest
    {
        [Required]
        [EmailAddress]
        
        public string Username { get; set; }

        [Required]
        [MinLength(8)]
        
        public string Password { get; set; }

        public int RoleId { get; set; }
    }
}