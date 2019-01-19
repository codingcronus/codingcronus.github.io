using MySql.Data.MySqlClient;
using System;
using System.Threading.Tasks;

namespace TddExample.Test.Infrastructure
{
    public class MySqlDockerServer : DockerServer
    {
        public string ConnectionString { get; }

        public MySqlDockerServer(string connectionString) : base (
            "MySqlIntegrationTestDb",
            "codingcronus/integrationtest-mysql", 
            "latest"
            )
        {
            ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        protected override async Task<bool> IsReady()
        {
            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    await conn.OpenAsync();

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
