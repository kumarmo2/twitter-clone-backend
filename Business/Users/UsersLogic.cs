using System.Threading.Tasks;
using TwitterWeb.Dtos;
using TwitterWeb.Dtos.Users;
using userModels = TwitterWeb.Models.Users;
using System;
using TwitterWeb.DataAccess.Users;
using TwitterWeb.Utils;

namespace TwitterWeb.Business.Users
{
  public class UsersLogic : IUsersLogic
  {

    private readonly IUserRepository _userRepository;
    private readonly IIdentityFactory _idFactory;
    public UsersLogic(IUserRepository userRepository, IIdentityFactory idFactory)
    {
      _userRepository = userRepository;
      _idFactory = idFactory;
    }

    public async Task<Result<userModels.User>> CreateUser(CreateUserRequest createUserRequest)
    {
      if (createUserRequest == null)
      {
        throw new ArgumentNullException("createUserRequest");
      }
      var result = new Result<userModels.User>();
      var userByEmailTask = _userRepository.GetByEmail(createUserRequest.Email);
      var userByHandleTask = _userRepository.GetByHandle(createUserRequest.Handle);

      var usersAllTaks = Task.WhenAll(userByEmailTask, userByHandleTask);
      await usersAllTaks;

      if (usersAllTaks.Status != TaskStatus.RanToCompletion)
      {
        throw usersAllTaks.Exception;
      }

      if (userByEmailTask.Result != null && userByEmailTask.Result.Id > 0)
      {
        result.ErrorMessages.Add("Email Already in use");
        return result;
      }

      if (userByHandleTask.Result != null && userByHandleTask.Result.Id > 0)
      {
        result.ErrorMessages.Add("Handle Already in use");
        return result;
      }

      var user = NewUser(createUserRequest, _idFactory.NextId());
      await _userRepository.Create(user);

      result.SuccessResult = user;
      return result;
    }
    private static userModels.User NewUser(CreateUserRequest createUserRequest, long id)
    {
      var hashedPassword = BCrypt.Net.BCrypt.HashPassword(createUserRequest.Password, 11);
      var now = DateTime.Now;

      return new userModels.User
      {
        Id = id,
        CreatedAt = now,
        DisplayName = createUserRequest.DisplayName,
        Email = createUserRequest.Email,
        FirstName = createUserRequest.FirstName,
        LastName = createUserRequest.LastName,
        Handle = createUserRequest.Handle,
        HashedPassword = hashedPassword,
        ModifiedOn = now
      };
    }

    public async Task<Result<userModels.User>> LoginUser(LoginRequest loginRequest)
    {
      if (loginRequest == null)
      {
        throw new ArgumentNullException("loginRequest");
      }
      var result = new Result<userModels.User>();
      var user = await _userRepository.GetByEmail(loginRequest.Email);

      if (user == null || user.Id < 1)
      {
        result.ErrorMessages.Add("Either Email or Password wrong");
        return result;
      }

      var isPasswordMatched = BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.HashedPassword);
      if (!isPasswordMatched)
      {
        result.ErrorMessages.Add("Either Email or Password wrong");
        return result;
      }

      result.SuccessResult = user;
      return result;
    }
  }
}