namespace PostalRoutingSimulation.MailItem;

public enum MailStatus
{
    Sent,
    CreatedAtOffice,
    InTransitToRegional,
    AtRegional,
    InTransitToOffice,
    AtOffice,
    Delivered,
    Misrouted,
    ReturnedToSender
}