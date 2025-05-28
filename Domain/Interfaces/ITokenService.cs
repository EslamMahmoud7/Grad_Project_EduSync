using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ITokenService
    {
        public Task<string> GenerateJWTToken(User user);
    }
}
