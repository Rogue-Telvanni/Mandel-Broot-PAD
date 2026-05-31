using BenchmarkDotNet.Attributes;

namespace MandleBroot_PAD;

[MemoryDiagnoser]
public class Benchmarks
{
    [ParamsSource(nameof(Sizes))]
    public (int width, int height) ImageSize {  get; set; }

    public IEnumerable<int> threadNumber => [2, 4, 6, 8, 10, 12, 14, 16, 18];

    public static IEnumerable<(int width, int height)> Sizes =>
    [
        (1200, 1200)
    ];
    
    [Benchmark]
    public void MetodoPadrao() =>
        Mandelbrot.Run("Padrao.pgm", ImageSize);
    
    [Benchmark]
    [ArgumentsSource(nameof(threadNumber))]
    public void MetodoThreadsSimples(int threadSize) =>
        Mandelbrot.RunThreadsSimples("Thread.pgm", ImageSize, threadSize);
    
    [Benchmark]
    [ArgumentsSource(nameof(threadNumber))]
    public void MetodoThreadsSimplesOtimizado(int threadSize) => 
        Mandelbrot.RunThreadsSimplesOtimizado("ThreadOtimizado.pgm", ImageSize, threadSize);
    
    [Benchmark]
    [ArgumentsSource(nameof(threadNumber))]
    public void MetodoParallel(int threadSize) =>
        Mandelbrot.RunParallelFor("Parallel.pgm", ImageSize, threadSize);
    
    [Benchmark]
    public void MetodoParallelPrimitives() =>
        Mandelbrot.RunParallelForPrimitives("ParallelPrimitives.pgm", ImageSize);
    
    
    [Benchmark]
    public void MetodoParallelLowLevelAVX2() =>
        Mandelbrot.RunParallelForLowLevelAVX2("ParallelLowLevelAVX2.pgm", ImageSize);
    
    [Benchmark]
    public void MetodoPadraoAVX2() =>
        Mandelbrot.RunAVX2("PadraoAVX2.pgm", ImageSize);
    
}