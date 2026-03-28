namespace PhoneBook.Contracts.Events;

public class ContactDeletedEvent
{
    public Guid ContactId { get; set; }
    public DateTime OccurredOn { get; set; } = DateTime.UtcNow;
}