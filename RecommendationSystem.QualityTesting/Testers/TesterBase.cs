using System;
using System.Diagnostics;
using System.IO;

namespace RecommendationSystem.QualityTesting.Testers
{
    public abstract class TesterBase : ITester
    {
        protected readonly Stopwatch Timer = new Stopwatch();
        protected TextWriter FileWriter;
        public string TestName { get; set; }

        public abstract void Test();

        protected void InitializeResultWriter(string filename)
        {
            var dir = Path.GetDirectoryName(filename);
            if (dir != null && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            FileWriter = new StreamWriter(filename);
        }

        protected void Write(string text, bool toConsole = true, bool toFile = true)
        {
            if (toConsole)
                Console.WriteLine(text);

            if (!toFile)
                return;

            FileWriter.WriteLine(text);
            FileWriter.Flush();
        }
    }
}