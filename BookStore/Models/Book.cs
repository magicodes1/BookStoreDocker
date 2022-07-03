using Nest;

namespace BookStoreDocker.Models;
[ElasticsearchType(RelationName = "book")]
public class Book
{
    public Guid BookId { get; set; } = Guid.NewGuid();
    
    [Text(Name = "title")]
    public string Title { get; set; } = string.Empty;

    [Number(Name = "price", DocValues = false)]
    public int Price { get; set; }


    [Nested]
    [PropertyName("authors")]
    public List<Author> Authors { get; set; }


    public Book()
    {
        Authors = new List<Author>();
    }
}