using Microsoft.AspNetCore.Identity;
using UserControl.Validations;

namespace UserControl.Models
{
    public class User : Entity
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public bool IsValid()
        {   
            validationResult = new UserValidator().Validate(this);
            return validationResult.IsValid;
        }
    }
}