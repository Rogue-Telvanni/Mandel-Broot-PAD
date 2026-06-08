using BenchmarkDotNet.Running;
using Raylib_cs;


Raylib.InitWindow(800, 480, "Hello World");

while (!Raylib.WindowShouldClose())
{
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.White);

    Raylib.DrawText("Hello, world!", 12, 12, 20, Color.Black);

    Raylib.EndDrawing();
}

Raylib.CloseWindow();

//var size = (1200, 1200);
// Mandelbrot.RunThreadsSimplesOtimizado("treadOtimizada.pgm", size);

//_ = BenchmarkRunner.Run(typeof(Program).Assembly);