public class GameOfLife
{
    public bool[,] Grid { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }

    public GameOfLife(int width, int height)
    {
        Width = width;
        Height = height;
        Grid = new bool[Width, Height];
        
        // Initialize the grid with a glider pattern
        Grid[Width / 2, Height / 2] = true;
        Grid[Width / 2 + 1, Height / 2 + 1] = true;
        Grid[Width / 2 + 2, Height / 2 - 1] = true;
        Grid[Width / 2 + 2, Height / 2] = true;
        Grid[Width / 2 + 2, Height / 2 + 1] = true;
    }

    public void Update()
    {
        // Update the state of the grid
        bool[,] newGrid = new bool[Width, Height];
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                int livingNeighbors = GetLivingNeighbors(x, y);

                // Apply the rules of the Game of Life
                if (Grid[x, y])
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
        Grid = newGrid;
    }

    private int GetLivingNeighbors(int x, int y)
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

                int nx = (x + dx + Width) % Width;
                int ny = (y + dy + Height) % Height;
                if (Grid[nx, ny])
                {
                    livingNeighbors++;
                }
            }
        }
        return livingNeighbors;
    }
}