
namespace PhoneBook.Domain.Entities
{
    public class Contact : BaseEntity
    {
        public string Name { get; private set; }
        public string Surname { get; private set; }
        public string Company { get; private set; }

        public Contact(string name, string surname, string company)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required", nameof(name));

            Name = name;
            Surname = surname;
            Company = company;
        }
    }
}
