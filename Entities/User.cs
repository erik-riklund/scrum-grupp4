using MongoDB.Entities;


namespace App.Entities
{
  public class User : Entity
  {
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Password { get; set; } = null!;

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public Address Address { get; set; }

    public Many<Order, User> Orders { get; set; } = null!;

    public Many<Role, User> Roles { get; set; } = null!;

    public User()
    {
      this.InitOneToMany(() => Roles);
      this.InitOneToMany(() => Orders);

    }
  }
}