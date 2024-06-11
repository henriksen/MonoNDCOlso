using Library.Catalog.Contracts;

namespace Library.Catalog.Domain;

public class Genre(GenreId id, string name, string description)
{
    public GenreId Id { get; } = id;
    public string Name { get; private set; } = name;
    public string Description { get; private set; } = description;

    public void UpdateName(string name)
    {
        Name = name;
    }

    public void UpdateDescription(string description)
    {
        Description = description;
    }
}