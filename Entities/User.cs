using MongoDB.Entities;


namespace App.Entities
{
  public class User : Entity
  {
    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public Many<Role, User> Roles { get; set; } = null!;

    public User()
    {
      this.InitOneToMany(() => Roles);
    }
  }
}