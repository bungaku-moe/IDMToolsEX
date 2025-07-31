using System.Data;
using System.Diagnostics;
using Dapper;
using MySqlConnector;

namespace IDMToolsEX.Lib;

public class DatabaseService : IDisposable
{
    private readonly string _connectionString;
    private MySqlConnection? _connection;

    #region Connection

    public DatabaseService(string database, string host, string port, string username, string password)
    {
        _connectionString =
            $"Database={database};Server={host};Port={port};User Id={username};Password={password};Allow User Variables=true;Persist Security Info=True;Pooling=true;Connection Timeout=15;";
    }

    public bool IsConnected { get; private set; }


    public void Dispose()
    {
        _connection?.Dispose();
    }

    private async Task EnsureConnectedAsync()
    {
        try
        {
            _connection ??= new MySqlConnection(_connectionString);

            if (_connection.State != ConnectionState.Open)
            {
                Debug.WriteLine("Attempting to open MySQL connection...");
                await _connection.OpenAsync();
                Debug.WriteLine("MySQL connection opened successfully.");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("MySQL OpenAsync failed.");
            Debug.WriteLine("Exception: " + ex.Message);
            Debug.WriteLine("StackTrace: " + ex.StackTrace);
            if (ex.InnerException != null) Debug.WriteLine("InnerException: " + ex.InnerException.Message);

            throw; // Rethrow so you can see it in ConnectAsync too if needed
        }
    }

    public async Task<bool> ConnectAsync()
    {
        try
        {
            await EnsureConnectedAsync();
            IsConnected = _connection?.State == ConnectionState.Open;
            Debug.WriteLine("Connection result: " + IsConnected);
        }
        catch (Exception ex)
        {
            Debug.WriteLine("ConnectAsync caught exception: " + ex.Message);
            IsConnected = false;
        }

        return IsConnected;
    }

    public async Task DisconnectAsync()
    {
        if (_connection != null)
        {
            await _connection.CloseAsync();
            await _connection.DisposeAsync();
            _connection = null;
        }

        IsConnected = false;
    }

    #endregion

    #region Sales Report

    public async Task<List<(int Qty, decimal Price, string Time, string Rtype)>> GetTransactionDetailsAsync(
        string plu, DateTimeOffset date, int shift)
    {
        await EnsureConnectedAsync();

        const string query = """
                                 SELECT
                                     CAST(QTY AS SIGNED) AS QTY,
                                     CAST(PRICE AS DECIMAL(10, 2)) AS PRICE,
                                     CAST(JAM AS CHAR) AS JAM,
                                     CAST(RTYPE AS CHAR) AS RTYPE
                                 FROM mtran
                                 WHERE PLU = @Plu AND DATE(TANGGAL) = @Tanggal AND SHIFT = @Shift;
                             """;

        var result = await _connection.QueryAsync<(int Qty, decimal Price, string Time, string Rtype)>(query, new
        {
            Plu = plu,
            Tanggal = date.ToString("yyyy-MM-dd"),
            Shift = shift
        });

        return result.ToList();
    }

    #endregion

    #region Actual Cash

    public async Task<(decimal totalConsumentCash, decimal totalChangeCash, decimal totalActualCash)>
        GetExpectedActualCashAsync(
            DateTimeOffset date, int shift, int station)
    {
        await EnsureConnectedAsync();

        const string query = """
                             SELECT
                                 IFNULL(SUM(NILAI), 0) AS totalConsumentCash,
                                 IFNULL(SUM(KEMBALI), 0) AS totalChangeCash,
                                 IFNULL(SUM(NILAI), 0) - IFNULL(SUM(KEMBALI), 0) AS totalActualCash
                             FROM bayar
                             WHERE
                                 TANGGAL = @Tanggal AND
                                 SHIFT = @Shift AND
                                 TIPE = 'CSH' AND
                                 STATION = @Station;
                             """;

        var result =
            await _connection
                .QuerySingleOrDefaultAsync<(decimal totalConsumentCash, decimal totalChangeCash, decimal totalActualCash
                    )?>(
                    query,
                    new
                    {
                        Tanggal = date.ToString("yyyy-MM-dd"),
                        Shift = shift,
                        Station = station.ToString("D2")
                    });

        if (result is null)
            return (0, 0, 0);

        return result.Value;
    }

    public async Task<(decimal? TotalCashout, int Count)> GetTotalCashoutAsync(DateTimeOffset date, int shift,
        int station)
    {
        await EnsureConnectedAsync();

        const string query = """
                             SELECT
                                 IFNULL(SUM(TUNAI), 0) AS totalCashout,
                                 SUM(CASE WHEN TUNAI != 0 THEN 1 ELSE 0 END) AS countCashout
                             FROM bayar
                             WHERE
                                 TANGGAL = @Tanggal AND
                                 SHIFT = @Shift AND
                                 STATION = @Station;
                             """;

        var result = await _connection.QuerySingleOrDefaultAsync<(decimal? TotalCashout, int Count)>(query, new
        {
            Tanggal = date.ToString("yyyy-MM-dd"),
            Shift = shift,
            Station = station.ToString("D2")
        });

        if (result.TotalCashout is null)
            return (0, 0);

        return result;
    }

    public async Task<(decimal Cashout, decimal Struk)> GetTotalBeritaAcaraVirtualAsync(DateTimeOffset date, int shift,
        int station)
    {
        await EnsureConnectedAsync();

        const string query = """
                             SELECT
                                 IFNULL(SUM(NOMINAL_CASHOUT), 0) AS TotalCashout,
                                 IFNULL(SUM(NOMINAL_STRUK), 0) AS TotalStruk
                             FROM batv_virtual
                             WHERE
                                 BUKTI_TGL = @Tanggal AND
                                 SHIFT = @Shift AND
                                 STATION = @Station;
                             """;

        var result = await _connection.QuerySingleOrDefaultAsync<(decimal Cashout, decimal Struk)>(query, new
        {
            Tanggal = date.ToString("yyyy-MM-dd"),
            Shift = shift,
            Station = station.ToString("D2")
        });

        return result;
    }

    public async Task<decimal> GetTotalSalesDepositAsync(DateTimeOffset date, int shift, int station)
    {
        await EnsureConnectedAsync();

        const string query = """
                             SELECT
                                 AMBIL
                             FROM serah
                             WHERE
                                 TANGGAL = @Tanggal AND
                                 SHIFT = @Shift AND
                                 STATION = @Station;
                             """;

        var results = await _connection.QueryAsync<decimal>(query, new
        {
            Tanggal = date.ToString("yyyy-MM-dd"),
            Shift = shift,
            Station = station.ToString("D2")
        });

        return results.Sum();
    }

    public async Task<decimal> GetTotalCancelAsync(DateTimeOffset date, int shift, int station)
    {
        await EnsureConnectedAsync();

        const string query = """
                             SELECT
                                 GROSS
                             FROM cancel
                             WHERE
                                 TANGGAL = @Tanggal AND
                                 SHIFT = @Shift AND
                                 STATION = @Station;
                             """;

        var results = await _connection.QueryAsync<decimal>(query, new
        {
            Tanggal = date.ToString("yyyy-MM-dd"),
            Shift = shift,
            Station = station.ToString("D2")
        });

        return results.Sum();
    }

    #endregion

    #region Display Barang

    public async Task<IEnumerable<string>> GetBarcodesAsync(string plu)
    {
        await EnsureConnectedAsync();

        const string query = "SELECT BARCD FROM barcode WHERE PLU = @Plu;";
        return await _connection.QueryAsync<string>(query, new { Plu = plu });
    }

    public async Task<(string? Description, string? Abbreviation)> GetProductDescriptionAsync(string plu)
    {
        await EnsureConnectedAsync();

        const string query = "SELECT DESC2, SINGKATAN FROM prodmast WHERE PRDCD = @Plu;";
        return await _connection.QuerySingleOrDefaultAsync<(string?, string?)>(query, new { Plu = plu });
    }

    public async Task<IEnumerable<(string shelfName, string shelfDescription)>> GetModisAsync()
    {
        await EnsureConnectedAsync();

        const string query = "SELECT DISTINCT KODEMODIS, KET_RAK FROM rak ORDER BY KODEMODIS;";
        return await _connection.QueryAsync<(string, string)>(query);
    }

    public async Task<IEnumerable<string>> GetShelfNumbersAsync(string shelfName)
    {
        await EnsureConnectedAsync();

        const string query = """
                             SELECT DISTINCT NOSHELF
                             FROM rak
                             WHERE KODEMODIS = @ShelfName
                             ORDER BY NOSHELF ASC;
                             """;

        return await _connection.QueryAsync<string>(query, new { ShelfName = shelfName });
    }

    public async Task<IEnumerable<string>> GetShelfPluAsync(string shelfName, string shelfNumberFrom,
        string shelfNumberTo)
    {
        await EnsureConnectedAsync();

        const string query = """
                             SELECT PLUMD
                             FROM rak
                             WHERE KODEMODIS = @ShelfName AND NOSHELF BETWEEN @ShelfNumberFrom AND @ShelfNumberTo;
                             """;

        return await _connection.QueryAsync<string>(query,
            new { ShelfName = shelfName, ShelfNumberFrom = shelfNumberFrom, ShelfNumberTo = shelfNumberTo });
    }

    #endregion

    #region Price Tag

    public async Task<(decimal? Price, string? Description, string? Packaging, IEnumerable<string> Barcodes)>
        GetItemDetailsByPluAsync(string plu)
    {
        await EnsureConnectedAsync();

        const string query = """
                             SELECT
                                 p.PRICE,
                                 p.DESC2 AS Description,
                                 p.KEMASAN AS Packaging,
                                 b.BARCD AS Barcode
                             FROM prodmast p
                             LEFT JOIN barcode b ON p.PRDCD = b.PLU
                             WHERE p.PRDCD = @ProductCode;
                             """;

        var result =
            await _connection.QueryAsync<(decimal?, string?, string?, string)>(query, new { ProductCode = plu });

        if (!result.Any())
            return (null, null, null, []);

        (decimal? Price, string? Description, string? Packaging, IEnumerable<string> Barcodes)? groupedResult = result
            .GroupBy(r => new { r.Item1, r.Item2, r.Item3 })
            .Select(g => (
                Price: g.Key.Item1,
                Description: g.Key.Item2,
                Packaging: g.Key.Item3,
                Barcodes: g.Select(r => r.Item4)
            ))
            .FirstOrDefault();

        return groupedResult ?? (null, null, null, []);
    }

    public async Task<(decimal? Price, string? Plu, string? Description, string? Packaging)>
        GetItemPriceByBarcodeAsync(string barcode)
    {
        await EnsureConnectedAsync();

        const string query = """
                                 SELECT
                                     p.PRICE,
                                     p.PRDCD AS Plu,
                                     p.DESC2 AS Description,
                                     p.KEMASAN AS Packaging
                                 FROM barcode b
                                 INNER JOIN prodmast p ON b.PLU = p.PRDCD
                                 WHERE b.BARCD = @Barcode;
                             """;
        return await _connection.QuerySingleOrDefaultAsync<(decimal?, string?, string?, string?)>(query,
            new { Barcode = barcode });
    }

    public async Task<(decimal? Price, decimal? Promo, DateTimeOffset? Start, DateTimeOffset? End)?>
        GetPromotionByPluAsync(string plu)
    {
        await EnsureConnectedAsync();

        const string query = """
                             SELECT PRICE, PRO_RUPIAH, MULAI, AKHIR
                             FROM ptag_promo_marketing
                             WHERE PRDCD = @Plu
                             LIMIT 1;
                             """;

        return await _connection.QuerySingleOrDefaultAsync<(decimal?, decimal?, DateTimeOffset?, DateTimeOffset?)>(
            query, new
            {
                Plu = plu
            });
    }

    public async Task<(decimal? Price, decimal? Promo, DateTimeOffset? Start, DateTimeOffset? End)?>
        GetPromotionByBarcodeAsync(
            string barcode)
    {
        await EnsureConnectedAsync();

        const string query = """
                             SELECT PLU
                             FROM barcode
                             WHERE BARCD = @Barcode
                             LIMIT 1;
                             """;

        var plu = await _connection.QuerySingleOrDefaultAsync<string>(query, new
        {
            Barcode = barcode
        });

        if (string.IsNullOrEmpty(plu))
            return null;

        return await GetPromotionByPluAsync(plu);
    }

    public async Task<string?> GetExpiredByPluAsync(string plu)
    {
        await EnsureConnectedAsync();

        const string query = """
                             SELECT EXPIRED
                             FROM expired_main_old
                             WHERE PRDCD = @Plu
                             LIMIT 1;
                             """;

        return await _connection.QuerySingleOrDefaultAsync<string?>(query, new
        {
            Plu = plu
        });
    }

    #endregion
}
