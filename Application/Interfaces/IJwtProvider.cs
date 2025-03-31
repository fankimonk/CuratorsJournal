using Application.Entities;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IJwtProvider
    {
        AuthToken GenerateToken(User user);
    }
}