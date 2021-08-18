using System.Text;
using IniParser;
using IniParser.Model;

namespace GenshinLauncher
{
    public abstract class Ini
    {
        protected readonly FileIniDataParser Parser = new FileIniDataParser();
        protected readonly IniData Data;

        protected Ini()
        {
            Data = new IniData();
        }

        protected Ini(string path)
        {
            Data = Parser.ReadFile(path, Encoding.UTF8);
        }

        public void WriteFile(string path) =>
            Parser.WriteFile(path, Data, Encoding.UTF8);
    }
}