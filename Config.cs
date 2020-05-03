using Rocket.API;
using RocketmodTemplate.Databases;

namespace RocketmodTemplate
{
    public class Config : IRocketPluginConfiguration
    {
        public DatabaseConfig DatabaseConfig;
        public string Example;
        public void LoadDefaults()
        {
            Example = "example";

            DatabaseConfig = new DatabaseConfig
            {
                DatabaseAddress = "127.0.0.1",
                DatabaseUsername = "root",
                DatabasePassword = "123456",
                DatabaseName = "unturned",
                DatabaseTableName = "example",
                DatabasePort = 3306
            };
        }
    }
}