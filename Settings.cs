using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_PAB_INF3
{
    static public class Settings
    {
        public static string basicQuery = @$"
                     INSERT INTO [dbo].[Kody]
                               ([zipcode]
                               ,[address]
                               ,[city]
                               ,[province]
                               ,[district])
                         VALUES
                               ('@zipcode'
                               ,'@address'
                               ,'@city'
                               ,'@province'
                               ,'@district'
                    );
                ";
        public static string _connString = @"Server=as-it\Arek;Database=PABLab;User Id=sa;Password=Qaz123elo;";
    }
}
