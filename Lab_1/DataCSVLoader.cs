using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Lab_PAB_INF3.Lab_1
{
    class DataCSVLoader
    {
        private readonly string _filePath;

        public DataCSVLoader(string filePath)
        {
            this._filePath = filePath;
        }

        public async Task<ICollection<LineDTO>> GetAllLinesAsync()
        {
            ICollection<LineDTO> data = null;
            try
            {
                data = new List<LineDTO>();
                int i = 0;
                foreach(var line in await File.ReadAllLinesAsync(_filePath))
                {
                    if(i > 0)
                        data.Add(new LineDTO { ZipCode = line.Split(";")[0], Address = line.Split(";")[1], City = line.Split(";")[2], Province = line.Split(";")[3], District = line.Split(";")[4] });
                    i++;
                }
            }
            catch(Exception ex)
            {
                Logger.ConsoleLog(2, $"podczas próby odczytania danych z pliku wystąpił błąd: {ex.Message}");
            }
            return data;
        }
    }
}
