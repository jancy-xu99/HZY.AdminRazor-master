namespace HZY.DapperCore.Dapper
{
    public class ConnectionConfig
    {
        public string ConnectionString { get; set; }
        public DbStoreType DbType { get; set; }
    }

    public enum DbStoreType
    {
        MySql = 0,
        SqlServer = 1,
        Sqlite = 2,
        Oracle = 3
    }
}