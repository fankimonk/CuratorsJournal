using API.Contracts.User;
using Contracts.User;
using Domain.Entities;

namespace API.Mappers
{
    public static class UserMapper
    {
        public static UserResponse ToResponse(this User user)
        {
            if (user.Role == null) throw new ArgumentNullException(nameof(user.Role));
            return new UserResponse(user.Id, user.UserName, new RoleResponse(user.Role.Id, user.Role.Name), user.WorkerId, null, null);
        }
    }
}
