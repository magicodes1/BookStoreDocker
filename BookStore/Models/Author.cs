using Nest;

namespace BookStoreDocker.Models;
[ElasticsearchType(RelationName = "author")]
public class Author
{

    public Guid AuthorId { get; set; } = Guid.NewGuid();
    [Text(Name ="first_name")]
    public string FirstName { get; set; } = string.Empty;
    [Text(Name ="last_name")]
    public string LastName { get; set; } = string.Empty;
    [Text(Name ="phone_number")]
    public string PhoneNumber { get; set; } = string.Empty;
    [Text(Name ="email")]
    public string Email { get; set; } = string.Empty;
    [Date(Format ="ddMMyyyy")]
    public string Birthday { get; set; } = string.Empty;
    [Nested]
    [PropertyName("books")]
    public List<Book> Books { get; set; }

    public Author()
    {
        Books = new List<Book>();
    }
    
}