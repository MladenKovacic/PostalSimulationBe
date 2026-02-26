namespace PostalRoutingSimulation.MailItem;
using PostalRoutingSimulation.ContactInformation;
public class CreateMailItemReq
{
    public string SenderName { get; set; }
    public string SenderZipCode { get; set; }
    public string SenderStreet { get; set; }
    public string SenderCity { get; set; }
    public string RecipientName { get; set; }
    public string RecipientStreet { get; set; }
    public string RecipientZipCode { get; set; }
    public string RecipientCity { get; set; }
    public int WeightGrams { get; set; }
    public int MailType { get; set; }
    
}

