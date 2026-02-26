namespace PostalRoutingSimulation.MailItem;

public class History
{
    public MailStatus Status { get; set; }
    public string Note { get; set; }
    public DateTime Date { get; set; }

    public override string ToString()
    {
        return $"{Date} {Note} {Status}";
    }
}