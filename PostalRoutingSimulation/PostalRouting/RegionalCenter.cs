using System.Diagnostics.Eventing.Reader;
using PostalRoutingSimulation.ContactInformation;

namespace PostalRoutingSimulation.PostalRouting;

using PostalRoutingSimulation.MailItem;

public class RegionalCenter
{
    public PostalOffice Offices { get; set; }
    public List<Address> Addresses { get; set; }
    private Dictionary<string, PostalOffice> OfficesByZip { get; set; }
    public List<MailItem> Holding { get; set; }

    public RegionalCenter()
    {
        Holding = new List<MailItem>();
        OfficesByZip = new Dictionary<string, PostalOffice>();
        Addresses = new List<Address>();
    }


    public void RegisterOffice(PostalOffice office)
    {
        OfficesByZip.TryAdd(office.Address.ZipCode, office);
    }

    public IEnumerable<PostalDto> GetAllOffices()
    {
        return OfficesByZip.Values.Select
        (office => new PostalDto(
         office.Address.Street,
         office.Address.City,
         office.Address.ZipCode));
    }

    public PostalOffice? GetOffice(string zip)
    {
        if (!OfficesByZip.TryGetValue(zip, out var office))
        {
            throw new Exception("Office not found");
        }
        return office;
    }

    public bool DoesZipCodeExist(string zipCode)
    {
        if (OfficesByZip.ContainsKey(zipCode))
        {
            return true;
        }

        return false;
    }

    public bool DoesAddressExist(Address address)
    {
        if (OfficesByZip.TryGetValue(address.ZipCode, out var postalOffice))
        {
            return postalOffice.DoesAddressCodeExist(address);
        }

        return false;
    }


    public MailItem CreateMailItem(MailItem item)
    {
        item.UpdateStatus(MailStatus.AtRegional, "We have recived the mailitem");
        Holding.Add(item);

        return item;
    }


    public void ReviceiveFromOffice(MailItem item)
    {
        item.UpdateStatus(MailStatus.AtRegional, "we are doing everything we can");
        Holding.Add(item);
    }


    public void RouteMailCycle()
    {
        var itemsProccesing = Holding.ToList();

        foreach (var item in itemsProccesing)
        {
            string lastZip = item.Recipient.Address.ZipCode;
            
            if (OfficesByZip.TryGetValue(lastZip, out var toLastOffice))
            {
                item.UpdateStatus(
                    MailStatus.InTransitToOffice,
                    $"Routing to office serving zip {lastZip}");

                toLastOffice.AcceptFromRegional(item);
                Holding.Remove(item);
            }
            else
            {
                item.UpdateStatus(
                    MailStatus.ReturnedToSender, $"No office found for zip {lastZip}"
                );
                Holding.Remove(item);
            }
        }
    }
}