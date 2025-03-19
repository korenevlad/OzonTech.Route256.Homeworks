using System.Globalization;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;

namespace WarehousesApp.Application.Repositories.Implementations;
public class WarehouseRepositoryCsv : IWarehouseRepository
{
    private const string DirPath = "DataWarehouses";
    private readonly int _maxParallelFiles;
    private readonly int _maxParallelSummation;

    public WarehouseRepositoryCsv(IConfiguration configuration)
    {
        _maxParallelFiles = configuration.GetValue<int>("ParallelSettings:MaxParallelFiles");
        _maxParallelSummation = configuration.GetValue<int>("ParallelSettings:MaxParallelSummation");
    }
    public async Task<(int, double)> GetTotalCost(CancellationToken cancellationToken)
    {
        var directoryPath = GetPathDir();
        var files = Directory.GetFiles(directoryPath, "*.csv");
        var totalFiles = files.Length;
        var processedFiles = 0;
        var itemsCount = 0;
        var totalCost = 0.0;
        using var semaphore = new SemaphoreSlim(_maxParallelFiles);
        var tasks = files.Select(async file =>
        {
            await semaphore.WaitAsync(cancellationToken);
            try
            {
                var (fileItemCount, fileTotalCost) = await ProcessFileAsync(file, _maxParallelSummation, cancellationToken);
                
                Interlocked.Add(ref itemsCount, fileItemCount);
                double initialValue, newValue;
                do
                {
                    initialValue = totalCost;
                    newValue = initialValue + fileTotalCost;
                } while (Interlocked.CompareExchange(ref totalCost, newValue, initialValue) != initialValue);
                var currentProcessed = Interlocked.Increment(ref processedFiles);
                Console.WriteLine($"Warehouses processed: {currentProcessed}/{totalFiles}");
            }
            finally { semaphore.Release(); }
        }).ToList();
        await Task.WhenAll(tasks);
        return (itemsCount, totalCost);
    }

    private async Task<(int, double)> ProcessFileAsync(string filePath, int maxParallelSummation, CancellationToken cancellationToken)
    {
        var costs = new List<double>();
        var readDataTask = Task.Run(async () =>
        {
            await foreach (var cost in ReadFileAsync(filePath, cancellationToken))
            {
                costs.Add(cost);
            }
        }, cancellationToken);
        var fileItemsCount = 0;
        var fileTotalCost = 0.0;
        var semaphore = new SemaphoreSlim(1, 1);
        await readDataTask;
        await Parallel.ForEachAsync(
            costs, 
            new ParallelOptions { MaxDegreeOfParallelism = maxParallelSummation, CancellationToken = cancellationToken },
            async (cost, _) =>
            {
                await semaphore.WaitAsync(cancellationToken);
                try
                {
                    fileItemsCount ++;
                    fileTotalCost += cost; 
                }
                finally{ semaphore.Release(); }
                await Task.Delay(1000, cancellationToken); // искусственная задержка 
            });
        return (fileItemsCount, fileTotalCost);
    }
    
    private async IAsyncEnumerable<double> ReadFileAsync(string filePath, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        using var reader = new StreamReader(filePath);
        await reader.ReadLineAsync();
        string? row;
        while ((row = await reader.ReadLineAsync()) != null)
        {
            if (cancellationToken.IsCancellationRequested)
                yield break;
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