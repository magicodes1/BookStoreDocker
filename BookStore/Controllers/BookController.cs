using BookStoreDocker.Models;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace BookStoreDocker.Controller;
[Controller]
[Route("api/{controller}")]
public class BookController : ControllerBase
{

    private readonly IElasticClient _elastic;

    public BookController(IElasticClient elastic)
    {
        _elastic = elastic;
    }

    [HttpPost]
    public async Task<IActionResult> add([FromBody]Book book)
    {
        var indexResponse  = await _elastic.IndexDocumentAsync<Book>(book);

        if(!indexResponse.IsValid) return BadRequest();

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> get()
    {
        var indexResponse  = await _elastic.SearchAsync<Book>(x=>x.Query(
            q=>q.Nested(x=>x.Path(x=>x.Authors))
                 ).MatchAll()
            );

        if(!indexResponse.IsValid) return BadRequest();

        return Ok(indexResponse.Documents);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> findById(string id)
    {
       
        var indexResponse  = await _elastic.SearchAsync<Book>(x=>x.Query(
                    q=>q.Match(x=>x.Field(f=>f.Title).Analyzer("standard").Boost(1.1).Query(id)))
                    .Size(1)
        
            );

        if(!indexResponse.IsValid) return BadRequest();

        return Ok(indexResponse.Documents);
    }

    [HttpGet("sort")]
    public async Task<IActionResult> sortByPriceFromlow2hight()
    {
       
        var indexResponse  = await _elastic.SearchAsync<Book>(x=>x.Sort(
                    q=>q.Ascending(k=>k.Price)
                    )
        
            );

        if(!indexResponse.IsValid) return BadRequest();

        return Ok(indexResponse.Documents);
    }

     [HttpGet("range")]
    public async Task<IActionResult> filterPriceinRange([FromQuery]string from,[FromQuery]string to)
    {
       
        var indexResponse  = await _elastic.SearchAsync<Book>(x=>x.Query(
                    q=>q.TermRange(x=>x.Field(f=>f.Price)
                        .GreaterThanOrEquals(from)
                        .LessThanOrEquals(to)
                        )
                    )
        
            );

        if(!indexResponse.IsValid) return BadRequest();

        return Ok(indexResponse.Documents);
    }

    
     [HttpGet("filter")]
    public async Task<IActionResult> filterBook([FromQuery]string authorName)
    {
       System.Console.WriteLine(authorName);
        var indexResponse  = await _elastic.SearchAsync<Book>(x=>x.Query(
                    q=>q.Nested(n=>n.Boost(1.1)
                        .InnerHits(i => i.Explain())
                        .Path(p=>p.Authors)
                        .Query(q=>q.Terms(t=>t.Field(f=>f.Authors.First().FirstName)
                        .Terms(authorName)
                        )
                        
                        ))
                    
                    )
        
            );

        if(!indexResponse.IsValid) return BadRequest();

        return Ok(indexResponse.Documents);
    }
}