using System.ComponentModel.DataAnnotations;

namespace UserControl.ViewModels
{
    public class LoginUserViewModel
    {
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string Password { get; set; }
    }
}