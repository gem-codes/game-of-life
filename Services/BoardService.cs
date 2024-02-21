using GameOfLifeTestProject.Interfaces;

namespace GameOfLifeTestProject.Services
{
    public class BoardService : IBoardService
    {
        private readonly IBoardRepository _boardRepository;

        public BoardService(IBoardRepository gameRepository)
        {
            _boardRepository = gameRepository;
        }

        public int GenerateBoard(int rows, int columns)
        {
            return _boardRepository.SaveNewBoardState(rows, columns);
        }

        public int[,] GetNextState(int boardId)
        {
            return _boardRepository.GetNextStateForBoard(boardId);
        }

        public int[,] GetStateAfterSteps(int boardId, int steps)
        {
            return _boardRepository.GetStateAfterSteps(boardId, steps);
        }

        public int[,] GetFinalStateForBoard(int boardId)
        {
            return _boardRepository.GetFinalStateForBoard(boardId);
        }
    }
}
