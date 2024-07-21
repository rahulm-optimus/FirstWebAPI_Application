using FirstWebAPI_Application.Data;
using FirstWebAPI_Application.Models;
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

        //private readonly ILogger<UserDetailsController> _logger;

        //public UserDetailsController(ILogger<UserDetailsController> logger)
        //{
        //    _logger = logger;
        //}

        //[HttpGet(Name = "GetUserDetails")]
        //public IEnumerable<UserDetails> Get()
        //{
        //    return Enumerable.Range(0, 4).Select(index => new UserDetails
        //    {
        //        Id = index,
        //        Name = names[index],
        //        City = cities[index],
        //    })
        //    .ToArray();
        //}

        private readonly UsersDBContext _dbContext;
        public UserDetailsController(UsersDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet(Name = "GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            var allUsers = _dbContext.UserDetails.ToList();
            return Ok(allUsers);

        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult getUserByID(int id)
        {
            var userByID = _dbContext.UserDetails.Find(id);

            if (userByID is null)
            {
                return NotFound($"User not found with user ID : {id} ");
            }
            return Ok(userByID);
        }

        [HttpPost]
        public IActionResult AddUser(UserDetailsDTO user)
        {
            var userEntity = new UserDetails()
            {
                Name = user.Name,
                City = user.City,
            };

            _dbContext.UserDetails.Add(userEntity);
            _dbContext.SaveChanges();

            return Ok(userEntity);

        }

        [HttpPut]
        [Route("{id:int}")]
        public IActionResult UpdateUser(int id, UpdateUserDTO user)
        {
            var userEntity = _dbContext.UserDetails.Find(id);
            if (userEntity is null)
            {
                return NotFound($"User not found with the ID : {id}");
            }


            userEntity.Name = user.Name;
            userEntity.City = user.City;

            _dbContext.SaveChanges();

            return Ok(userEntity);

        }

        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult UpdateUserint(int id)
        {
            var userEntity = _dbContext.UserDetails.Find(id);
            if (userEntity is null)
            {
                return NotFound($"User not found with the ID : {id}");
            }


            _dbContext.UserDetails.Remove(userEntity);
            _dbContext.SaveChanges();

            return Ok($"User by Id : {id} of Name : {userEntity.Name} is deleted from teh database ");

        }
    };
}

