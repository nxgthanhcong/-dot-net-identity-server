using System.ComponentModel.DataAnnotations;

namespace Security.Models.RequestModels
{
    public class ManualSignupReq
    {
        [Required]
        [EmailAddress]
        public string Username { get; set; }
        
        [Required]
        [RegularExpression("^(?=.*[@])(?=.*[a-zA-Z]).{8,}$")]
        public string Password { get; set; }

    }
}
