using MySql.Data.MySqlClient;

namespace hhsl_api_server.Sql
{
    public class MySqlOperator
    {
        private static string connetStr = "server=127.0.0.1;port=3308;user=root;password=root; database=hhsl;charset=utf8;";
        private MySqlConnection mySql;

        //public static void Config(ServerConfigEntity entity)
        //{
        //    // todo 部署时候需要修改
        //    connetStr = $"server={entity.Ip};port={entity.MySql.Port};" +
        //                $"user={entity.MySql.User};password={entity.MySql.Password}; database={entity.MySql.Name};charset=utf8;";
        //}
        /// <summary>
        /// 连接
        /// </summary>
        public void Connect()
        {
            if (mySql == null)
            {
                mySql = new MySqlConnection(connetStr);
                mySql.Open();
            }
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public MySqlDataReader Reader(string sql)
        {
            if (mySql == null)
            {
                return null;
            }
            MySqlDataReader reader = null;
            MySqlCommand cmd = new MySqlCommand(sql, mySql);
            reader = cmd.ExecuteReader();
            return reader;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="sql"></param>
        public void Execute(string sql)
        {
            if (mySql == null)
            {
                return;
            }
            MySqlCommand cmd = new MySqlCommand(sql, mySql);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="sql"></param>
        public object ExecuteScalar(string sql)
        {
            if (mySql == null)
            {
                return null;
            }
            MySqlCommand cmd = new MySqlCommand(sql, mySql);
            return cmd.ExecuteScalar();
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void DisConnected()
        {
            if (mySql != null)
            {
                mySql.Close();
                mySql = null;
            }
        }

    }
}