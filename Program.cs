using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Collections.Generic;

public class Program
{
    private static Vector2f cellSize;
    private static Time timePerFrame = Time.FromSeconds(0.5f);
    private static Font font;
    private static Text instructionText;
   
    static void Main(string[] args)
    {
        // Create a new SFML window
        var mode = new VideoMode(800, 800);
        var window = new RenderWindow(mode, "Game of Life");

        var patterns = GameOfLife.LoadPatternsFromJson(); // Assume you have this function
        var startPattern = GetStartPattern(patterns.Patterns);
        Console.WriteLine("startPattern: " + startPattern);

        // Handle the close events
        window.Closed += (s, e) => window.Close();
        window.KeyPressed += HandleKeyPress;

        // Load the font
        font = new Font("font/CaskaydiaCoveNerdFont-Bold.ttf");

        // Initialize the instruction text
        instructionText = new Text("Speed up - Key up\nSpeed down - Key down", font, 20)
        {
            FillColor = Color.White,
            Position = new Vector2f(10, 10) // Position text in the top left corner
        };

        // Initialize the game
        var game = new GameOfLife(100, 100, startPattern);
        Console.WriteLine($"game.Width: {game.Width}, game.Height: {game.Height}, width: {mode.Width }, height: {mode.Height}");
        cellSize = new Vector2f(mode.Width / game.Width, mode.Height / game.Height);

        // Create a clock for controlling the frame rate
        var clock = new Clock();

        // Run the game loop
        while (window.IsOpen)
        {
            // Handle events
            window.DispatchEvents();

            // Only update the game every timePerFrame seconds
            if (clock.ElapsedTime >= timePerFrame)
            {
                // Reset the clock
                clock.Restart();

                // Update the game
                game.Update();

                // Clear the window
                window.Clear();

                // Draw the current state of the grid
                for (int y = 0; y < game.Height; y++)
                {
                    for (int x = 0; x < game.Width; x++)
                    {
                        var cell = game.Grid[x, y];
                        if (cell.IsAlive)
                        {
                            // Calculate a color based on the cell's age
                            var ageFactor = Math.Min(cell.Age / 3f, 1f); // Change the denominator to adjust the speed of color transition
                            var color = new Color((byte)(255 * ageFactor), 255, (byte)(255 * (1 - ageFactor)));

                            var rectangle = new RectangleShape(cellSize)
                            {
                                FillColor = color,
                                Position = new Vector2f(x * cellSize.X, y * cellSize.Y)
                            };
                            window.Draw(rectangle);
                        }
                    }
                }

                // Draw the instruction text
                window.Draw(instructionText);

                // Display the window
                window.Display();
            }
        }
    }

    private static bool[,] GetStartPattern(List<CellPattern> patterns)
    {
        Console.WriteLine("Select a pattern to start with:");
        for (int i = 0; i < patterns.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {patterns[i].Name}");
        }

        int choice;
        while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > patterns.Count)
            {
            Console.WriteLine("Invalid choice. Please enter a number corresponding to a pattern:");
        } 

        var selectedPattern = patterns[choice - 1];
        return selectedPattern.Pattern;
    }    


    private static void HandleKeyPress(object sender, KeyEventArgs e)
    {
        var window = (RenderWindow)sender;

        switch (e.Code)
        {
            case Keyboard.Key.Up:
                timePerFrame = Time.FromSeconds(timePerFrame.AsSeconds() - 0.1f);
                break;
            case Keyboard.Key.Down:
                timePerFrame = Time.FromSeconds(timePerFrame.AsSeconds() + 0.1f);
                break;
            default:
                break;
        }
    }
}