using CourierCounter.Models.Entities;

namespace CourierCounter.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(ApplicationUser user, int workerId);
    }
}
