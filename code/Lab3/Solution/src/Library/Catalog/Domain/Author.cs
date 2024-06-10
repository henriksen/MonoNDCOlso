using Library.Catalog.Contracts;

namespace Library.Catalog.Domain;

public class Author
{
    public AuthorId Id { get; }
    public string Name { get; private set; }
    public string Biography { get; private set; }
    public DateTimeOffset? BirthDate { get; private set; }
    public DateTimeOffset? DeathDate { get; private set; }

    // Private parameterless constructor for EF Core
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Author() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Author(AuthorId id, string name, string biography, DateTimeOffset? birthDate, DateTimeOffset? deathDate)
    {
        Id = id;
        Name = name;
        Biography = biography;
        BirthDate = birthDate;
        DeathDate = deathDate;
    }

    public static Author Create(AuthorId id, string name, string biography, DateTimeOffset? birthDate, DateTimeOffset? deathDate)
    {
        return new Author(id, name, biography, birthDate, deathDate);
    }

    public void UpdateName(string name)
    {
        Name = name;
    }

    public void UpdateBiography(string biography)
    {
        Biography = biography;
    }

    public void UpdateBirthDate(DateTimeOffset? birthDate)
    {
        BirthDate = birthDate;
    }

    public void UpdateDeathDate(DateTimeOffset? deathDate)
    {
        DeathDate = deathDate;
    }
}
