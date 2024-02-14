using FacilitEase.Data;
using FacilitEase.Models.EntityModels;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Resources;
using System.Security.Claims;
using System.Text;

namespace FacilitEase.Services
{
    public class LoginService : ILoginService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        ResourceManager resourceManager = new ResourceManager("FacilitEase.Resource", typeof(Program).Assembly);

        public LoginService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public object CheckUser(string username)
        {
            var user = _context.TBL_USER.FirstOrDefault(e => e.Email == username);
            if (user == null)
            {
                TBL_USER newUser = new TBL_USER();

                var employee = _context.TBL_EMPLOYEE.FirstOrDefault(e => e.Email == username);

                if (employee != null)
                {
                    newUser.Email = employee.Email;
                    newUser.EmployeeId = employee.Id;
                    DateTime currentDateTime = DateTime.Now;
                    newUser.CreatedDate = currentDateTime;
                    newUser.UpdatedDate = currentDateTime;
                    newUser.CreatedBy = 1;
                    newUser.UpdatedBy = 1;

                    _context.TBL_USER.Add(newUser);
                    _context.SaveChanges();

                    TBL_USER_ROLE_MAPPING roleMap = new TBL_USER_ROLE_MAPPING();
                    roleMap.UserId = (from e in _context.TBL_EMPLOYEE
                                      where e.Email == username
                                      select e.Id).FirstOrDefault();
                    roleMap.UserRoleId = (from r in _context.TBL_USER_ROLE
                                          where r.UserRoleName == resourceManager.GetString("Employee")
                                          select r.Id).FirstOrDefault();
                    _context.TBL_USER_ROLE_MAPPING.Add(roleMap);
                    _context.SaveChanges();
                    var token = new { token = GenerateJwtToken(newUser, _config) };
                    return token;
                }
                else
                {
                    return new { error = "Employee not found" };
                }
            }
            else
            {
                var token = new { token = GenerateJwtToken(user, _config) };
                return token;
            }
        }

        public string GenerateJwtToken(TBL_USER User, IConfiguration _config)
        {
            var EmployeeName=(from e in _context.TBL_EMPLOYEE
                             join u in _context.TBL_USER on e.Id equals u.EmployeeId
                             where u.Id==User.EmployeeId
                             select (e.FirstName+" "+e.LastName).ToString()).FirstOrDefault();
            var roles = from m in _context.TBL_USER_ROLE_MAPPING
                        join e in _context.TBL_USER on m.UserId equals e.Id
                        join u in _context.TBL_USER_ROLE on m.UserRoleId equals u.Id
                        where e.Email == User.Email
                        select u.UserRoleName;
            var authClaims = new List<Claim>
            {
               new Claim(ClaimTypes.Name, User.Email),
               new Claim(ClaimTypes.NameIdentifier, User.EmployeeId.ToString()),
               new Claim("EmployeeName", EmployeeName),
            };
            if (roles != null)
            {
                foreach (var userRole in roles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var Sectoken = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              authClaims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

            return token;
        }
    }
}