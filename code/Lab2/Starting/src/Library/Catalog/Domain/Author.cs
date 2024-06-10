using Library.Catalog.Contracts;

namespace Library.Catalog.Domain;

public class Author(AuthorId id, string name, string biography, DateTimeOffset? birthDate, DateTimeOffset? deathDate)
{
    public AuthorId Id { get; } = id;
    public string Name { get; private set; } = name;
    public string Biography { get; private set; } = biography;
    public DateTimeOffset? BirthDate { get; private set; } = birthDate;
    public DateTimeOffset? DeathDate { get; private set; } = deathDate;

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
