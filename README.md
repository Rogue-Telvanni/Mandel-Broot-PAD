// * Summary *

BenchmarkDotNet v0.15.8, Linux CachyOS
AMD Ryzen 5 3600 1.72GHz, 1 CPU, 12 logical and 6 physical cores
  [Host]     : .NET 10.0.4, X64 NativeAOT x86-64-v3
  DefaultJob : .NET 10.0.4, X64 NativeAOT x86-64-v3


| Method                        | ImageSize    | Mean      | Error    | StdDev   |
|------------------------------ |------------- |----------:|---------:|---------:|
| MetodoPadrao                  | (1200, 1200) | 628.15 ms | 5.609 ms | 5.246 ms |
| MetodoThreadsSimples          | (1200, 1200) | 196.93 ms | 0.888 ms | 0.831 ms |
| MetodoThreadsSimplesOtimizado | (1200, 1200) |  72.25 ms | 2.031 ms | 5.629 ms |
| MetodoParallel                | (1200, 1200) |  74.17 ms | 1.468 ms | 2.371 ms |
| MetodoParallelPrimitives      | (1200, 1200) |  42.68 ms | 0.847 ms | 1.159 ms |
| MetodoParallelLowLevelAVX2    | (1200, 1200) |  12.90 ms | 0.216 ms | 0.202 ms |
| MetodoPadraoAVX2              | (1200, 1200) |  92.66 ms | 0.451 ms | 0.422 ms |

// * Hints *
Outliers
  Benchmarks.MetodoThreadsSimplesOtimizado: Default -> 11 outliers were removed (90.04 ms..110.68 ms)
  Benchmarks.MetodoParallel: Default                -> 2 outliers were removed (83.30 ms, 84.23 ms)
  Benchmarks.MetodoParallelPrimitives: Default      -> 4 outliers were removed (46.32 ms..48.96 ms)
  Benchmarks.MetodoParallelLowLevelAVX2: Default    -> 4 outliers were removed (13.95 ms..14.47 ms)
  Benchmarks.MetodoPadraoAVX2: Default              -> 2 outliers were detected (91.65 ms, 91.90 ms)

// * Legends *
  ImageSize : Value of the 'ImageSize' parameter
  Mean      : Arithmetic mean of all measurements
  Error     : Half of 99.9% confidence interval
  StdDev    : Standard deviation of all measurements
  1 ms      : 1 Millisecond (0.001 sec)

// ***** BenchmarkRunner: End *****
Run time: 00:03:02 (182.56 sec), executed benchmarks: 7

Global total time: 00:04:23 (263.88 sec), executed benchmarks: 7
