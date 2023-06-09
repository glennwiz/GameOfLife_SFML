using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

public class GameOfLife
{
    public Cell[,] Grid { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }

    public GameOfLife(int width, int height, bool[,] startPattern = null)
    {
        PatternList patterns;
        patterns = LoadPatternsFromJson();

        Grid = new Cell[width, height];
        this.Width = width;
        this.Height = height;
        // Initialize all cells as dead cells
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Grid[x, y] = new Cell { IsAlive = false, Age = 0 };
            }
        }

        InitializePattern(startPattern, width / 2 - 1, height / 2 - 1);
    }

    public static PatternList LoadPatternsFromJson()
    {
        PatternList patterns;
        using (StreamReader r = new StreamReader("patterns.json"))
        {
            string json = r.ReadToEnd();
            patterns = JsonConvert.DeserializeObject<PatternList>(json);
        }

        //return List<CellPattern> patterns;
        return patterns;

    }

    public void InitializePattern(bool[,] pattern, int offsetX, int offsetY)
    {
        Console.WriteLine($"Initializing pattern at ({offsetX}, {offsetY})");
        for (int y = 0; y < pattern.GetLength(0); y++)
        {
            for (int x = 0; x < pattern.GetLength(1); x++)
            {
                int finalX = x + offsetX;
                int finalY = y + offsetY;
                if(finalX >= Width || finalY >= Height)
                {
                    Console.WriteLine($"Skipping position ({finalX}, {finalY}) - out of bounds");
                    continue;
                }

                Console.WriteLine($"Setting position ({finalX}, {finalY}) to {pattern[y, x]}");
                Grid[finalX, finalY] = new Cell { IsAlive = pattern[y, x] };
            }
        }
    }
 

    public void Update()
    {
        Cell[,] newGrid = new Cell[Width, Height];
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                int livingNeighbors = GetLivingNeighbors(x, y);

                // Apply the rules of the Game of Life
                if (Grid[x, y].IsAlive)
                {
                    // Any live cell with two or three live neighbors lives on to the next generation
                    if (livingNeighbors == 2 || livingNeighbors == 3)
                    {
                        newGrid[x, y] = new Cell { IsAlive = true, Age = Grid[x, y].Age + 1 }; // Increment age
                    }
                    else
                    {
                        newGrid[x, y] = new Cell { IsAlive = false, Age = 0 }; // Reset age
                    }
                }
                else
                {
                    // Any dead cell with exactly three live neighbors becomes a live cell
                    if (livingNeighbors == 3)
                    {
                        newGrid[x, y] = new Cell { IsAlive = true, Age = 0 }; // Reset age
                    }
                    else
                    {
                        newGrid[x, y] = new Cell { IsAlive = false, Age = Grid[x, y].Age }; // Keep age
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
                if (Grid[nx, ny].IsAlive)
                {
                    livingNeighbors++;
                }
            }
        }
        return livingNeighbors;
    }
}