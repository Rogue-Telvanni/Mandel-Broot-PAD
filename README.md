// * Summary *

BenchmarkDotNet v0.15.8, Linux CachyOS
AMD Ryzen 5 3600 1.72GHz, 1 CPU, 12 logical and 6 physical cores
  [Host]     : .NET 10.0.4, X64 NativeAOT x86-64-v3
  DefaultJob : .NET 10.0.4, X64 NativeAOT x86-64-v3


| Method                        | ImageSize    | threadSize | Mean      | Error    | StdDev   | Gen0     | Gen1     | Gen2     | Allocated |
|------------------------------ |------------- |----------- |----------:|---------:|---------:|---------:|---------:|---------:|----------:|
| MetodoThreadsSimples          | (1200, 1200) | 2          | 315.89 ms | 1.463 ms | 1.369 ms |        - |        - |        - |   1.38 MB |
| MetodoThreadsSimplesOtimizado | (1200, 1200) | 2          | 315.32 ms | 1.586 ms | 1.483 ms |        - |        - |        - |   1.38 MB |
| MetodoParallel                | (1200, 1200) | 2          | 315.34 ms | 1.983 ms | 1.855 ms |        - |        - |        - |   1.38 MB |
| MetodoThreadsSimples          | (1200, 1200) | 4          | 314.87 ms | 1.924 ms | 1.706 ms |        - |        - |        - |   1.38 MB |
| MetodoThreadsSimplesOtimizado | (1200, 1200) | 4          | 159.63 ms | 0.561 ms | 0.468 ms | 250.0000 | 250.0000 | 250.0000 |   1.38 MB |
| MetodoParallel                | (1200, 1200) | 4          | 161.18 ms | 0.752 ms | 0.667 ms | 250.0000 | 250.0000 | 250.0000 |   1.39 MB |
| MetodoThreadsSimples          | (1200, 1200) | 6          | 300.00 ms | 1.203 ms | 1.066 ms |        - |        - |        - |   1.38 MB |
| MetodoThreadsSimplesOtimizado | (1200, 1200) | 6          | 108.18 ms | 0.574 ms | 0.480 ms | 200.0000 | 200.0000 | 200.0000 |   1.38 MB |
| MetodoParallel                | (1200, 1200) | 6          | 112.54 ms | 0.438 ms | 0.388 ms | 200.0000 | 200.0000 | 200.0000 |   1.39 MB |
| MetodoThreadsSimples          | (1200, 1200) | 8          | 260.60 ms | 1.443 ms | 1.349 ms |        - |        - |        - |   1.38 MB |
| MetodoThreadsSimplesOtimizado | (1200, 1200) | 8          |  85.93 ms | 0.287 ms | 0.269 ms | 166.6667 | 166.6667 | 166.6667 |   1.38 MB |
| MetodoParallel                | (1200, 1200) | 8          |  88.19 ms | 1.564 ms | 1.306 ms | 200.0000 | 200.0000 | 200.0000 |   1.39 MB |
| MetodoThreadsSimples          | (1200, 1200) | 10         | 223.98 ms | 0.729 ms | 0.646 ms |        - |        - |        - |   1.38 MB |
| MetodoThreadsSimplesOtimizado | (1200, 1200) | 10         |  71.74 ms | 1.119 ms | 2.129 ms | 250.0000 | 250.0000 | 250.0000 |   1.38 MB |
| MetodoParallel                | (1200, 1200) | 10         |  72.69 ms | 0.593 ms | 0.526 ms | 142.8571 | 142.8571 | 142.8571 |   1.39 MB |
| MetodoThreadsSimples          | (1200, 1200) | 12         | 196.24 ms | 0.591 ms | 0.553 ms |        - |        - |        - |   1.38 MB |
| MetodoThreadsSimplesOtimizado | (1200, 1200) | 12         |  67.63 ms | 1.351 ms | 2.063 ms | 250.0000 | 250.0000 | 250.0000 |   1.38 MB |
| MetodoParallel                | (1200, 1200) | 12         |  66.68 ms | 0.901 ms | 0.704 ms | 250.0000 | 250.0000 | 250.0000 |    1.4 MB |
| MetodoPadrao                  | (1200, 1200) | ?          | 626.73 ms | 4.610 ms | 4.312 ms |        - |        - |        - |   1.38 MB |
| MetodoParallelPrimitives      | (1200, 1200) | ?          |  41.73 ms | 0.811 ms | 1.189 ms | 230.7692 | 230.7692 | 230.7692 |    1.4 MB |
| MetodoParallelLowLevelAVX2    | (1200, 1200) | ?          |  12.75 ms | 0.223 ms | 0.186 ms | 250.0000 | 250.0000 | 250.0000 |    1.4 MB |
| MetodoPadraoAVX2              | (1200, 1200) | ?          |  92.37 ms | 0.465 ms | 0.435 ms | 166.6667 | 166.6667 | 166.6667 |   1.38 MB |


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
