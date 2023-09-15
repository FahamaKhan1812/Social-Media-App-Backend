using Microsoft.AspNetCore.Mvc;
using SocialMedia.Domain.Models;

namespace SocialMedia.Api.Controllers.V1;

[ApiVersion("1.0")]
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
            Text = "Hello"
        };
        return Ok(post);
    }
}
