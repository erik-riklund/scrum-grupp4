using MongoDB.Entities;

namespace App.Entities
{
  public class Movie : Entity
  {
    public string Title { get; set; } = null!;

    [OwnerSide]
    public Many<Actor, Movie> Actors { get; set; } = null!;

    public Movie() => this.InitManyToMany(() => Actors, actor => actor.Movies);
  }

  public class Actor : Entity
  {
    public string Name { get; set; } = null!;

    [InverseSide]
    public Many<Movie, Actor> Movies { get; set; } = null!;

    public Actor() => this.InitManyToMany(() => Movies, movie => movie.Actors);
  }
}