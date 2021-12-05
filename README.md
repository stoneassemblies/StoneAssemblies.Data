# StoneAssemblies.Data
StoneAssemblies.Data extends the System.Data namespace by providing useful extension methods to query your database as fast as [Dapper](https://dapper-tutorial.net/dapper) does ;)

Build Status
------------
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=StoneAssemblies.Data&metric=alert_status)](https://sonarcloud.io/dashboard?id=StoneAssemblies.Data)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=StoneAssemblies.Data&metric=ncloc)](https://sonarcloud.io/dashboard?id=StoneAssemblies.Data)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=StoneAssemblies.Data&metric=coverage)](https://sonarcloud.io/dashboard?id=StoneAssemblies.Data)

Branch | Status
------ | :------:
master | [![Build Status](https://dev.azure.com/alexfdezsauco/External%20Repositories%20Builds/_apis/build/status/stoneassemblies.StoneAssemblies.Data?branchName=master)](https://dev.azure.com/alexfdezsauco/External%20Repositories%20Builds/_build/latest?definitionId=15&branchName=master)
develop | [![Build Status](https://dev.azure.com/alexfdezsauco/External%20Repositories%20Builds/_apis/build/status/stoneassemblies.StoneAssemblies.Data?branchName=develop)](https://dev.azure.com/alexfdezsauco/External%20Repositories%20Builds/_build/latest?definitionId=15&branchName=develop)

Basic Usage
-------------

To read a single entity mapped from the reader result.

    var person = await dataReader.SingleAsync<Person>();
    
To read all entities mapped from the reader result.

    var persons = await dataReader.AllAsync<Person>().ToListAsync();

Preliminary Benchmark Results
-----------------------------

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


|                           Method | Iterations |       Mean |    Error |   StdDev |
|--------------------------------- |----------- |-----------:|---------:|---------:|
|               'Dapper reading B' |        100 |   131.3 ms |  3.65 ms | 10.24 ms |
| 'StoneAssemblies.Data reading B' |        100 |   139.1 ms |  4.48 ms | 12.78 ms |
|               'Dapper reading B' |        500 |   641.5 ms | 12.12 ms | 13.47 ms |
| 'StoneAssemblies.Data reading B' |        500 |   590.6 ms | 17.69 ms | 49.31 ms |
|               'Dapper reading B' |       1000 | 1,329.7 ms | 26.16 ms | 49.77 ms |
| 'StoneAssemblies.Data reading B' |       1000 | 1,107.7 ms | 21.13 ms | 24.33 ms |


|                           Method | Iterations |       Mean |    Error |   StdDev |
|--------------------------------- |----------- |-----------:|---------:|---------:|
|               'Dapper reading C' |        100 |   129.2 ms |  2.53 ms |  4.88 ms |
| 'StoneAssemblies.Data reading C' |        100 |   106.8 ms |  2.12 ms |  4.38 ms |
|               'Dapper reading C' |        500 |   680.8 ms | 13.15 ms | 12.30 ms |
| 'StoneAssemblies.Data reading C' |        500 |   575.4 ms | 11.31 ms | 14.30 ms |
|               'Dapper reading C' |       1000 | 1,328.3 ms | 26.22 ms | 53.56 ms |
| 'StoneAssemblies.Data reading C' |       1000 | 1,152.2 ms |  9.46 ms |  7.39 ms |


 
 

