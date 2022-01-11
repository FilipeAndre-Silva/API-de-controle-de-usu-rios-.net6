using System.ComponentModel.DataAnnotations;

namespace UserControl.ViewModels
{
    public class CreateUserViewModelNoValidation
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}