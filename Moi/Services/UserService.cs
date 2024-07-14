using AutoMapper;
using GaragesStructure.DATA.DTOs;
using e_parliament.Interface;
using GaragesStructure.DATA.DTOs.User;
using GaragesStructure.Entities;
using GaragesStructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace GaragesStructure.Services{
    public interface IUserService{
        Task<(UserDto? user, string? error)> Login(LoginForm loginForm);
        Task<(AppUser? user, string? error)> DeleteUser(Guid id);
        Task<(UserDto? UserDto, string? error)> Register(RegisterForm registerForm);
        Task<(AppUser? user, string? error)> UpdateUser(UpdateUserForm updateUserForm, Guid id);
        Task<(AppUser? user, string? error)> ChangeProfileInfo(Guid id , ChangeProfileInfo changeProfileInfo);
        Task<(UserDto? user, string? error)> GetUserById(Guid id);


        Task<(List<UserDto>? user, int? totalCount, string? error)> GetAll(UserFilter filter);

        Task<(AppUser? user, string? error)> ChangeUserState(Guid id, bool isActive);

    }

    public class UserService : IUserService{
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public UserService(
            IRepositoryWrapper repositoryWrapper, 
            IMapper mapper,
            ITokenService tokenService 
            ) {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task<(UserDto? user, string? error)> Login(LoginForm loginForm) {
            var user = await _repositoryWrapper.User.Get(u => u.Email == loginForm.Email, i => i.Include(x => x.Role));
            if (user == null) return (null, "User not found");
            if (user.IsActive == false) return (null, "User is not active");
            if (!BCrypt.Net.BCrypt.Verify(loginForm.Password, user.Password)) return (null, "Wrong password");
            var userDto = _mapper.Map<UserDto>(user);
            userDto.Token = _tokenService.CreateToken(user, user.Role);
            return (userDto, null);
        }

        public async Task<(AppUser? user, string? error)> DeleteUser(Guid id) {
            var user = await _repositoryWrapper.User.Get(u => u.Id == id);
            if (user == null) return (null, "User not found");
            await _repositoryWrapper.User.SoftDelete(id);
            return (user, null);
        }

        public async Task<(UserDto? UserDto, string? error)> Register(RegisterForm registerForm) {
            var role = await _repositoryWrapper.Role.Get(r => r.Id == registerForm.Role);
            if (role == null) return (null, "Role not found");

         
            var user = await _repositoryWrapper.User.Get(u => u.Email == registerForm.Email);
            if (user != null) return (null, "User already exists");
            var newUser = new AppUser
            {
                Email = registerForm.Email,
                FullName = registerForm.FullName,
                Password = BCrypt.Net.BCrypt.HashPassword(registerForm.Password),
            };
            // set role 
            newUser.RoleId = role.Id;

            await _repositoryWrapper.User.CreateUser(newUser);
            newUser.Role = role;
            var userDto = _mapper.Map<UserDto>(newUser);
            userDto.Token = _tokenService.CreateToken(newUser, role);
            return (userDto, null);
        }

        public async Task<(AppUser? user, string? error)> UpdateUser(UpdateUserForm updateUserForm, Guid id) {
            var user = await _repositoryWrapper.User.Get(u => u.Id == id);
            if (user == null) return (null, "User not found");
            if (updateUserForm.Password != null)
            {
                updateUserForm.Password = BCrypt.Net.BCrypt.HashPassword(updateUserForm.Password);
            }

            if (updateUserForm.Role != null)
            {
                user.RoleId = updateUserForm.Role;
                updateUserForm.Role = null;
            }
            
            if(updateUserForm.IsActive != null)
            {
                user.IsActive = updateUserForm.IsActive;
            }

            user = _mapper.Map(updateUserForm, user);
            await _repositoryWrapper.User.UpdateUser(user);
            return (user, null);
        }

        public async Task<(UserDto? user, string? error)> GetUserById(Guid id) {
            var user = await _repositoryWrapper.User.Get(u => u.Id == id);
            if (user == null) return (null, "User not found");
            var userDto = _mapper.Map<UserDto>(user);
            return (userDto, null);
        }
        
        public async Task<(AppUser? user, string? error)> ChangeProfileInfo(Guid id , ChangeProfileInfo changeProfileInfo) {
            var user = await _repositoryWrapper.User.Get(u => u.Id == id);
            if (user == null) return (null, "User not found");
            if (changeProfileInfo.Password != null)
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(changeProfileInfo.Password);
            }

            await _repositoryWrapper.User.UpdateUser(user);
            return (user, null);
        }

        public async Task<(List<UserDto>? user, int? totalCount, string? error)> GetAll(UserFilter filter)
        {
            var (users , totalCount) = await _repositoryWrapper.User.GetAll<UserDto>(
                x => (
                    (filter.FullName == null || x.FullName.Contains(filter.FullName)) &&
                    (filter.Email == null || x.Email.Contains(filter.Email)) &&
                    (filter.RoleId == null || x.RoleId.Equals(filter.RoleId)) &&
                    (filter.IsActive == null || x.IsActive.Equals(filter.IsActive))
                    ) ,
                filter.PageNumber , filter.PageSize, filter.Deleted);
            return (users , totalCount , null);
        }
        
        
        public async Task<(AppUser? user, string? error)> ChangeUserState(Guid id, bool isActive)
        {
            var user = await _repositoryWrapper.User.Get(u => u.Id == id);
            if (user == null) return (null, "User not found");
            user.IsActive = isActive;
            await _repositoryWrapper.User.UpdateUser(user);
            return (user, null);
        }
    }
}