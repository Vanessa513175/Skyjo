using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Logger
    {
        #region Enum
        public enum ELevelMessage
        {
            Info = 0,
            Warning = 1,
            Error = 2
        }
        #endregion

        #region Constants
        private const string FILE_PATH = "C:\\Users\\vanes\\Documents\\ApplicationUno\\Logs";
        #endregion

        #region Events
        #endregion

        #region Fields and Properties
        private static readonly Lazy<Logger> _instance = new(() => new Logger());
        public static Logger Instance => _instance.Value;

        public event Action<string> LogUpdated;
        #endregion

        #region Constructor
        private Logger()
        {
            string? directory = Path.GetDirectoryName(FILE_PATH);
            if (directory!=null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
        #endregion

        #region Methods

        #region Private and Protected Methods
        private static string GetLogFilePath()
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            return Path.Combine(FILE_PATH, $"log_{date}.txt");
        }

        private static void LogInFile(string message)
        {
            try
            {
                string filePath = GetLogFilePath();
                File.AppendAllText(filePath, message + Environment.NewLine);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de l'écriture dans le fichier : {ex.Message}");
            }
        }
        #endregion

        #region Public Methods
        public void Log(ELevelMessage level, string message)
        {
            string finalMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {message}";

            LogInFile(finalMessage);

            LogUpdated?.Invoke(finalMessage);

        }
        #endregion

        #endregion
    }
}
