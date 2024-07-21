using FirstWebAPI_Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace FirstWebAPI_Application.Repository
{
    public interface IUserDetailsRepository
    {
        Task<List<UserDetails>> GetAllUsers();
        Task<UserDetails?> GetUserByID(int id);
        Task<UserDetails> CreateUser(UserDetailsDTO user);
        Task<UserDetails?> UpdateUserById(int id,UpdateUserDTO updateUserDTO);
        Task<UserDetails?> DeleteUserById(int id);
    }
}
