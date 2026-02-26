namespace PostalRoutingSimulation.MailItem;

public record ZipcodePackageListResponse(List<FilteredMailItem> Outgoing, List<FilteredMailItem> Incoming);

public record FilteredMailItem(
     string SenderName,
      string SenderZipCode,
      string RecipientName,
      string RecipientZipCode
    );
    
    