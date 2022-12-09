﻿using GameStore.Models;
using GameStore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GameStore.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class GameCommentController : Controller
    {
        private readonly GameCommentService _gameCommentsService;
        public GameCommentController(GameCommentService gameCommentService)
        {
            _gameCommentsService = gameCommentService;
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("AllGameComments")]
        public async Task<IActionResult> GetAllGameAllComments()
        {
            return new OkObjectResult(await _gameCommentsService.GetAllGameAllComments());
        }
        [HttpGet]
        [Route("AllGameCommentsForGame/{id}")]
        public async Task<IActionResult> GetAllCommentsForGame(int gameId)
        {
            return new OkObjectResult(await _gameCommentsService.GetAllCommentsForGame(gameId));
        }
        [HttpPost]
        [Route("AddCommentToGame")]
        public async Task<IActionResult> AddCommentToGame([FromBody] CommentModel comment, Guid? parentCommentId)
        {
            return new OkObjectResult(await _gameCommentsService.AddCommentToGame(comment, parentCommentId));
        }
        [HttpPut]
        [Route("EditComment/{Guid}")]
        public async Task<IActionResult> EditComment([FromBody] CommentModel editedComment, Guid id)
        {
            return new OkObjectResult(await _gameCommentsService.EditComment(editedComment, id));
        }
        [HttpPut]
        [Route("RestoreComment/{Guid}")]
        public async Task<IActionResult> RestoreComment(Guid id)
        {
            return new OkObjectResult(await _gameCommentsService.RestoreComment(id));
        }

        [HttpDelete]
        [Route("DeleteComment/{Guid}")]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            return new OkObjectResult(await _gameCommentsService.DeleteComment(id));
        }
    }
}