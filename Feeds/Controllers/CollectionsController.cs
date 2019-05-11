using System;
using System.Collections.Generic;
using Feeds.Controllers.Models;
using Feeds.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Feeds.Controllers
{
    [Authorize]
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class CollectionsController : ControllerBase
    {
        private readonly ICollectionRepository _userCollectionRepository;

        public CollectionsController(ICollectionRepository UserCollectionRepository) => _userCollectionRepository = UserCollectionRepository;

        [HttpGet]
        public ActionResult<List<UserCollection>> GetById(string id)
        {
            if (id != null)
            {
                return Ok(_userCollectionRepository.GetUserCollections(Guid.Parse(id)));
            }
            return BadRequest();
        }

        [HttpGet]
        public ActionResult<List<UserCollection>> GetCollections()
        {
                return Ok(_userCollectionRepository.GetCollections());
        }

        [HttpPost]
        public ActionResult<Guid> Create([FromBody] CreateCollectionRequest createRequest)
        {
            if (createRequest != null)
            {
                UserCollection collection = new UserCollection
                {
                    Id = Guid.NewGuid(),
                    Name = createRequest.CollectionName,
                    UserId = Guid.Parse(createRequest.UserId)
                };
                if (_userCollectionRepository.Create(collection))
                {
                    return Ok(collection.Id);
                } 
            }
            return BadRequest();
        }

        [HttpPut]
        public ActionResult Rename([FromBody] RenameCollectionRequest renameRequest)
        {
             if (renameRequest != null && 
                _userCollectionRepository.Rename(
                     new UserCollection
                     {
                         Id = Guid.Parse(renameRequest.CollectionId),
                         Name = renameRequest.NewName
                     }))
             {
                    return Ok();
             }
             return BadRequest();
        }

        [HttpDelete]
        public ActionResult Delete([FromBody] string CollectionId)
        {
            if (CollectionId != null && _userCollectionRepository.Delete(Guid.Parse(CollectionId)))
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost]
        public ActionResult AddFeed([FromBody]AddFeedToCollectionRequest collectionFeed)
        {
            if (collectionFeed != null && 
                _userCollectionRepository.AddFeedToCollection(
                    new CollectionFeeds
                    {
                        CollectionId = Guid.Parse(collectionFeed.CollectionId),
                        FeedId = Guid.Parse(collectionFeed.FeedId)
                    }))
            {
                
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete]
        public ActionResult DeleteFeed([FromBody]AddFeedToCollectionRequest collectionFeed)
        {
            if (collectionFeed!=null && 
                _userCollectionRepository.DeleteFeedFromCollection(new CollectionFeeds
                {
                    CollectionId = Guid.Parse(collectionFeed.CollectionId),
                    FeedId = Guid.Parse(collectionFeed.FeedId)
                }))
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}