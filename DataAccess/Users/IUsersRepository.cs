using Models.Users;
using System.Threading.Tasks;

namespace DataAccess.Users
{
    public interface IUserRepository
    {
        Task Create(User user);
        Task<User> Get(long id);
        Task<User> GetByHandle(string handle);
        Task<User> GetByEmail(string email);
        Task Update(User user);
    }
}