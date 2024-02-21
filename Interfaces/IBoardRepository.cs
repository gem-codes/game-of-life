using GameOfLifeTestProject.Models;

namespace GameOfLifeTestProject.Interfaces
{
    public interface IBoardRepository
    {
        int SaveNewBoardState(int rows, int columns);
        int[,] GetNextStateForBoard(int boardId);
        int[,] GetStateAfterSteps(int boardId, int steps);
        int[,] GetFinalStateForBoard(int boardId);
    }
}
