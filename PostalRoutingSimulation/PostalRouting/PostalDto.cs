using System.Security.Authentication;

namespace PostalRoutingSimulation.PostalRouting;

public record PostalDto(
    string Street,
    string City,
    string ZipCode);