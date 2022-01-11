using AutoMapper;
using UserControl.Models;

namespace UserControl.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ViewModels.CreateUserViewModel, User>().ReverseMap();
            CreateMap<ViewModels.UpdateUserViewModel, User>().ReverseMap();
            CreateMap<ViewModels.CreateUserViewModelNoValidation, User>().ReverseMap();
        }
    }
}