using System.Globalization;

namespace PostalRoutingSimulation.PostalRouting;

using PostalRoutingSimulation.ContactInformation;
using PostalRoutingSimulation.MailItem;

public class PostalOffice
{
    public string ZipCode { get; init; }
    private Dictionary<string, List<Person>> ResidentsByStreet { get; set; }
    public List<MailItem> Incoming { get; init; }
    public List<MailItem> Outgoing { get; init; } 
    private RegionalCenter RegionalCenter { get; init; }

    public PostalOffice(string zipCode, RegionalCenter regionalCenter)
    {
        ZipCode = zipCode;
        ResidentsByStreet = new Dictionary<string, List<Person>>();
        Incoming = new List<MailItem>();
        Outgoing = new List<MailItem>();
        RegionalCenter = regionalCenter;
    }

    
    public void RegisterResident(Person person)
    {
        var contactInfo = person.Address.ZipCode;
        string contactsStreet = person.Address.Street;

        if (contactInfo != ZipCode)
        {
            throw new Exception("Person does not live in that ZipCode.");
        }
        
        ResidentsByStreet.TryAdd(contactsStreet, new List<Person>());
        ResidentsByStreet[contactsStreet].Add(person);

        Console.WriteLine($"{person.Name},{contactsStreet}, {contactInfo}");
    }
    
    public bool DoesAddressCodeExist(Address address)
    {
        if (ResidentsByStreet.TryGetValue(address.Street, out var streetResident))
        { 
            foreach (var tenant in streetResident)
            {
                
                if(tenant.Address.Equals(address))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool KnowsRecipient(Person person)
    {
       
        if (ResidentsByStreet.TryGetValue(person.Address.Street, out var streetResident))
        { 
            foreach (var tenant in streetResident)
            {
                
                if(tenant.Equals(person))
                {
                    return true;
                }
            }
        }
        return false;
    }
    public MailItem CreateMailItemAtOffice(MailItem item)
    {
        item.UpdateStatus(MailStatus.CreatedAtOffice,"We have recived the mailitem");
        Outgoing.Add(item);
         
        return item;
    }
    

    public MailItem CreateOutgoingMail(Person sender,Person recipient, int weightGrams, MailType type)
    {
        var item = new MailItem(
            MailStatus.CreatedAtOffice,
            sender,
            recipient,
            weightGrams,
            type
        );

        Outgoing.Add(item);
        return item;
    }

    public void AcceptFromRegional(MailItem item)
    {
        item.UpdateStatus(MailStatus.AtOffice, "its at the office going out soon");
        Incoming.Add(item);
    }



    public void ProcessMailCycle()
    {
        var incoming = new List<MailItem>(Incoming);

        foreach (var incomingItem in incoming)
        {
            if (KnowsRecipient(incomingItem.Recipient))
            {
                incomingItem.UpdateStatus(
                    MailStatus.Delivered,
                    "Thank you for using us"
                    
                );
               incomingItem.ShowHistory();
            }
            else
            {
                incomingItem.UpdateStatus(
                    MailStatus.ReturnedToSender,
                    "Didn't find your recipient"
                );

                 RegionalCenter.ReviceiveFromOffice(incomingItem);
                Console.WriteLine("Was send back to office");
            }

            // Incoming.Remove(incomingItem);
        }


        var outgoing = Outgoing.ToList();

        foreach (var outgoingItem in outgoing)
        {
            outgoingItem.UpdateStatus(
                MailStatus.InTransitToRegional,
                "It's on its way"
            );
            Console.WriteLine("its on its wayt");
            RegionalCenter.ReviceiveFromOffice(outgoingItem);
            Outgoing.Remove(outgoingItem);
        }
    }


    public override string ToString()
    {
        return $"{ResidentsByStreet}";
    }
}


// foreach (var tenant in ResidentsByStreet)
// {
//     foreach (var resident in tenant.Value)
//     {
//         if (resident.Equals(person))
//             return true;
//     }
// } ResidentsByStreet.TryGetValue((person.Address.Street, out var streetResident) && streetResident.Contains(person));