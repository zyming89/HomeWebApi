#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeWebApi.Data;
using HomeWebApi.Models;
using HomeWebApi.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Newtonsoft.Json;

namespace HomeWebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly HomeWebApiContext _context;

        private IConstomJWTService _IConstomJWTService = null;

        public AuthController(HomeWebApiContext context, IConstomJWTService constomJWTService)
        {
            _context = context;
            _IConstomJWTService = constomJWTService;
        }

        // GET: api/Auth
        [HttpGet]
        [Authorize]
        public async Task<string> Check()
        {
            var claims = User.Claims;
            var userName = User.Identity.Name;
            var code = claims.FirstOrDefault(t => t.Type == "code");
            var user = await _context.User.SingleOrDefaultAsync(u => u.Code == userName); //查询用户
            return JsonConvert.SerializeObject(new
            {
                result = true,
                message = "已授权用户！",
                data = new
                {
                    user = new
                    {
                        id = user.Id,
                        code = user.Code,
                        name = user.Name,
                        email = user.Email,
                    },

                }
            });
        }
        // POST: api/Auth/Login
        [HttpPost]
        public async Task<string> Login(UserRequst userRequst)

        {

            var user = await _context.User.SingleOrDefaultAsync(u => u.Code == userRequst.Code); //查询用户

            if (user == null)
            {
                return JsonConvert.SerializeObject(new
                {
                    result = false,
                    message = "用户名或密码错误！"
                });
            }

            if (user.Code.Equals(userRequst.Code) && user.Password.Equals(userRequst.Password))
            {
                string accessToken = _IConstomJWTService.GetToken(userRequst.Code, userRequst.Password);
                return JsonConvert.SerializeObject(new
                {
                    result = true,
                    message = "登录成功！",
                    data = new
                    {
                        user = new
                        {
                            id = user.Id,
                            code = user.Code,
                            name = user.Name,
                            email = user.Email,
                        },
                        accessToken
                    }
                });
            }
            else
            {
                return JsonConvert.SerializeObject(new
                {
                    result = false,
                    message = "用户名或密码错误！",
                });
            }

        }

        // GET: api/Auth/Logout
        [HttpGet]
        [Authorize]
        public string Logout()
        {
            return JsonConvert.SerializeObject(new
            {
                result = true,
                message = "已退出登录！",
                data = new { }
            });
        }

        // PUT: api/Auth/5

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Auth
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.User.Add(user);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserExists(user.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Auth/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(string id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
