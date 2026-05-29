using EmployeeApi.Models;

namespace EmployeeApi.Services
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
