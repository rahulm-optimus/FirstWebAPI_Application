using FirstWebAPI_Application.Data;
using FirstWebAPI_Application.Models;
using FirstWebAPI_Application.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FirstWebAPI_Application.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserDetailsController : ControllerBase
    {
        //sample data for testing only 
        //private string[] names = { "Rahul", "Afsal", "Ashwani", "Nishank", "Manish" };
        //private string[] cities = { "Kullu", "Chennai", "Chandigarh", "Bhuvneshwar", "Odisa" };

        private readonly ILogger<UserDetailsController> _logger;
        private readonly IUserDetailsRepository _userDetailsRepository;

        public UserDetailsController(ILogger<UserDetailsController> logger, IUserDetailsRepository userDetailsRepository)
        {
            _logger = logger;
            _userDetailsRepository = userDetailsRepository;
        }

        [HttpGet(Name = "GetAllUsers")]
        //normal IAction method
        //public IActionResult GetAllUsers()
        //{
        //    var allUsers = _dbContext.UserDetails.ToList();
        //    return Ok(allUsers);

        //}
 
        public async Task<IActionResult> GetAllUsers()
        {
            var allUsers = await _userDetailsRepository.GetAllUsers();
            return Ok(allUsers);

        }


        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetUserByID(int id)
        {
            var userByID = await _userDetailsRepository.GetUserByID(id);

            if (userByID is null)
            {
                return NotFound($"User not found with user ID : {id} ");
            }
            return Ok(userByID);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(UserDetailsDTO user)
        {
            var userEntity = await _userDetailsRepository.CreateUser(user);
            _logger.LogInformation("Item added in the data base");
            return Ok(userEntity);

        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserDTO user)
        {
            var userEntity = await _userDetailsRepository.UpdateUserById(id, user);

            if (userEntity is null)
            {
                return NotFound($"User not found with the ID : {id}");
            }

            return Ok(userEntity);

        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteUserById(int id)
        {
            var userEntity = await _userDetailsRepository.DeleteUserById(id);
            if (userEntity is null)
            {
                return NotFound($"User not found with the ID : {id}");
            }

            return Ok($"User by Id : {id} of Name : {userEntity.Name} is deleted from teh database ");

        }
    };
}

