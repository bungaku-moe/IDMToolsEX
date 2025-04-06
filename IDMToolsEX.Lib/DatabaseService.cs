using System.Data;
using Dapper;
using MySqlConnector;

namespace IDMToolsEX.Lib;

public class DatabaseService : IDisposable
{
    private readonly string _connectionString;
    private MySqlConnection? _connection;

    public DatabaseService(string database, string host, string port, string username, string password)
    {
        _connectionString =
            $"Database={database};Server={host};Port={port};User Id={username};Password={password};Allow User Variables=true;Persist Security Info=True;Pooling=true;Connection Timeout=30;";
    }

    public bool IsConnected { get; private set; }

    public void Dispose()
    {
        _connection?.Dispose();
    }

    private async Task EnsureConnectedAsync()
    {
        if (!IsConnected) return;

        if (_connection == null) _connection = new MySqlConnection(_connectionString);

        if (_connection.State != ConnectionState.Open) await _connection.OpenAsync();
    }


    public async Task<bool> ToggleConnectionAsync()
    {
        if (IsConnected)
        {
            if (_connection != null)
            {
                await _connection.CloseAsync();
                await _connection.DisposeAsync();
                _connection = null;
            }

            IsConnected = false;
        }
        else
        {
            IsConnected = true; // Set before calling EnsureConnectedAsync
            await EnsureConnectedAsync();

            // If EnsureConnectedAsync fails, we assume connection was unsuccessful
            if (_connection?.State != ConnectionState.Open) IsConnected = false;
        }

        return IsConnected;
    }


    public async Task<(decimal totalCash, decimal totalChangeCash, decimal totalSalesCash)> GetExpectedActualCashAsync(
        DateTimeOffset date, int shift, int station)
    {
        await EnsureConnectedAsync();

        const string query = @"
        SELECT
            IFNULL(SUM(NILAI), 0) AS totalCash,
            IFNULL(SUM(KEMBALI), 0) AS changeCash,
            IFNULL(SUM(NILAI), 0) - IFNULL(SUM(KEMBALI), 0) AS totalSalesCash
        FROM bayar
        WHERE
            TANGGAL = @Tanggal AND
            SHIFT = @Shift AND
            TIPE = 'CSH' AND
            STATION = @Station;
    ";

        var result =
            await _connection
                .QuerySingleOrDefaultAsync<(decimal totalCash, decimal totalChangeCash, decimal totalSalesCash)?>(
                    query,
                    new
                    {
                        Tanggal = date.ToString("yyyy-MM-dd"),
                        Shift = shift,
                        Station = station.ToString("D2")
                    });

        // Null check: If result is null, return 0s
        if (result is null)
            return (0, 0, 0);

        return result.Value;
    }

//     private async Task<T?> GetSingleValueAsync<T>(string column, DateTimeOffset date, int shift, int station,
//         string table = "initial")
//         where T : struct
//     {
//         try
//         {
//             await EnsureConnectedAsync();
//
//             var query = $"""
//                          SELECT {column} FROM {table}
//                          WHERE TANGGAL = @Tanggal AND SHIFT = @Shift AND STATION = @Station
//                          """;
//
//             return await _connection.QueryFirstOrDefaultAsync<T?>(query, new
//             {
//                 Tanggal = date.ToString("yyyy-MM-dd"),
//                 Shift = shift,
//                 Station = station.ToString("D2")
//             });
//         }
//         catch (Exception e)
//         {
//             Console.WriteLine($"[Database Error] {e.Message}");
//             throw;
//         }
//     }
//
//     public Task<decimal?> GetKasAktualAsync(DateTimeOffset date, int shift, int station)
//     {
//         return GetSingleValueAsync<decimal>("KAS_AKTUAL", date, shift, station);
//     }
//
//     public async Task<string> TestAsync()
//     {
//         try
//         {
//             await EnsureConnectedAsync();
//
//             var query = "SELECT PLU, BARCODE, ADDTIME FROM barcode LIMIT 10"; // Ensure you select a valid column
//             var result = await _connection.QueryAsync<string>(query);
//
//             return result is not null ? string.Join(", ", result) : "No data found.";
//         }
//         catch (Exception e)
//         {
//             Console.WriteLine($"[Database Test Error] {e.Message}");
//             return string.Empty;
//         }
//     }
}
