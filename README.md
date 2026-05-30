// * Summary *

BenchmarkDotNet v0.15.8, Linux CachyOS
AMD Ryzen 5 3600 1.72GHz, 1 CPU, 12 logical and 6 physical cores
  [Host]     : .NET 10.0.4, X64 NativeAOT x86-64-v3
  DefaultJob : .NET 10.0.4, X64 NativeAOT x86-64-v3


| Method                        | ImageSize    | threadSize | Mean      | Error    | StdDev   |
|------------------------------ |------------- |----------- |----------:|---------:|---------:|
| MetodoThreadsSimples          | (1200, 1200) | 2          | 315.96 ms | 1.813 ms | 1.607 ms |
| MetodoThreadsSimplesOtimizado | (1200, 1200) | 2          | 314.80 ms | 1.092 ms | 1.022 ms |
| MetodoParallel                | (1200, 1200) | 2          | 313.78 ms | 0.925 ms | 0.865 ms |
| MetodoThreadsSimples          | (1200, 1200) | 4          | 315.08 ms | 1.527 ms | 1.429 ms |
| MetodoThreadsSimplesOtimizado | (1200, 1200) | 4          | 159.59 ms | 0.472 ms | 0.442 ms |
| MetodoParallel                | (1200, 1200) | 4          | 160.98 ms | 0.748 ms | 0.700 ms |
| MetodoThreadsSimples          | (1200, 1200) | 6          | 300.32 ms | 1.012 ms | 0.845 ms |
| MetodoThreadsSimplesOtimizado | (1200, 1200) | 6          | 108.46 ms | 0.429 ms | 0.402 ms |
| MetodoParallel                | (1200, 1200) | 6          | 112.53 ms | 0.200 ms | 0.187 ms |
| MetodoThreadsSimples          | (1200, 1200) | 8          | 259.50 ms | 0.981 ms | 0.918 ms |
| MetodoThreadsSimplesOtimizado | (1200, 1200) | 8          |  86.06 ms | 0.361 ms | 0.320 ms |
| MetodoParallel                | (1200, 1200) | 8          |  87.58 ms | 0.624 ms | 0.487 ms |
| MetodoThreadsSimples          | (1200, 1200) | 10         | 223.48 ms | 0.750 ms | 0.665 ms |
| MetodoThreadsSimplesOtimizado | (1200, 1200) | 10         |  71.39 ms | 0.810 ms | 0.718 ms |
| MetodoParallel                | (1200, 1200) | 10         |  72.91 ms | 0.400 ms | 0.374 ms |
| MetodoThreadsSimples          | (1200, 1200) | 12         | 196.04 ms | 0.814 ms | 0.762 ms |
| MetodoThreadsSimplesOtimizado | (1200, 1200) | 12         |  66.52 ms | 1.278 ms | 1.569 ms |
| MetodoParallel                | (1200, 1200) | 12         |  65.68 ms | 1.086 ms | 1.016 ms |
| MetodoPadrao                  | (1200, 1200) | ?          | 625.90 ms | 2.320 ms | 1.937 ms |
| MetodoParallelPrimitives      | (1200, 1200) | ?          |  41.74 ms | 0.820 ms | 1.150 ms |
| MetodoParallelLowLevelAVX2    | (1200, 1200) | ?          |  12.54 ms | 0.134 ms | 0.105 ms |
| MetodoPadraoAVX2              | (1200, 1200) | ?          |  92.43 ms | 0.327 ms | 0.306 ms |

// * Hints *
Outliers
  Benchmarks.MetodoThreadsSimples: Default          -> 1 outlier  was  removed (326.18 ms)
  Benchmarks.MetodoThreadsSimples: Default          -> 2 outliers were removed (305.73 ms, 310.89 ms)
  Benchmarks.MetodoThreadsSimplesOtimizado: Default -> 1 outlier  was  removed (86.95 ms)
  Benchmarks.MetodoParallel: Default                -> 3 outliers were removed (91.18 ms..124.74 ms)
  Benchmarks.MetodoThreadsSimples: Default          -> 1 outlier  was  removed (225.22 ms)
  Benchmarks.MetodoThreadsSimplesOtimizado: Default -> 1 outlier  was  removed (73.91 ms)
  Benchmarks.MetodoPadrao: Default                  -> 2 outliers were removed (635.67 ms, 640.83 ms)
  Benchmarks.MetodoParallelPrimitives: Default      -> 1 outlier  was  removed (44.94 ms)
  Benchmarks.MetodoParallelLowLevelAVX2: Default    -> 3 outliers were removed (13.36 ms..19.46 ms)

// * Legends *
  ImageSize  : Value of the 'ImageSize' parameter
  threadSize : Value of the 'threadSize' parameter
  Mean       : Arithmetic mean of all measurements
  Error      : Half of 99.9% confidence interval
  StdDev     : Standard deviation of all measurements
  1 ms       : 1 Millisecond (0.001 sec)

// ***** BenchmarkRunner: End *****
Run time: 00:05:33 (333.24 sec), executed benchmarks: 22

Global total time: 00:06:55 (415.14 sec), executed benchmarks: 22
