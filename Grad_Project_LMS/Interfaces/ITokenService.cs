using Grad_Project_LMS.Models;

namespace Grad_Project_LMS.Interfaces
{
    public interface ITokenService
    {
        public Task<string> GenerateJWTToken(Student student);
    }
}
