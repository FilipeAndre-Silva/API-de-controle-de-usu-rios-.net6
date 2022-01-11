using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserControl.Data;
using UserControl.Models;
using UserControl.Services;
using UserControl.ViewModels;

namespace UserControl.Controller
{
    [ApiController]
    [Route("v1/users")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAsync([FromServices] AppDbContext context)
        {
            var users = await context.Users.AsNoTracking().ToListAsync();

            if (!users.Any())
            {
                return NoContent();
            }

            users.ForEach(u => u.Password = "");
            return Ok(users);
        }

        [HttpGet]
        [Route("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByIdAsync(int id, [FromServices] AppDbContext context)
        {
            var user = await context.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            user.Password = string.Empty;
            return Ok(user);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> PostAsync([FromServices] AppDbContext context,
                                                   [FromServices] IMapper mapper,
                                                   [FromBody] CreateUserViewModel createUserViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = mapper.Map<User>(createUserViewModel);
            user.Password = EncryptPassword.EcryptUserPassword(user.Password);

            try
            {
                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();
                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("PostValidationDomain")]
        [AllowAnonymous]
        public async Task<IActionResult> PostValidationDomainAsync([FromServices] AppDbContext context,
                                                                   [FromServices] IMapper mapper,
                                                                   [FromBody] CreateUserViewModelNoValidation createUserViewModel)
        {
            var user = mapper.Map<User>(createUserViewModel);

            if (!user.IsValid())
            {
                var validationErrors = user.validationResult.Errors.Select(e => e.ErrorMessage);
                return BadRequest(validationErrors);
            }

            user.Password = EncryptPassword.EcryptUserPassword(user.Password);

            try
            {
                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();
                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = "employee,manager")]
        public async Task<IActionResult> PutAsync(int id, [FromServices] AppDbContext context,
                                                          [FromServices] IMapper mapper,
                                                          [FromBody] UpdateUserViewModel updateUserViewModel)
        {
            if (id != updateUserViewModel.Id)
            {
                return NotFound(new { message = "Usuário não encontrado." });
            }

            var userSearched = await context.Users.SingleOrDefaultAsync(u => u.Id == id);

            if (userSearched == null)
            {
                return NotFound();
            }

            try
            {   
                userSearched.Role = updateUserViewModel.Role;
                context.Users.Update(userSearched);
                await context.SaveChangesAsync();

                var userViewModel = mapper.Map<UpdateUserViewModel>(userSearched);
                return Ok(userViewModel);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> DeleteAsync(int id, [FromServices] AppDbContext context,
                                                             [FromServices] IMapper mapper)
        {
            var userSearched = await context.Users.SingleOrDefaultAsync(u => u.Id == id);

            if (userSearched == null)
            {
                return NotFound();
            }

            try
            {   
                context.Users.Remove(userSearched);
                await context.SaveChangesAsync();

                var userViewModel = mapper.Map<UpdateUserViewModel>(userSearched);
                return Ok(userViewModel);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> LoginAsync([FromServices] IConfiguration configuration,
                                                    [FromServices] AppDbContext context,
                                                    [FromServices] IMapper mapper,
                                                    [FromBody] LoginUserViewModel loginUserViewModel)
        {
            loginUserViewModel.Password = EncryptPassword.EcryptUserPassword(loginUserViewModel.Password);
            var user = context.Users.AsNoTracking().Where(x => x.Username == loginUserViewModel.Username &&
												x.Password == loginUserViewModel.Password)
												.FirstOrDefault();

            if (user == null)
            {
                return NotFound();
            }
            
            user.Password = string.Empty;
            var token = new TokenService(configuration).GenerateToken(user);

            return new
            {
                user = user,
                token = token
            };
        }
    }
}