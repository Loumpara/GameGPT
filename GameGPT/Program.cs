using System;
using System.Threading;

class PongGame
{
    // Constants for game difficulty and customization
    const int width = 40;
    const int height = 20;
    const int paddleHeight = 4;
    const char frameChar = '@';  // You can change this to any character you like (e.g., '*', '=', etc.)
    const int ballSpeed = 120;   // Lower value = faster ball, higher value = slower ball (adjust as needed)

    static int ballX = width / 2, ballY = height / 2;
    static int ballDirX = 1, ballDirY = 1;
    static int paddle1Y = height / 2 - 2, paddle2Y = height / 2 - 2;
    static int paddle1Score = 0, paddle2Score = 0;
    static bool gameOver = false;

    static void Main()
    {
        Console.CursorVisible = false;
        Console.SetWindowSize(width + 1, height + 1); // Set window size

        while (!gameOver)
        {
            Draw();
            Input();
            Logic();
            Thread.Sleep(ballSpeed); // Control game speed based on ballSpeed constant
        }

        Console.SetCursorPosition(width / 2 - 4, height / 2);
        Console.WriteLine("Game Over!");
        Console.SetCursorPosition(width / 2 - 5, height / 2 + 1);
        Console.WriteLine($"Score: {paddle1Score} - {paddle2Score}");
    }

    static void Draw()
    {
        Console.Clear();
        // Draw top border
        for (int i = 0; i < width; i++)
            Console.Write(frameChar);
        Console.WriteLine();

        // Draw the game area
        for (int y = 1; y < height - 1; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (x == 0 || x == width - 1)
                    Console.Write(frameChar); // Left and right borders
                else if (x == ballX && y == ballY)
                    Console.Write("O"); // Ball
                else if (x == 1 && y >= paddle1Y && y < paddle1Y + paddleHeight)
                    Console.Write("|"); // Left paddle
                else if (x == width - 2 && y >= paddle2Y && y < paddle2Y + paddleHeight)
                    Console.Write("|"); // Right paddle
                else
                    Console.Write(" "); // Empty space
            }
            Console.WriteLine();
        }

        // Draw bottom border
        for (int i = 0; i < width; i++)
            Console.Write(frameChar);
        Console.WriteLine();

        // Display scores
        Console.SetCursorPosition(width / 2 - 10, 0);
        Console.WriteLine($"Player 1: {paddle1Score} | Player 2: {paddle2Score}");
    }

    static void Input()
    {
        if (Console.KeyAvailable)
        {
            var key = Console.ReadKey(true).Key;
            // Control left paddle
            if (key == ConsoleKey.W && paddle1Y > 0) paddle1Y--;
            if (key == ConsoleKey.S && paddle1Y + paddleHeight < height - 1) paddle1Y++;

            // Control right paddle
            if (key == ConsoleKey.UpArrow && paddle2Y > 0) paddle2Y--;
            if (key == ConsoleKey.DownArrow && paddle2Y + paddleHeight < height - 1) paddle2Y++;
        }
    }

    static void Logic()
    {
        ballX += ballDirX;
        ballY += ballDirY;

        // Ball collision with top/bottom walls
        if (ballY == 0 || ballY == height - 1)
            ballDirY *= -1;

        // Ball collision with paddles
        if (ballX == 2 && ballY >= paddle1Y && ballY < paddle1Y + paddleHeight)
            ballDirX *= -1;
        if (ballX == width - 3 && ballY >= paddle2Y && ballY < paddle2Y + paddleHeight)
            ballDirX *= -1;

        // Ball goes out of bounds (left or right)
        if (ballX == 0)
        {
            paddle2Score++;
            ResetBall();
        }
        if (ballX == width - 1)
        {
            paddle1Score++;
            ResetBall();
        }
    }

    static void ResetBall()
    {
        ballX = width / 2;
        ballY = height / 2;
        ballDirX = ballDirX == 1 ? -1 : 1; // Change ball direction
        ballDirY = ballDirY == 1 ? -1 : 1; // Change ball direction
    }
}
