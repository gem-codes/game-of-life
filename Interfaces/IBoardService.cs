using GameOfLifeTestProject.Models;

namespace GameOfLifeTestProject.Interfaces
{
    // Services/IBoardService.cs
    public interface IBoardService
    {
        int GenerateBoard(int rows, int cols);
        int[,] GetNextState(int board);
        int[,] GetStateAfterSteps(int boardId, int steps);
        int[,] GetFinalStateForBoard(int boardId);
    }


}
