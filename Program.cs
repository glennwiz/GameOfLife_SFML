using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace GameOfLifeSFML
{    
    public class Program
    {
        static void Main(string[] args)
        {
            // Create a new SFML window
            var mode = new VideoMode(800, 800);
            var window = new RenderWindow(mode, "Game of Life");

            // Handle the close events
            window.Closed += (s, e) => window.Close();

            // Initialize the game
            var game = new GameOfLife(30, 30);

            // The size of each cell in pixels
            var cellSize = new Vector2f(mode.Width / game.Width, mode.Height / game.Height);

            // Create a clock for controlling the frame rate
            var clock = new Clock();
            var timePerFrame = Time.FromSeconds(0.5f); // Here you can adjust the speed of the game

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
                            if (game.Grid[x, y])
                            {
                                var rectangle = new RectangleShape(cellSize)
                                {
                                    FillColor = Color.White,
                                    Position = new Vector2f(x * cellSize.X, y * cellSize.Y)
                                };
                                window.Draw(rectangle);
                            }
                        }
                    }

                    // Display the window
                    window.Display();
                }
            }
        }
    }
}
