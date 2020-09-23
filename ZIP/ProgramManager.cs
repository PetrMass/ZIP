using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ZIP
{
    class ProgramManager
    {
        public static string sourceFile = "D://test/book.pdf";
        public static string compressedFile = "D://test/book2.gz";
        public static string targetFile = "D://test/book3.pdf";

        public static object lock1 = new Object(); // блокирует доступ к части файла для zipper
        public static object lock2 = new Object();// блокирует запуск writer
        public static object lock3 = new Object();// блокирует коллекцию List
        public static int size = 1024000;
        public static List<object> list = new List<object>();
        List<Thread> threads = new List<Thread>();

        public static bool stop = true;
        int i = 0;
        ObjForZipper objZ = new ObjForZipper();
        bool w;

        Writer writer = new Writer();
        Reader reader = new Reader();
        Zipper zipper = new Zipper();

        public void Compress()
        {
            Thread thread = new Thread(writer.WriteToFile);
            thread.Start();

            while (i < reader.sourceStream.Length)
            {
                Monitor.Enter(lock2);
                for (int c = 0; c < 3; c++)
                {
                    Console.WriteLine(i);
                    Thread thread2 = new Thread(new ParameterizedThreadStart(zipper.Zip));
                    threads.Add(thread2);

                    Monitor.Enter(lock1);
                    objZ = reader.ReadPortion();
                    thread2.Start(objZ);
                    Monitor.Exit(lock1);

                    i += size;

                    Thread.Sleep(200);// ждем запуск потока чтоб он захватил лок1
                }
                Monitor.Exit(lock2);              
            }
            do
            {
                w = false;
                foreach (Thread h in threads) // проверка, что есть живые потоки
                {
                    if (h.IsAlive == true)
                    {
                        w = true;
                    }
                }
            }
            while (w); // цикл для проверки статуса выполнения потоков сжатия
            stop = false;
        }
    }
}
