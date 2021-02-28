using System.Threading.Tasks;
using Dtos;
using Dtos.Users;
using userModels = Models.Users;

namespace Business.Users
{
    public interface IUsersLogic
    {
        Task<Result<userModels.User>> CreateUser(CreateUserRequest createUserRequest);
        Task<Result<userModels.User>> LoginUser(LoginRequest loginRequest);
        Task<Result<userModels.User>> GetUser(long userId);

    }
}