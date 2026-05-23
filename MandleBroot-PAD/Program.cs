using BenchmarkDotNet.Running;
using MandleBroot_PAD;

// var size = (1200, 1200);
// Mandelbrot.RunThreadsSimplesOtimizado("treadOtimizada.pgm", size);


var summary = BenchmarkRunner.Run(typeof(Program).Assembly);