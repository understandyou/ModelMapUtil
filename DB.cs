using SqlSugar;

namespace ConsoleApp1
{
    public static class DB
    {
        public static SqlSugarClient DBcontext  { get; set; }
        static DB()
        {
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = "Server=.;database=testa;uid=sa;pwd=sqlserver",//连接符字串
                DbType = DbType.SqlServer,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute//从特性读取主键自增信息
            });
            DBcontext = db;
        }

    }
}