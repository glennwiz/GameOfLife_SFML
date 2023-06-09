using System;

namespace GameOfLife
{
    class Program
    {
        static void Main(string[] args)
        {
            // Parse the command-line arguments
            int width = 30;
            int height = 30;
            if (args.Length >= 2)
            {
                int.TryParse(args[0], out width);
                int.TryParse(args[1], out height);
            }

            // Initialize the grid with a "walker" pattern
            //bool[,] grid = new bool[width, height];
            //for (int x = width / 2 - 2; x <= width / 2 + 2; x++)
            //{
            //    grid[x, height / 2] = true;
            //}
	    
	    // Initialize the grid with a glider pattern
bool[,] grid = new bool[width, height];
grid[width / 2, height / 2] = true;
grid[width / 2 + 1, height / 2 + 1] = true;
grid[width / 2 + 2, height / 2 - 1] = true;
grid[width / 2 + 2, height / 2] = true;
grid[width / 2 + 2, height / 2 + 1] = true;

            // Run the game loop
            while (true)
            {Console.CursorVisible = false;
                Console.Clear();
		    // Print the current state of the grid
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    for (int x = 0; x < grid.GetLength(0); x++)
                    {
                        Console.Write(grid[x, y] ? "O" : " ");
                    }
                    Console.WriteLine();
                }

                // Update the state of the grid
                bool[,] newGrid = new bool[grid.GetLength(0), grid.GetLength(1)];
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    for (int x = 0; x < grid.GetLength(0); x++)
                    {
                        int livingNeighbors = GetLivingNeighbors(grid, x, y);

                        // Apply the rules of the Game of Life
                        if (grid[x, y])
                        {
                            // Any live cell with two or three live neighbors lives on to the next generation
                            if (livingNeighbors == 2 || livingNeighbors == 3)
                            {
                                newGrid[x, y] = true;
                            }
                        }
                        else
                        {
                            // Any dead cell with exactly three live neighbors becomes a live cell
                            if (livingNeighbors == 3)
                            {
                                newGrid[x, y] = true;
                            }
                        }
                    }
                }

                // Replace the old grid with the new grid
                grid = newGrid;

                // Wait for a short period of time before updating the grid again
                System.Threading.Thread.Sleep(500);
            }
        }

        // Returns the number of living neighbors for the cell at the specified position
    	// Returns the number of living neighbors for the cell at the specified position, with the edges of the grid wrapping around
static int GetLivingNeighbors(bool[,] grid, int x, int y)
{
    int livingNeighbors = 0;
    for (int dy = -1; dy <= 1; dy++)
    {
        for (int dx = -1; dx <= 1; dx++)
        {
            if (dx == 0 && dy == 0)
            {
                continue;
            }

            int nx = (x + dx + grid.GetLength(0)) % grid.GetLength(0);
            int ny = (y + dy + grid.GetLength(1)) % grid.GetLength(1);
            if (grid[nx, ny])
            {
                livingNeighbors++;
            }
        }
    }
    return livingNeighbors;
}


    }
}
