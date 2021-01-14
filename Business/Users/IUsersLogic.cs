using System.Threading.Tasks;
using TwitterWeb.Dtos;
using TwitterWeb.Dtos.Users;
using userModels = Models.Users;

namespace TwitterWeb.Business.Users
{
    public interface IUsersLogic
    {
        Task<Result<userModels.User>> CreateUser(CreateUserRequest createUserRequest);
        Task<Result<userModels.User>> LoginUser(LoginRequest loginRequest);
    }
}