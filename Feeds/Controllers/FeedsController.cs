using System;
using System.Collections.Generic;
using Feeds.ControllerModels;
using Feeds.Controllers.Models;
using Feeds.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Feeds.Controllers
{
    [Authorize]
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class FeedsController : ControllerBase
    {
        private readonly IFeedRepository _feedRepository;

        public FeedsController(IFeedRepository feedRepository)
        {
            _feedRepository = feedRepository;
        }
        
        [HttpGet]
        public ActionResult<List<Feed>> Get()
        {
            return Ok(_feedRepository.GetAllFeeds());
        }

        [HttpPost]
        public ActionResult<Guid> Create([FromBody] CreateFeedRequest createRequest)
        {
            Feed feed = new Feed
            {
                Id = Guid.NewGuid(),
                Name = createRequest.Name,
                Tags = createRequest.Tags,
            };
            if (createRequest !=null && _feedRepository.Create(feed))
            {
                return Ok(feed.Id);
            } 
            return BadRequest();
        }

        [HttpPost]
        public ActionResult AddPosts([FromBody] AddPostsToFeedRequest postsRequest)
        {
            if (postsRequest != null && _feedRepository.AddPosts(new Post
            {
                Text = postsRequest.Text,
                Image = postsRequest.Image,
                SourceUrl = postsRequest.SourceUrl,
                Date = postsRequest.Date,
                FeedId = Guid.Parse(postsRequest.FeedId)
            }))
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete]
        public ActionResult Delete([FromBody]string id)
        {
            if (id!= null && _feedRepository.Delete(Guid.Parse(id)))
            {
                return Ok();
            }  
            return BadRequest();
        }
    }
}
