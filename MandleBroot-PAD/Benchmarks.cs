using BenchmarkDotNet.Attributes;

namespace MandleBroot_PAD;

public class Benchmarks
{
    [ParamsSource(nameof(Sizes))]
    public (int width, int height) ImageSize {  get; set; }
    
    public static IEnumerable<(int width, int height)> Sizes =>
    [
        (1200, 1200)
    ];
    
    [Benchmark]
    public void MetodoPadrao() =>
        Mandelbrot.Run("Padrao.pgm", ImageSize);
    
    [Benchmark]
    public void MetodoThreadsSimples() =>
        Mandelbrot.RunThreadsSimples("Thread.pgm", ImageSize);
    
    [Benchmark]
    public void MetodoThreadsSimplesOtimizado() => 
        Mandelbrot.RunThreadsSimplesOtimizado("ThreadOtimizado.pgm", ImageSize);
    
    [Benchmark]
    public void MetodoParallel() =>
        Mandelbrot.RunParallelFor("Parallel.pgm", ImageSize);
    
    [Benchmark]
    public void MetodoParallelLowLevel() =>
        Mandelbrot.RunParallelForLowLevel("ParallelLowLevel.pgm", ImageSize);
    
    
    [Benchmark]
    public void MetodoParallelLowLevelAVX2() =>
        Mandelbrot.RunParallelForLowLevelAVX2("ParallelLowLevelAVX2.pgm", ImageSize);
    
}