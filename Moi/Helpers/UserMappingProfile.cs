using System.Text.Json.Nodes;
using AutoMapper;
using BackEndStructuer.DATA.DTOs;
using BackEndStructuer.Entities;
using GaragesStructure.DATA.DTOs.Country;
using GaragesStructure.DATA.DTOs.roles;
using GaragesStructure.DATA.DTOs.User;
using GaragesStructure.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OneSignalApi.Model;

// here to implement


namespace GaragesStructure.Helpers
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {

            CreateMap<AppUser, UserDto>();
            CreateMap<RegisterForm, App>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<UpdateUserForm, AppUser>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Role, RoleDto>();
            CreateMap<AppUser, AppUser>();

            CreateMap<Permission, PermissionDto>();
            CreateMap<Permission, MyPermissionDto>();

            CreateMap<Country, CountryDto>();
            CreateMap<CountryForm, Country>();
            CreateMap<CountryUpdate, Country>()
                .ForAllMembers(m => m.Condition(p => p != null))
                ;


            // here to add
CreateMap<Music, MusicDto>();
CreateMap<MusicForm,Music>();
CreateMap<MusicUpdate,Music>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
CreateMap<Category, CategoryDto>();
CreateMap<CategoryForm,Category>();
CreateMap<CategoryUpdate,Category>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        }
    }
}
