using System.Globalization;
using System.Text.Json.Serialization;

namespace PostalRoutingSimulation.MailItem;

using PostalRoutingSimulation.ContactInformation;

public class MailItem
{
    public Person Sender { get; init; }
    public Person Recipient { get; init; }
    public int WeightGrams { get; init; }
    private History Information { get; set; }
     private List<History> _history { get; init; } = new();
     private MailStatus MailStatus { get; set; } 
     public MailType MailType { get; init; }

     public MailItem()
     {
         UpdateStatus(MailStatus.CreatedAtOffice, "Created");
     }
     public MailItem(MailStatus mailStatus)
     {
         MailStatus = mailStatus;
     }

     public MailItem(Person sender, Person recipient, int weightGrams, MailType mailType)
     {
         UpdateStatus(MailStatus.CreatedAtOffice, "Created");
         Sender = sender;
         Recipient = recipient;
         WeightGrams = weightGrams;
         MailType = mailType;
     }
     
    public MailItem(MailStatus mailStatus,Person sender, Person recipient, int weightGrams, MailType mailType)
    {
        Sender = sender;
        Recipient = recipient;
        WeightGrams = weightGrams;
        MailType = mailType;

     
    }

    public void UpdateStatus(MailStatus newStatus, string note)
    {
        MailStatus = newStatus;
        
        _history.Add(new History
        {
            Status = newStatus,
            Note = note,
            Date = DateTime.Now
        });
    }
    
    public void ShowHistory()
    {
        foreach (var info in _history)
        {
            Console.WriteLine(info);
        }
    }

    public static MailItem CreateFrom(CreateMailItemReq req)
    {
        var sender = new Person(
            req.SenderName,
            new Address(req.SenderStreet, req.SenderCity, req.SenderZipCode));
    
        var recipient = new Person(

            req.RecipientName,
            new Address(req.RecipientStreet, req.RecipientCity, req.RecipientZipCode));


        var mailItem = new MailItem(
            sender,
            recipient,
            req.WeightGrams,
            (MailType)req.MailType);
        
        return mailItem;
    }
}