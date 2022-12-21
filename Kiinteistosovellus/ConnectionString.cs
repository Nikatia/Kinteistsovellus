using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kiinteistosovellus
{
    public static class ConnectionString
    {
        static string account = CloudConfigurationManager.GetSetting("StorageAccountName");
        static string key = CloudConfigurationManager.GetSetting("StorageAccountKey");
        public static CloudStorageAccount GetConnectionString()
        {
            string connectionString = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", account, key);
            return CloudStorageAccount.Parse(connectionString);
        }
    }
}
