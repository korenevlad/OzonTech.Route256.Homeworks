using System.Globalization;
using Microsoft.Extensions.Configuration;

namespace WarehousesApp.Application.Repositories.Implementations;
public class WarehouseRepositoryCsv : IWarehouseRepository
{
    private const string DirPath = "DataWarehouses";
    private readonly int _maxParallelFiles;
    private readonly int _maxParallelSummation;

    // TODO: Cancelation token везде
    public WarehouseRepositoryCsv(IConfiguration configuration)
    {
        _maxParallelFiles = configuration.GetValue<int>("ParallelSettings:MaxParallelFiles");
        _maxParallelSummation = configuration.GetValue<int>("ParallelSettings:MaxParallelSummation");
    }
    public async Task<(int, double)> GetTotalCost()
    {
        var directoryPath = GetPathDir();
        var files = Directory.GetFiles(directoryPath, "*.csv");
        var totalFiles = files.Length;
        var processedFiles = 0;
        var itemsCount = 0;
        var totalCost = 0.0;
        object lockObj = new();
        using var semaphore = new SemaphoreSlim(_maxParallelFiles);
        var tasks = files.Select(async file =>
        {
            await semaphore.WaitAsync();
            try
            {
                var (fileItemCount, fileTotalCost) = await ProcessFileAsync(file, _maxParallelSummation);
                lock (lockObj)
                {
                    itemsCount += fileItemCount;
                    totalCost += fileTotalCost;
                }
                Interlocked.Increment(ref processedFiles);
                Console.WriteLine($"Warehouses processed: {processedFiles}/{totalFiles}");
            }
            finally { semaphore.Release(); }
        }).ToList();
        await Task.WhenAll(tasks);
        return (itemsCount, totalCost);
    }

    private async Task<(int, double)> ProcessFileAsync(string filePath, int maxParallelSummation)
    {
        var costs = new List<double>();
        var readDataTask = Task.Run(async () =>
        {
            await foreach (var cost in ReadFileAsync(filePath))
            {
                costs.Add(cost);
            }
        });
        var fileItemsCount = 0;
        var fileTotalCost = 0.0;
        object lockObj = new();
        await readDataTask;
        await Parallel.ForEachAsync(costs, new ParallelOptions { MaxDegreeOfParallelism = maxParallelSummation },
            async (cost, _) =>
            {
                lock (lockObj)
                {
                    fileItemsCount ++;
                    fileTotalCost += cost;
                }
                await Task.Delay(1000); // искусственная задержка 
            });
        return (fileItemsCount, fileTotalCost);
    }
    
    private async IAsyncEnumerable<double> ReadFileAsync(string filePath)
    {
        using var reader = new StreamReader(filePath);
        await reader.ReadLineAsync();
        string? row;
        while ((row = await reader.ReadLineAsync()) != null)
        {
            var splitedRow = row.Split(',');
            if (splitedRow.Length == 2 && double.TryParse(splitedRow[1], NumberStyles.Any, CultureInfo.InvariantCulture, out var cost))
            {
                yield return cost;
            }
        }
    }
    
    private string GetPathDir()
    {
        var projectRoot = GetProjectRoot();
        var relativePathDir = Path.Combine(projectRoot, "..", DirPath);
        var fullPathDir = Path.GetFullPath(relativePathDir);
        return fullPathDir;
    }
    
    private string GetProjectRoot()
    {
        var baseDirPath = new DirectoryInfo(AppContext.BaseDirectory);
        while (baseDirPath != null && !File.Exists(Path.Combine(baseDirPath.FullName, $"{baseDirPath.Name}.csproj")))
        {
            baseDirPath = baseDirPath.Parent;
        }
        return baseDirPath?.FullName ?? throw new Exception("Не удалось найти корень проекта!");
    }
}