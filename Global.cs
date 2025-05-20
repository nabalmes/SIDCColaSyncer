using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIDCColaSyncer
{
    public class Global
    {
        private static string server = Properties.Settings.Default.HOST;
        private static string destinationDB = Properties.Settings.Default.DESTINATION_DB;
        private static string userName = Properties.Settings.Default.USERNAME;
        private static string password = PasswordDecode(Properties.Settings.Default.PASSWORD);
        private static string port = Properties.Settings.Default.PORT;


        public static string DestinationDatabase = $"Server={server};Database={destinationDB};Port={port};Username={userName};Password={password};SslMode=None;persistsecurityinfo=True;";

        public static string PasswordDecode(string passwordEncoded)
        {

            var passwordDecoded = Convert.FromBase64String(passwordEncoded);
            return Encoding.UTF8.GetString(passwordDecoded);
        }





        /// <summary>
        /// Get latest transaction number of header table in sofos2
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>

    }
}
