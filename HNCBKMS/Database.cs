using System.Data.SqlClient;
using System.Windows.Forms;
using System;

public class Database
{
    private SqlConnection DBConn;
    private SqlConnectionStringBuilder DBConnString;
    private SqlCommand DBCmd;
    private SqlDataReader DBReader;
    private bool WindowsAuthentication;
    private string SQLServerLocation;
    private string InitialCatalog;
    private String UserID;
    private String Password;
    private int ConnectTimeout;
    public bool isConnection = false;

    public Database(string sqlServerLocation, string initialCatalog, int timeout = 10)
    {
        this.WindowsAuthentication = true;
        this.SQLServerLocation = sqlServerLocation.Trim();
        this.InitialCatalog = initialCatalog;
        this.ConnectTimeout = timeout;
    }

    public Database(string sqlServerLocation, string initialCatalog, string userID, string password, int timeout = 10)
    {
        this.WindowsAuthentication = false;
        this.SQLServerLocation = sqlServerLocation.Trim();
        this.InitialCatalog = initialCatalog;
        this.UserID = userID.Trim();
        this.Password = password.Trim();
        this.ConnectTimeout = timeout;
    }

    public bool Connect()
    {
        bool returnFlag = false;
        DBConnString = new SqlConnectionStringBuilder();
        try
        {
            DBConnString.DataSource = SQLServerLocation;
            if (WindowsAuthentication == true)
            {
                DBConnString.IntegratedSecurity = true;
            }
            else
            {
                DBConnString.IntegratedSecurity = false;
                DBConnString.UserID = UserID;
                DBConnString.Password = Password;

            }

            DBConnString.InitialCatalog = InitialCatalog;
            DBConnString.ConnectTimeout = ConnectTimeout;
            DBConn = new SqlConnection(DBConnString.ConnectionString);
            DBConn.Open();
            isConnection = true;
            returnFlag = true;
        }
        catch
        {
            returnFlag = false;
        }

        return returnFlag;
    }

    public SqlDataReader ExecuteQuery(string cmd)
    {
        DBCmd = new SqlCommand(cmd, DBConn);
        DBReader = DBCmd.ExecuteReader();          
        return DBReader;
    }

    public int ExecuteNonQuery(string cmd)
    {
        DBCmd = new SqlCommand(cmd, DBConn);
        return DBCmd.ExecuteNonQuery();
    }

    public bool Disconnect()
    {
        bool returnFlag = false;

        try
        {
            if (isConnection)
            {
                DBConnString = null;
                DBConn.Close();
                DBConn.Dispose();
                isConnection = false;
            }
            returnFlag = true;
        }
        catch
        {
            returnFlag = false;
        }

        return returnFlag;
    }

    public SqlConnection GetSQLConnection()
    {
        return DBConn;
    }

    public string GetSQLServerLocation()
    {
        return SQLServerLocation;
    }

    public string GetUserID()
    {
        return UserID;
    }

    public string GetDatabaseName()
    {
        return InitialCatalog;
    }
}

