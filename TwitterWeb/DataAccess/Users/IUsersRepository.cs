using TwitterWeb.Models.Users;
using System.Threading.Tasks;

namespace TwitterWeb.DataAccess.Users
{
  public interface IUserRepository
  {
    Task Create(User user);
    Task Get(long id);
    Task<User> GetByHandle(string handle);
    Task<User> GetByEmail(string email);
    Task Update(User user);
  }
}