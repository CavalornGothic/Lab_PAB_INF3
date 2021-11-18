using System;
using System.Threading.Tasks;
using Lab_PAB_INF3.Lab_1;

namespace Lab_PAB_INF3
{
    class LabRun
    {
        static async Task Main(string[] args)
        {
            Logger.ConsoleLog(0, "wykonanie testowych metod z Lab nr 1");
            DataUploader uploader = new DataUploader();
            //uploader.SendDataLineByLine(await new DataCSVLoader(@"D:\kody.csv").GetAllLinesAsync());
            uploader.SendAllData(await new DataCSVLoader(@"D:\kody.csv").GetAllLinesAsync());
            Console.ReadKey();
        }
    }
}
