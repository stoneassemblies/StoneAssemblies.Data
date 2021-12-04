# StoneAssemblies.Data
StoneAssemblies.Data extends the System.Data namespace by providing useful extension methods to query your database as fast as [Dapper](https://dapper-tutorial.net/dapper) does ;)

## Preliminary Benchmark Results

> BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
Intel Core i7-8750H CPU 2.20GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=5.0.403
  [Host]     : .NET 5.0.12 (5.0.1221.52207), X64 RyuJIT
  DefaultJob : .NET 5.0.12 (5.0.1221.52207), X64 RyuJIT
  
  
|                           Method | Iterations |       Mean |    Error |   StdDev |
|--------------------------------- |----------- |-----------:|---------:|---------:|
|               'Dapper reading A' |        100 |   135.6 ms |  2.67 ms |  5.08 ms |
| 'StoneAssemblies.Data reading A' |        100 |   133.1 ms |  2.40 ms |  2.47 ms |
|               'Dapper reading A' |        500 |   670.2 ms | 11.56 ms | 22.55 ms |
| 'StoneAssemblies.Data reading A' |        500 |   637.3 ms | 11.42 ms | 24.34 ms |
|               'Dapper reading A' |       1000 | 1,302.8 ms | 25.72 ms | 41.53 ms |
| 'StoneAssemblies.Data reading A' |       1000 | 1,377.2 ms | 25.32 ms | 22.44 ms |


|                           Method | Iterations |       Mean |    Error |    StdDev |
|--------------------------------- |----------- |-----------:|---------:|----------:|
|               'Dapper reading B' |        100 |   125.1 ms |  2.36 ms |   2.32 ms |
| 'StoneAssemblies.Data reading B' |        100 |   104.4 ms |  1.61 ms |   1.51 ms |
|               'Dapper reading B' |        500 |   667.1 ms |  6.36 ms |   5.31 ms |
| 'StoneAssemblies.Data reading B' |        500 |   646.1 ms | 10.66 ms |   8.33 ms |
|               'Dapper reading B' |       1000 | 1,299.5 ms | 17.39 ms |  14.52 ms |
| 'StoneAssemblies.Data reading B' |       1000 | 1,374.8 ms | 60.61 ms | 176.80 ms |


|                           Method | Iterations |       Mean |    Error |   StdDev |
|--------------------------------- |----------- |-----------:|---------:|---------:|
|               'Dapper reading C' |        100 |   108.0 ms |  2.15 ms |  3.58 ms |
| 'StoneAssemblies.Data reading C' |        100 |   127.0 ms |  2.15 ms |  2.48 ms |
|               'Dapper reading C' |        500 |   655.1 ms | 11.33 ms |  8.85 ms |
| 'StoneAssemblies.Data reading C' |        500 |   615.9 ms |  6.66 ms |  6.23 ms |
|               'Dapper reading C' |       1000 | 1,287.6 ms | 18.43 ms | 16.34 ms |
| 'StoneAssemblies.Data reading C' |       1000 | 1,225.0 ms | 15.76 ms | 13.16 ms |


 
 

