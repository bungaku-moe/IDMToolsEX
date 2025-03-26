using System.Data;
using Dapper;
using MySql.Data.MySqlClient;

namespace IDMToolsEX.Lib;

public class DatabaseService : IDisposable
{
    private readonly string _connectionString;
    private MySqlConnection? _connection;

    public DatabaseService(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public bool IsConnected => _connection?.State == ConnectionState.Open;

    public void Dispose()
    {
        _connection?.Dispose();
    }

    public async Task<bool> ToggleConnectionAsync()
    {
        if (IsConnected)
        {
            await Task.Run(() => _connection?.Close());
            return false;
        }

        _connection = new MySqlConnection(_connectionString);
        await _connection.OpenAsync();
        return _connection.State == ConnectionState.Open;
    }

    private async Task<T?> GetSingleValueAsync<T>(string column, DateTimeOffset date, int shift, int station,
        string table = "initial")
        where T : struct
    {
        if (!IsConnected)
            throw new InvalidOperationException("Database is not connected.");

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
        return GetSingleValueAsync<decimal>("Sum(Gross)", date, shift, station, "Mtran");
    }
}
