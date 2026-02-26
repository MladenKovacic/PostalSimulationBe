namespace PostalRoutingSimulation.MailItem;
using PostalRoutingSimulation.ContactInformation;

public record ZipcodePackageListResponse(List<FilteredMailItem> Outgoing, List<FilteredMailItem> Incoming);

public record FilteredMailItem(
    string SenderName,
    string SenderZipCode,
    string RecipientName,
    string RecipientZipCode
)
{
    public static FilteredMailItem toDTO(MailItem req)
    {
        var test = new FilteredMailItem(
            req.Sender.Name,
            req.Sender.Address.ZipCode,
            req.Recipient.Name,
            req.Recipient.Address.ZipCode);
        return test;
    }
};
    
    