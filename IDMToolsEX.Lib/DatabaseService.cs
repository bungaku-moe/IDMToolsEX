using System.Data;
using Dapper;
using MySqlConnector;

namespace IDMToolsEX.Lib;

public class DatabaseService : IDisposable
{
    private readonly string _connectionString;
    private MySqlConnection? _connection;
    private bool _isConnected;

    public DatabaseService(string database, string host, string port, string username, string password)
    {
        _connectionString =
            $"Database={database};Server={host};Port={port};User Id={username};Password={password};Allow User Variables=true;Persist Security Info=True;Pooling=true;Connection Timeout=30;";
    }

    public bool IsConnected => _isConnected;

    public void Dispose()
    {
        _connection?.Dispose();
    }

    private async Task EnsureConnectedAsync()
    {
        if (!_isConnected) return; // Prevent reconnection if toggled off

        if (_connection == null)
        {
            _connection = new MySqlConnection(_connectionString);
        }

        if (_connection.State != ConnectionState.Open)
        {
            await _connection.OpenAsync();
        }
    }


    public async Task<bool> ToggleConnectionAsync()
    {
        if (_isConnected)
        {
            if (_connection != null)
            {
                await _connection.CloseAsync();
                await _connection.DisposeAsync(); // Ensure proper cleanup
                _connection = null; // Prevent reuse of the old connection
            }

            _isConnected = false;
        }
        else
        {
            await EnsureConnectedAsync();
            _isConnected = true;
        }

        return _isConnected;
    }


    private async Task<T?> GetSingleValueAsync<T>(string column, DateTimeOffset date, int shift, int station,
        string table = "initial")
        where T : struct
    {
        try
        {
            await EnsureConnectedAsync(); // Ensure the connection is open

            var query = $"""
                         SELECT {column} FROM {table}
                         WHERE TANGGAL = @Tanggal AND SHIFT = @Shift AND STATION = @Station
                         """;

            return await _connection.QueryFirstOrDefaultAsync<T?>(query, new
            {
                Tanggal = date.ToString("yyyy-MM-dd"),
                Shift = shift,
                Station = station.ToString("D2")
            });
        }
        catch (Exception e)
        {
            Console.WriteLine($"[Database Error] {e.Message}");
            throw;
        }
    }

    public Task<decimal?> GetKasAktualAsync(DateTimeOffset date, int shift, int station)
    {
        return GetSingleValueAsync<decimal>("KAS_AKTUAL", date, shift, station);
    }

    public Task<decimal?> GetKasComAsync(DateTimeOffset date, int shift, int station)
    {
        return GetSingleValueAsync<decimal>("KAS_COMP", date, shift, station);
    }

    public Task<decimal?> GetKSalesCashAsync(DateTimeOffset date, int shift, int station)
    {
        return GetSingleValueAsync<decimal>("SALES_CASH", date, shift, station);
    }

    public Task<decimal?> GetStationKasAktualAsync(DateTimeOffset date, int shift, int station)
    {
        return GetSingleValueAsync<decimal>("Station_kas_aktual", date, shift, station);
    }

    public Task<decimal?> GetMtranTodayAsync(DateTimeOffset date, int shift, int station)
    {
        return GetSingleValueAsync<decimal>("SUM(GROSS) AS Total", date, shift, station, "mtran");
    }

    public async Task<string> TestAsync()
    {
        try
        {
            await EnsureConnectedAsync();

            var query = "SELECT PLU, BARCODE, ADDTIME FROM barcode LIMIT 10"; // Ensure you select a valid column
            var result = await _connection.QueryAsync<string>(query);

            return result is not null ? string.Join(", ", result) : "No data found.";
        }
        catch (Exception e)
        {
            Console.WriteLine($"[Database Test Error] {e.Message}");
            return string.Empty;
        }
    }

    #region MYSQL

    public async Task<decimal?> GetSingleValue(string column, DateTimeOffset date, int shift, int station,
        string table = "initial")
    {
        try
        {
            await EnsureConnectedAsync();
            var query =
                $"SELECT {column} FROM {table} WHERE TANGGAL = @Tanggal AND SHIFT = @Shift AND STATION = @Station";

            using var cmd = new MySqlCommand(query, _connection);
            cmd.Parameters.AddWithValue("@Tanggal", date.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@Shift", shift);
            cmd.Parameters.AddWithValue("@Station", station.ToString("D2"));

            var result = await cmd.ExecuteScalarAsync();
            return result != DBNull.Value ? Convert.ToDecimal(result) : null;
        }
        catch (Exception e)
        {
            Console.WriteLine($"[Database Error] {e.Message}");
            return null;
        }
    }

    public Task<decimal?> GetKasAktual(DateTimeOffset date, int shift, int station)
    {
        return GetSingleValue("KAS_AKTUAL", date, shift, station);
    }

    public Task<decimal?> GetKasCom(DateTimeOffset date, int shift, int station)
    {
        return GetSingleValue("KAS_COMP", date, shift, station);
    }

    public Task<decimal?> GetKSalesCash(DateTimeOffset date, int shift, int station)
    {
        return GetSingleValue("SALES_CASH", date, shift, station);
    }

    public Task<decimal?> GetStationKasAktual(DateTimeOffset date, int shift, int station)
    {
        return GetSingleValue("Station_kas_aktual", date, shift, station);
    }

    public Task<decimal?> GetMtranToday(DateTimeOffset date, int shift, int station)
    {
        return GetSingleValue("SUM(GROSS)", date, shift, station, "mtran");
    }

    #endregion
}
