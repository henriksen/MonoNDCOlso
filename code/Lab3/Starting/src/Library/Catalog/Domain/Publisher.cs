using Library.Catalog.Contracts;

namespace Library.Catalog.Domain;

public class Publisher(PublisherId id, string name, string address, string contactInformation)
{
    public PublisherId Id { get; } = id;
    public string Name { get; private set; } = name;
    public string Address { get; private set; } = address;
    public string ContactInformation { get; private set; } = contactInformation;

    public void UpdateName(string name)
    {
        Name = name;
    }

    public void UpdateAddress(string address)
    {
        Address = address;
    }

    public void UpdateContactInformation(string contactInformation)
    {
        ContactInformation = contactInformation;
    }
}
