using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Domain.Models;

namespace SocialMedia.Api.Controllers.V2;

[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class PostController : ControllerBase
{
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        Post post = new()
        {
            Id = id,
            Text = "Hello Universe"
        };
        return Ok(post);
    }
}
