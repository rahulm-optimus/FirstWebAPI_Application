using FirstWebAPI_Application.Data;
using FirstWebAPI_Application.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstWebAPI_Application.Repository
{
    public class UserDetailsRepository : IUserDetailsRepository
    {

        private readonly UsersDBContext _dbContext;

        public UserDetailsRepository(UsersDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<UserDetails>> GetAllUsers()
        {
            return await _dbContext.UserDetails.ToListAsync();
        }

        public async Task<UserDetails?> GetUserByID(int id)
        {
            return await _dbContext.UserDetails.FindAsync(id);
        }

        public async Task<UserDetails> CreateUser(UserDetailsDTO user)
        {
            var userEntity = new UserDetails()
            {
                Name = user.Name,
                City = user.City,
            };

            await _dbContext.UserDetails.AddAsync(userEntity);

            await _dbContext.SaveChangesAsync();

            return userEntity;
        }

        public async Task<UserDetails?> UpdateUserById(int id, UpdateUserDTO updateUserDTO)
        {

            var userEntity = await _dbContext.UserDetails.FindAsync(id);

            if (userEntity != null)
            {

                userEntity.Name = updateUserDTO.Name;
                userEntity.City = updateUserDTO.City;

                await _dbContext.SaveChangesAsync();
                return userEntity;
            }

            return null;

        }

        public async Task<UserDetails?> DeleteUserById(int id)
        {

            var userEntity = await _dbContext.UserDetails.FindAsync(id);

            if (userEntity != null)
            {
                _dbContext.UserDetails.Remove(userEntity);
                await _dbContext.SaveChangesAsync();
                return userEntity;

            }

            return null;

        }
    }
}
