using TwitterWeb.Models.Users;
using Dapper;
using System.Threading.Tasks;

namespace TwitterWeb.DataAccess.Users
{
  public class UserRepository : IUserRepository
  {
    private readonly IDbConnectionFactory _dbConnectionFactory;
    public UserRepository(IDbConnectionFactory dbConnectionFactory)
    {
      _dbConnectionFactory = dbConnectionFactory;
    }
    public async Task Create(User user)
    {
      var query = $@"insert into users.users(id, firstname, lastname, displayname, handle, email, hashedpassword, createdat, modifiedat)
                    values
                    (@id, @firstname, @lastname, @displayname, @handle, @email, @hashedpassword, @createdat, @modifiedat)";
      using (var con = _dbConnectionFactory.GetDbConnection())
      {
        await con.ExecuteAsync(query, new
        {
          id = user.Id,
          firstname = user.FirstName,
          lastname = user.LastName,
          displayname = user.DisplayName,
          handle = user.Handle,
          email = user.Email,
          hashedpassword = user.HashedPassword,
          createdat = user.CreatedAt,
          modifiedat = user.ModifiedOn
        });
      }

    }

    public Task Get(long id)
    {
      throw new System.NotImplementedException();
    }

    public async Task<User> GetByEmail(string email)
    {
      var query = $"select * from users.users where email = @email";
      using (var con = _dbConnectionFactory.GetDbConnection())
      {
        return await con.QueryFirstOrDefaultAsync<User>(query, new { email = email });
      }
    }

    public async Task<User> GetByHandle(string handle)
    {
      var query = $"select * from users.users where handle = @handle";
      using (var con = _dbConnectionFactory.GetDbConnection())
      {
        return await con.QueryFirstOrDefaultAsync<User>(query, new { handle = handle });
      }
    }

    public Task Update(User user)
    {
      throw new System.NotImplementedException();
    }
  }
}