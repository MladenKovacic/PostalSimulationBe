namespace PostalRoutingSimulation.ContactInformation;

public class Address
{
    public string Street { get; init; }
    public string City { get; init; }
    public string ZipCode { get; init; }

    public Address(string street, string city, string zipcode)
    {
        Street = street;
        City = city;
        ZipCode = zipcode;
    }

    
    public bool Equals(Address? other)
    {
        if (other == null)
            return false;

        return Street == other.Street && 
               City == other.City &&
               ZipCode == other.ZipCode;
    }
    
    
    public override string ToString()
    {
        return $"{Street} {City} {ZipCode}";
    }
}