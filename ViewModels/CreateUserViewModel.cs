using System.ComponentModel.DataAnnotations;

namespace UserControl.ViewModels
{
    public class CreateUserViewModel
    {
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string Username { get; set; }


        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string Password { get; set; }
        

        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string Role { get; set; }
    }
}