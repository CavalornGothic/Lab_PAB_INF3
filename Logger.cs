using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_PAB_INF3
{
    public static class Logger
    {
        public static string GetLevel(int level)
        {
            string lev = "";
            switch (level)
            {
                case 0:
                    {
                        lev = "[Info]";
                        break;
                    }
                case 1:
                    {
                        lev = "[Warning]";
                        break;
                    }
                case 2:
                    {
                        lev = "[Error]";
                        break;
                    }
            }
            return lev;
        }
        public static void ConsoleLog(int level, string content)
        {
            Console.WriteLine($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] {GetLevel(level)} : {content}");
        }
    }
}
