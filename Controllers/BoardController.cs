using GameOfLifeTestProject.Interfaces;
using GameOfLifeTestProject.Models;
using GameOfLifeTestProject.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameOfLifeTestProject.Controllers
{
    // Controllers/BoardController.cs
    [Route("api/[controller]")]
    [ApiController]
    public class BoardController : ControllerBase
    {
        private readonly IBoardService _boardService;

        public BoardController(IBoardService boardService)
        {
            _boardService = boardService;
        }

        [HttpPost]
        public IActionResult CreateBoard([FromBody] Board board)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var boardId = _boardService.GenerateBoard(board.Rows, board.Columns);
            return Ok(boardId);
        }

        [HttpPost("next")]
        public IActionResult GetNextState(int boardId)
        {
            var nextState = _boardService.GetNextState(boardId);
            if (nextState == null)
                return NotFound();

            return Ok(nextState);
        }

        [HttpGet("futurestate/{boardId}/{steps}")]
        public IActionResult GetFutureStateForBoard(int boardId, int steps)
        {
            var futureState = _boardService.GetStateAfterSteps(boardId, steps);
            if (futureState == null)
                return NotFound();

            return Ok(futureState);
        }

        [HttpGet("finalstate/{boardId}")]
        public IActionResult GetFinalStateForBoard(int boardId)
        {
            var finalState = _boardService.GetFinalStateForBoard(boardId);
            if (finalState == null)
                return NotFound();

            return Ok(finalState);
        }
    }

}

