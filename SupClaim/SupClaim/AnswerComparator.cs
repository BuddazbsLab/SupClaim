using NLog;
using System.Text;

namespace SupClaim
{
    internal class AnswerComparator
    {
        private string _buffer;
        private readonly Logger _logger;
        private readonly string _resultHendler;

        public AnswerComparator(Logger logger, string resultHendler)
        {
            this._logger = logger;
            this._resultHendler = resultHendler;
        }

        public string Buffer
        {
            get => _buffer;
            set => _buffer = value;
        }

        public string ResultHendler => this._resultHendler;

        public Logger LoggerInfo => this._logger;


        public async Task<bool> CompareDataObtained()
        {

            LoggerInfo.Info("[*]Читаю данные из файла");
            using (FileStream fstream = File.OpenRead("buffer.txt"))
            {

                byte[] buffer = new byte[fstream.Length];
                await fstream.ReadAsync(buffer);
                string textFromFile = Encoding.Default.GetString(buffer);
                Buffer = textFromFile;
            }

            LoggerInfo.Info("[*]Сравниваю данные полученные по API с теми что в файле");
            int compareString = string.Compare(Buffer, ResultHendler);

            if (compareString <= 0)
            {
                LoggerInfo.Info("[*]Получены новые данные. Записываю в файл.");

                using (FileStream fstream = new("buffer.txt", FileMode.OpenOrCreate))
                using (StreamWriter sr = new(fstream))
                {
                    await sr.WriteLineAsync(ResultHendler);
                }

                bool result = true;
                LoggerInfo.Info("[*]Напаривл статистику в телеграмм");
                LoggerInfo.Info("═════════════════════════════════════════════════════════════════════════════════");

                return result;
            }
            else
            {
                return false;
            }
        }
    }
}

