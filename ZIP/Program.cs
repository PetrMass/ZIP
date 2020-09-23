using System;

namespace ZIP
{
    class Program
    {
        static void Main(string[] args)
        {
            ProgramManager programManager = new ProgramManager();
            programManager.Compress();
            //Console.WriteLine(stop);
            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
