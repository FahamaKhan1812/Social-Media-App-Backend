using Microsoft.AspNetCore.Mvc;

namespace SocialMedia.Api.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.baseRoute)]
[ApiController]
public class PostController : ControllerBase
{
    [HttpGet]
    [Route(ApiRoutes.Posts.GetById)]
    public IActionResult GetById(int id)
    {
        //Post post = new()
        //{
        //    Id = id,
        //    Text = "Hello"
        //};
        return Ok(true);
    }
}
