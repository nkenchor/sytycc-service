
using Sytycc_Service.Domain;

namespace Sytycc_Service.Api;

public interface IUserService{
      

    Task<string> CreateUser(CreateUserDto user);
    Task<string> UpdateUser(string reference, UpdateUserDto user);
    Task<string> DeleteUser(string reference);
    Task<UserDto> GetUserByReference(string reference);
    Task<List<UserDto>> GetUserList(int page);
    Task<List<UserDto>> SearchUserList(int page, string title);
    Task<UserDto> GetUserByUserName(string username);
    Task<string?> Login(LoginUserDto userDto);
      
}

