# StoneAssemblies.Data
StoneAssemblies.Data extends the System.Data namespace by providing useful extension methods to query your database as fast as [Dapper](https://dapper-tutorial.net/dapper) does ;)

## Preliminary Benchmark Results

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
Intel Core i7-8750H CPU 2.20GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=5.0.403
  [Host]     : .NET 5.0.12 (5.0.1221.52207), X64 RyuJIT
  DefaultJob : .NET 5.0.12 (5.0.1221.52207), X64 RyuJIT

|                           Method | Iterations |        Mean |     Error |     StdDev |      Median |
|--------------------------------- |----------- |------------:|----------:|-----------:|------------:|
|               'Dapper reading C' |        100 |   130.89 ms |  2.575 ms |   5.023 ms |   130.61 ms |
| **'StoneAssemblies.Data reading C'** |        **100** |    **99.31 ms** |  **1.960 ms** |   **2.178 ms** |    **99.49 ms** |
|               'Dapper reading B' |        100 |   133.53 ms |  2.301 ms |   2.557 ms |   132.77 ms |
| **'StoneAssemblies.Data reading B'** |        **100** |   **123.18 ms** |  **2.214 ms** |   **2.071 ms** |   **123.33 ms** |
|               **'Dapper reading A'** |        **100** |   **127.06 ms** |  **2.321 ms** |   **2.171 ms** |   **126.62 ms** |
| 'StoneAssemblies.Data reading A' |        100 |   130.06 ms |  1.625 ms |   1.440 ms |   130.17 ms |
|               'Dapper reading C' |        500 |   637.42 ms | 11.279 ms |  18.214 ms |   634.51 ms |
| **'StoneAssemblies.Data reading C'** |        **500**|   **570.04 ms** |  **8.628 ms** |   **8.070 ms** |   **569.36 ms** |
|               'Dapper reading B' |        500 |   614.17 ms |  9.873 ms |   8.752 ms |   612.22 ms |
| **'StoneAssemblies.Data reading B'** |        **500** |  **598.14 ms** |  **8.328 ms** |   **7.790 ms** |   **597.69 ms** |
|               **'Dapper reading A'** |        **500** |   **624.50 ms** |  **8.224 ms** |   **9.471 ms** |   **623.71 ms** |
| 'StoneAssemblies.Data reading A' |        500 |   660.26 ms |  8.780 ms |   7.784 ms |   656.28 ms |
|               **'Dapper reading C'** |       **1000** | **1,183.65 ms** | **10.897 ms** |   **9.660 ms** | **1,183.97 ms** |
| 'StoneAssemblies.Data reading C' |       1000 | 1,198.48 ms | 22.103 ms |  20.675 ms | 1,191.79 ms |
|               'Dapper reading B' |       1000 | 1,287.50 ms | 22.109 ms |  20.681 ms | 1,278.64 ms |
| **'StoneAssemblies.Data reading B'** |       **1000** | **1,259.45 ms** | **23.284 ms** |  **21.780 ms** | **1,254.55 ms** |
|               **'Dapper reading A'** |       **1000** | **1,290.84 ms** | **60.200 ms** | **168.808 ms** | **1,216.74 ms** |
| 'StoneAssemblies.Data reading A' |       1000 | 1,412.35 ms | 27.861 ms |  28.611 ms | 1,403.92 ms |
 
 

