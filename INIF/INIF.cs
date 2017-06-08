using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;

namespace INIF
{
    /// <summary>
    /// Класс INIF предназначен для работы с INI-файлами. Этот клас не наследуется.
    /// </summary>
    public sealed class INIF
    {
        /// <summary>
        /// Максимальный размер(для чтения).
        /// </summary>
        private int BuffSize = 1024;

        /// <summary>
        /// Храненимый путь к INI-файлу.
        /// </summary>
        private string TempPath = null;

        /// <summary>
        /// Объект INIF.
        /// </summary>
        private static INIF instance = null;

        /// <summary>
        /// Извлекает строку из указанного раздела в файле инициализации.
        /// </summary>
        /// <param name="AppName">Имя раздела, содержащего имя ключа. Если этот параметр имеет значение NULL,
        /// функция GetPrivateString копирует все имена разделов в файл в предоставленный буфер.</param>
        /// <param name="KeyName">Имя ключа, связанная строка которого должна быть извлечена.
        /// Если этот параметр имеет значение NULL, все имена ключей в разделе, указанном параметром appName,
        /// копируются в буфер, указанный параметром ReturnString .</param>
        /// <param name="LpDefault">Строка по умолчанию. Если ключ keyName не найден в файле инициализации,
        /// GetPrivateString копирует строку по умолчанию в буфер ReturnString.
        /// Если этот параметр имеет значение NULL, по умолчанию используется пустая строка "".
        /// Избегайте указания строки по умолчанию с пустыми символами в конце.Функция вставляет пустой
        /// символ в буфер returnString, чтобы удалить все конечные пробелы.</param>
        /// <param name="ReturnString">Указатель на буфер, получающий полученную строку.</param>
        /// <param name="Size">Размер буфера, на который указывает параметр ReturnString, в символах.</param>
        /// <param name="FileName">Имя файла инициализации. Если этот параметр не содержит полный путь к файлу,
        /// система выполняет поиск файла в каталоге Windows.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileString")]
        private static extern int GetPrivateString(string AppName, string KeyName, string LpDefault, StringBuilder ReturnString, int Size, string FileName);

        /// <summary>
        /// Копирует строку в указанный раздел файла инициализации.
        /// </summary>
        /// <param name="AppName">Имя раздела, в который будет скопирована строка.
        /// Если раздел не существует, он создается. Имя раздела не зависит от регистра;
        /// Строка может быть любой комбинацией прописных и строчных букв.</param>
        /// <param name="KeyName">Имя ключа, связанного со строкой.
        /// Если ключ не существует в указанном разделе, он создается.
        /// Если этот параметр имеет значение NULL, удаляется весь раздел, включая все записи в этом разделе.</param>
        /// <param name="LpString">Строка, завершающаяся значением NULL, которая записывается в файл.
        /// Если этот параметр имеет значение NULL, ключ, на который указывает параметр KeyName, удаляется.</param>
        /// <param name="FileName">Имя файла инициализации.
        /// Если файл был создан с помощью символов Юникода, функция записывает в файл символы Юникода.
        /// В противном случае функция записывает символы ANSI.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", EntryPoint = "WritePrivateProfileString")]
        private static extern int WritePrivateString(string AppName, string KeyName, string LpString, string FileName);

        /// <summary>
        /// Конструктор INIF.
        /// </summary>
        private INIF() {}

        /// <summary>
        /// Получает объект INIF.
        /// </summary>
        /// <param name="FileName">Имя INI-файла.</param>
        /// <returns></returns>
        public static INIF getInstance(string FileName = null)
        {
            if (INIF.instance == null)
            {
                INIF.instance = new INIF();
            }

            INIF.instance.Path = FileName;
            return INIF.instance;
        }

        /// <summary>
        /// Записываем значение в INI (по секции и ключу).
        /// </summary>
        /// <param name="Section">Имя раздела, в который будет скопирована строка.</param>
        /// <param name="Key">Имя ключа, связанного со строкой.</param>
        /// <param name="Value">Строка, завершающаяся значением NULL, которая записывается в файл.</param>
        public void Write(string Section, string Key, string Value)
        {
            INIF.WritePrivateString(Section, Key, Value, this.TempPath);
        }

        /// <summary>
        /// Возвращает значение из INI-Файла по секции и ключу.
        /// </summary>
        /// <param name="Section">Имя раздела, содержащего имя ключа.</param>
        /// <param name="Key">Имя ключа, связанная строка которого должна быть извлечена.</param>
        /// <returns></returns>
        public string Get(string Section, string Key)
        {
            StringBuilder buff = new StringBuilder(this.BuffSize);
            INIF.GetPrivateString(Section, Key, null, buff, this.BuffSize, this.TempPath);
            return buff.ToString();
        }

        /// <summary>
        /// Удаляем выбранную секцию.
        /// </summary>
        /// <param name="Section">имя удаляемого раздела.</param>
        public void DeleteSection(string Section)
        {
            this.Write(Section, null, null);
        }

        /// <summary>
        /// Возвращает или устанавливает путь к INI-файлу.
        /// </summary>
        public string Path
        {
            get { return this.TempPath; }
            set { this.TempPath = this.setFileName(value); }
        }

        /// <summary>
        /// Возвращает или устанавливает размер для чтения файла.
        /// </summary>
        public int Buffer
        {
            get { return this.BuffSize; }
            set { this.BuffSize = value; }
        }

        /// <summary>
        /// Устанавливает полный путь к файлу настроек.
        /// </summary>
        /// <param name="FileName">Имя INI-файла.</param>
        /// <returns></returns>
        private string setFileName(string FileName)
        {
            return System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + System.IO.Path.DirectorySeparatorChar + FileName;
        }
    }
}
