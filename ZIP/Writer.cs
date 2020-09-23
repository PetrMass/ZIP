using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace ZIP
{
    class Writer
    {
        FileStream writeStream = new FileStream(ProgramManager.compressedFile, FileMode.OpenOrCreate, FileAccess.Write);
        int writeNumber = 0;
        public void WriteToFile()
        {
            while (ProgramManager.stop)
            {
                Monitor.Enter(ProgramManager.lock2);
                Monitor.Enter(ProgramManager.lock3); // блокирую List
                Monitor.Exit(ProgramManager.lock2);

                foreach (object _objW in ProgramManager.list)
                {
                    ObjForWriter objW = (ObjForWriter)_objW;
                    Console.WriteLine("{0}+{1}", objW.readNumber, writeNumber);

                    if (objW.readNumber == writeNumber)
                    {
                        writeStream.Write(objW.array, 0, objW.memoryPosition);
                        objW.readNumber = -1;
                        writeNumber++;
                    }                    
                }
                for (int i = 0; i < ProgramManager.list.Count; i+=0) // удаляю объекты, части которых были записаны
                {
                    ObjForWriter objW2 = (ObjForWriter)ProgramManager.list[i];
                    if (objW2.readNumber == -1) { ProgramManager.list.Remove(objW2);}
                    else { i++; }
                }
                Monitor.Exit(ProgramManager.lock3);
            } 
            Console.WriteLine("Close");
            writeStream.Close();
        }
    }
}
