// * Summary *

BenchmarkDotNet v0.15.8, Linux CachyOS
AMD Ryzen 5 3600 1.72GHz, 1 CPU, 12 logical and 6 physical cores
  [Host]     : .NET 10.0.8, X64 NativeAOT x86-64-v3
  DefaultJob : .NET 10.0.4, X64 NativeAOT x86-64-v3


| Method                        | ImageSize    | Mean      | Error    | StdDev   |
|------------------------------ |------------- |----------:|---------:|---------:|
| MetodoPadrao                  | (1200, 1200) | 620.42 ms | 0.487 ms | 0.431 ms |
| MetodoThreadsSimples          | (1200, 1200) | 194.22 ms | 0.542 ms | 0.507 ms |
| MetodoThreadsSimplesOtimizado | (1200, 1200) |  61.79 ms | 0.932 ms | 0.826 ms |
| MetodoParallel                | (1200, 1200) |  66.89 ms | 1.331 ms | 1.866 ms |
| MetodoParallelLowLevel        | (1200, 1200) |  39.99 ms | 0.699 ms | 0.620 ms |
| MetodoParallelLowLevelAVX2    | (1200, 1200) |  12.03 ms | 0.094 ms | 0.083 ms |

// * Hints *
Outliers
  Benchmarks.MetodoPadrao: Default                  -> 1 outlier  was  removed (621.67 ms)
  Benchmarks.MetodoThreadsSimplesOtimizado: Default -> 1 outlier  was  removed (63.99 ms)
  Benchmarks.MetodoParallel: Default                -> 1 outlier  was  removed (77.71 ms)
  Benchmarks.MetodoParallelLowLevel: Default        -> 1 outlier  was  removed, 2 outliers were detected (38.40 ms, 44.62 ms)
  Benchmarks.MetodoParallelLowLevelAVX2: Default    -> 1 outlier  was  removed, 2 outliers were detected (11.85 ms, 12.22 ms)

// * Legends *
  ImageSize : Value of the 'ImageSize' parameter
  Mean      : Arithmetic mean of all measurements
  Error     : Half of 99.9% confidence interval
  StdDev    : Standard deviation of all measurements
  1 ms      : 1 Millisecond (0.001 sec)

// ***** BenchmarkRunner: End *****
Run time: 00:01:43 (103.44 sec), executed benchmarks: 6

Global total time: 00:02:56 (176.67 sec), executed benchmarks: 6
