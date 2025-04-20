using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ITokenService
    {
        public Task<string> GenerateJWTToken(Student student);
    }
}
