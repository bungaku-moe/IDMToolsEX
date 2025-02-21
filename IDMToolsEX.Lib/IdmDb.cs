using MySql.Data.MySqlClient;

namespace IDMToolsEX.Lib;

public class IdmDb
{
    public MySqlConnection Connect(string database, string host, int port, string user, string password)
    {
        string connectionString =
            $"allow user variables=true;Persist Security Info=True;server={host};port={port};pooling=true;user id={user};password={password};connection timeout=75;database={database};ConvertZeroDateTime=True;";
        return new MySqlConnection(connectionString);
    }

    public int GetActualCash(MySqlCommand mySqlCommand, DateOnly date, byte shift, byte station)
    {
        mySqlCommand.CommandText =
            $"Select Kas_Aktual From Initial Where Tanggal='{date:yyyy-MM-dd}' And Shift='{shift}' And Station='{station}'";
        return Convert.ToInt32(mySqlCommand.ExecuteScalar());
    }

    public int GetSumGross(MySqlCommand mySqlCommand, DateOnly date, byte shift, byte station)
    {
        mySqlCommand.CommandText =
            $"Select Sum(Gross) From Mtran Where Tanggal='{date:yyyy-MM-dd}' And Shift='{shift}' And Station='{station}'";
        return Convert.ToInt32(mySqlCommand.ExecuteScalar());
    }
}
