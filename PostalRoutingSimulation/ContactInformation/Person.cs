namespace PostalRoutingSimulation.ContactInformation;

public class Person : IEquatable<Person>
{
    public string Name { get; init; }
    public Address Address { get; init; }

    public Person(string name, Address address)
    {
        Name = name;
        Address = address;
    }
    

    public bool Equals(Person? other)
    {
        if (other == null)
            return false;

        return Address.Equals(other.Address) && Name == other.Name;
        
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Address?.GetHashCode() ?? 0);
    }
  
}
