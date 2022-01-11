using System.ComponentModel.DataAnnotations;

namespace UserControl.ViewModels
{
    public class UpdateUserViewModel
    {
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public int Id { get; set; }
        

        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string Role { get; set; }
    }
}