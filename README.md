// * Summary *

BenchmarkDotNet v0.15.8, Linux CachyOS
AMD Ryzen 5 3600 1.72GHz, 1 CPU, 12 logical and 6 physical cores
  [Host]     : .NET 10.0.4, X64 NativeAOT x86-64-v3
  DefaultJob : .NET 10.0.4, X64 NativeAOT x86-64-v3


| Method                        | ImageSize    | threadSize | Mean      | Error    | StdDev   | Gen0     | Gen1     | Gen2     | Allocated |
|------------------------------ |------------- |----------- |----------:|---------:|---------:|---------:|---------:|---------:|----------:|
| MetodoThreadsSimples          | (1200, 1200) | 2          | 315.11 ms | 1.669 ms | 1.561 ms |        - |        - |        - |   1.38 MB |
| MetodoThreadsSimplesOtimizado | (1200, 1200) | 2          | 315.98 ms | 1.216 ms | 1.138 ms |        - |        - |        - |   1.38 MB |
| MetodoParallel                | (1200, 1200) | 2          | 313.63 ms | 0.963 ms | 0.853 ms |        - |        - |        - |   1.38 MB |
| MetodoThreadsSimples          | (1200, 1200) | 4          | 314.02 ms | 1.591 ms | 1.489 ms |        - |        - |        - |   1.38 MB |
| MetodoThreadsSimplesOtimizado | (1200, 1200) | 4          | 159.96 ms | 0.873 ms | 0.817 ms | 250.0000 | 250.0000 | 250.0000 |   1.38 MB |
| MetodoParallel                | (1200, 1200) | 4          | 160.98 ms | 0.763 ms | 0.713 ms | 250.0000 | 250.0000 | 250.0000 |   1.39 MB |
| MetodoThreadsSimples          | (1200, 1200) | 6          | 300.87 ms | 1.253 ms | 1.111 ms |        - |        - |        - |   1.38 MB |
| MetodoThreadsSimplesOtimizado | (1200, 1200) | 6          | 108.56 ms | 0.465 ms | 0.412 ms | 200.0000 | 200.0000 | 200.0000 |   1.38 MB |
| MetodoParallel                | (1200, 1200) | 6          | 112.88 ms | 0.754 ms | 0.705 ms | 200.0000 | 200.0000 | 200.0000 |   1.39 MB |
| MetodoThreadsSimples          | (1200, 1200) | 8          | 260.56 ms | 1.381 ms | 1.292 ms |        - |        - |        - |   1.38 MB |
| MetodoThreadsSimplesOtimizado | (1200, 1200) | 8          |  86.11 ms | 1.122 ms | 0.937 ms | 166.6667 | 166.6667 | 166.6667 |   1.38 MB |
| MetodoParallel                | (1200, 1200) | 8          |  88.00 ms | 0.918 ms | 0.858 ms | 166.6667 | 166.6667 | 166.6667 |   1.39 MB |
| MetodoThreadsSimples          | (1200, 1200) | 10         | 222.71 ms | 0.581 ms | 0.515 ms |        - |        - |        - |   1.38 MB |
| MetodoThreadsSimplesOtimizado | (1200, 1200) | 10         |  70.24 ms | 0.466 ms | 0.413 ms | 250.0000 | 250.0000 | 250.0000 |   1.38 MB |
| MetodoParallel                | (1200, 1200) | 10         |  72.42 ms | 0.241 ms | 0.226 ms | 142.8571 | 142.8571 | 142.8571 |   1.39 MB |
| MetodoThreadsSimples          | (1200, 1200) | 12         | 195.29 ms | 0.867 ms | 0.811 ms |        - |        - |        - |   1.38 MB |
| MetodoThreadsSimplesOtimizado | (1200, 1200) | 12         |  73.48 ms | 2.567 ms | 7.569 ms | 250.0000 | 250.0000 | 250.0000 |   1.38 MB |
| MetodoParallel                | (1200, 1200) | 12         |  68.45 ms | 1.356 ms | 3.061 ms | 250.0000 | 250.0000 | 250.0000 |    1.4 MB |
| MetodoThreadsSimples          | (1200, 1200) | 14         | 178.18 ms | 1.135 ms | 1.062 ms |        - |        - |        - |   1.38 MB |
| MetodoThreadsSimplesOtimizado | (1200, 1200) | 14         |  78.77 ms | 1.571 ms | 2.445 ms | 166.6667 | 166.6667 | 166.6667 |   1.38 MB |
| MetodoParallel                | (1200, 1200) | 14         |  69.79 ms | 1.393 ms | 2.440 ms | 250.0000 | 250.0000 | 250.0000 |    1.4 MB |
| MetodoThreadsSimples          | (1200, 1200) | 16         | 159.53 ms | 0.957 ms | 0.895 ms | 250.0000 | 250.0000 | 250.0000 |   1.38 MB |
| MetodoThreadsSimplesOtimizado | (1200, 1200) | 16         |  84.18 ms | 0.535 ms | 0.474 ms | 166.6667 | 166.6667 | 166.6667 |   1.38 MB |
| MetodoParallel                | (1200, 1200) | 16         |  73.71 ms | 1.445 ms | 2.334 ms | 250.0000 | 250.0000 | 250.0000 |    1.4 MB |
| MetodoThreadsSimples          | (1200, 1200) | 18         | 145.97 ms | 1.814 ms | 1.697 ms | 250.0000 | 250.0000 | 250.0000 |   1.38 MB |
| MetodoThreadsSimplesOtimizado | (1200, 1200) | 18         |  76.61 ms | 0.754 ms | 0.705 ms | 142.8571 | 142.8571 | 142.8571 |   1.38 MB |
| MetodoParallel                | (1200, 1200) | 18         |  74.24 ms | 1.475 ms | 2.208 ms | 250.0000 | 250.0000 | 250.0000 |    1.4 MB |
| MetodoPadrao                  | (1200, 1200) | ?          | 626.68 ms | 4.705 ms | 4.401 ms |        - |        - |        - |   1.38 MB |
| MetodoParallelPrimitives      | (1200, 1200) | ?          |  40.48 ms | 0.536 ms | 0.448 ms | 230.7692 | 230.7692 | 230.7692 |    1.4 MB |
| MetodoParallelLowLevelAVX2    | (1200, 1200) | ?          |  12.38 ms | 0.144 ms | 0.128 ms | 250.0000 | 250.0000 | 250.0000 |    1.4 MB |
| MetodoPadraoAVX2              | (1200, 1200) | ?          |  92.37 ms | 0.345 ms | 0.306 ms | 166.6667 | 166.6667 | 166.6667 |   1.38 MB |



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
