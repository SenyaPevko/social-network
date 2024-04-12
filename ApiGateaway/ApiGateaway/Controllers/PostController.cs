using Application.Posts.Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Sagas.Posts.GetPost.Requests;
using Sagas.Posts.GetPost.Responses;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("/api/gateway")]
    public class PostController(IBus bus) : ControllerBase
    {
        [HttpGet("{postId:guid}", Name = nameof(GetPostAsync))]
        [Produces("application/json", "application/xml")]
        [ProducesResponseType(typeof(PostViewModel), 200)]
        public async Task<IActionResult> GetPostAsync([FromRoute] Guid postId)
        {
            var response = await bus.Request<GetPostListRequest, GetPostListResponse>
                (new GetPostListRequest
                {
                    PostsId = new[] { postId },
                    CorrelationId = Guid.NewGuid()
                });
            
            return Ok(response.Message);
        }
    }
}