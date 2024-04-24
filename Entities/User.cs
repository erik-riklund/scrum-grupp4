using MongoDB.Entities;

namespace App.Entities
{
  public class User : Entity
  {
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public Address Address { get; set; } = null!;

    public Many<Role, User> Roles { get; set; } = null!;

    public Many<Order, User> Orders { get; set; } = null!;

    public User()
    {
      this.InitOneToMany(() => Roles);
      this.InitOneToMany(() => Orders);
    }
  }
}