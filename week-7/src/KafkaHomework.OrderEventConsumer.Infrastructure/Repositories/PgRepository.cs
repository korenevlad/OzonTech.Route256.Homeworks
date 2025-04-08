using System.Threading.Tasks;
using System.Transactions;
using KafkaHomework.OrderEventConsumer.Domain.Repositories;
using Npgsql;

namespace KafkaHomework.OrderEventConsumer.Infrastructure.Repositories;
public abstract class PgRepository : IPgRepository
{
    private readonly string _connectionString;
    protected const int DefaultTimeoutInSeconds = 5;
    protected PgRepository(string connectionString) => _connectionString = connectionString;
    protected async Task<NpgsqlConnection> GetConnection()
    {
        if (Transaction.Current is not null &&
            Transaction.Current.TransactionInformation.Status is TransactionStatus.Aborted)
        {
            throw new TransactionAbortedException("Transaction was aborted");
        }
        var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        connection.ReloadTypes();
        return connection;
    }
}