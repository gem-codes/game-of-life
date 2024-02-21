using GameOfLifeTestProject.DBContext;
using GameOfLifeTestProject.Interfaces;
using GameOfLifeTestProject.Models;
using Microsoft.Data.SqlClient;

namespace GameOfLifeTestProject.Repositories
{
    public class BoardRepository : IBoardRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public BoardRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int SaveNewBoardState(int rows, int columns)
        {
            var newBoard = new Board
            {
                Rows = rows,
                Columns = columns
            };

            _dbContext.Boards.Add(newBoard);
            _dbContext.SaveChanges();

            return newBoard.Id;
        }

        public int[,] GetNextStateForBoard(int boardId)
        {
            var board = _dbContext.Boards.FirstOrDefault(b => b.Id == boardId);
            if (board == null)
                return null; // or throw exception

            // Convert the board state to a 2D array
            int[,] currentState = ConvertStringTo2DArray(board.CurrentState, board.Rows, board.Columns);
            int[,] nextState = CalculateNextState(currentState);

            return nextState;
        }

        public int[,] GetStateAfterSteps(int boardId, int steps)
        {
            var board = _dbContext.Boards.FirstOrDefault(b => b.Id == boardId);
            if (board == null)
                return null; // or throw exception

            // Convert the board state to a 2D array
            int[,] currentState = ConvertStringTo2DArray(board.CurrentState, board.Rows, board.Columns);
            for (int i = 0; i < steps; i++)
            {
                currentState = CalculateNextState(currentState);
            }

            return currentState;
        }

        public int[,] GetFinalStateForBoard(int boardId)
        {
            var board = _dbContext.Boards.FirstOrDefault(b => b.Id == boardId);
            if (board == null)
                return null; // or throw exception

            // Convert the board state to a 2D array
            int[,] currentState = ConvertStringTo2DArray(board.CurrentState, board.Rows, board.Columns);
            while (!IsFinalState(currentState))
            {
                currentState = CalculateNextState(currentState);
            }

            return currentState;
        }

        // Method to convert a string representing the board state to a 2D array
        private int[,] ConvertStringTo2DArray(string state, int rows, int columns)
        {
            int[,] array = new int[rows, columns];
            string[] rowsArray = state.Split(';');
            for (int i = 0; i < rows; i++)
            {
                string[] colsArray = rowsArray[i].Split(',');
                for (int j = 0; j < columns; j++)
                {
                    array[i, j] = int.Parse(colsArray[j]);
                }
            }
            return array;
        }

        // Method to calculate the next state of the board
        private int[,] CalculateNextState(int[,] currentState)
        {
            int rows = currentState.GetLength(0);
            int cols = currentState.GetLength(1);
            int[,] nextState = new int[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    int liveNeighbors = CountLiveNeighbors(currentState, i, j);
                    if (currentState[i, j] == 1)
                    {
                        nextState[i, j] = (liveNeighbors == 2 || liveNeighbors == 3) ? 1 : 0;
                    }
                    else
                    {
                        nextState[i, j] = (liveNeighbors == 3) ? 1 : 0;
                    }
                }
            }

            return nextState;
        }

        // Method to count the number of live neighbors for a cell
        private int CountLiveNeighbors(int[,] board, int x, int y)
        {
            int rows = board.GetLength(0);
            int cols = board.GetLength(1);
            int count = 0;

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int row = (x + i + rows) % rows;
                    int col = (y + j + cols) % cols;
                    count += board[row, col];
                }
            }

            count -= board[x, y];
            return count;
        }

        // Method to check if the board has reached a final state
        private bool IsFinalState(int[,] board)
        {
            // Initialize a variable to track if any cell's state changes in the next iteration
            bool hasChanged = false;

            // Apply the rules of Conway's Game of Life to determine the next state of each cell
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    // Get the current state of the cell
                    int currentState = board[i, j];

                    // Get the count of live neighbors
                    int liveNeighbors = CountLiveNeighbors(board, i, j);

                    // Apply the rules of Conway's Game of Life to determine the next state
                    int nextState = currentState;
                    if (currentState == 1 && (liveNeighbors < 2 || liveNeighbors > 3))
                    {
                        // Any live cell with fewer than two live neighbors dies, or with more than three live neighbors dies
                        nextState = 0;
                    }
                    else if (currentState == 0 && liveNeighbors == 3)
                    {
                        // Any dead cell with exactly three live neighbors becomes a live cell
                        nextState = 1;
                    }

                    // Check if the state of the cell changes
                    if (currentState != nextState)
                    {
                        hasChanged = true;
                        break; // No need to check further if a change is detected
                    }
                }

                // If a change is detected, break from the outer loop as well
                if (hasChanged)
                {
                    break;
                }
            }

            // If no cell's state changes in the next iteration, it is considered a final state
            return !hasChanged;
        }

    }
}
