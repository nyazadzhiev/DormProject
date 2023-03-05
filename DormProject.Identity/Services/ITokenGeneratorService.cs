using DormProject.Identity.Data;

namespace DormProject.Identity.Services
{
    public interface ITokenGeneratorService
    {
        string GenerateToken(AppUser user, IEnumerable<string> roles = null);
    }
}
