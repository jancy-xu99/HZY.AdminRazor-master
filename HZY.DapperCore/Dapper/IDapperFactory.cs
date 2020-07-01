namespace HZY.DapperCore.Dapper
{
    public interface IDapperFactory
    {
        DapperClient CreateClient(string name);
    }
}