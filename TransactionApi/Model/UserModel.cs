using System.ComponentModel.DataAnnotations;

namespace TransactionApi.Model
{
    public class UserModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = default!;

        [Required]
        public string Password { get; set; } = default!;
    }
}
