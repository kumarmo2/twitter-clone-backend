namespace TwitterWeb.Dtos.Users
{
  public class User
  {
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string DisplayName { get; set; }
    public string Handle { get; set; }
    public string Email { get; set; }
  }
}