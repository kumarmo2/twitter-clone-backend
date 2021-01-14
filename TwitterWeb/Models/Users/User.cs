using System;
using TwitterWeb.Dtos.Users;

namespace TwitterWeb.Models.Users
{
  public class User
  {
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string DisplayName { get; set; }
    public string Handle { get; set; }
    public string Email { get; set; }
    public string HashedPassword { get; set; }
    public string IsDeactivated { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedOn { get; set; }

  }
}