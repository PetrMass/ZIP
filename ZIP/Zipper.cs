using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading;

namespace ZIP
{
    class Zipper
    {
        public MemoryStream memStream;
        public GZipStream compressionStream;// поток сжатия
        ObjForWriter objW;
        public void Zip(object o)
        {
            objW = new ObjForWriter();
            memStream = new MemoryStream(0);
            compressionStream = new GZipStream(memStream, CompressionMode.Compress);

            Monitor.Enter(ProgramManager.lock1);
            ObjForZipper objZ = (ObjForZipper)o;
            objW.readNumber = objZ.readNumber;

            byte[] arrayZ = new byte[objZ.array.Length];

            int p = 0;
            foreach (byte _byte in objZ.array)
            {
                arrayZ[p] = _byte;
                p++;
            }
            Monitor.Exit(ProgramManager.lock1);
 
            compressionStream.Write(arrayZ, 0, arrayZ.Length); // собственно, та нагрузка, котора параллелится
            objW.memoryPosition = (int)memStream.Position;
            objW.array = memStream.ToArray();

            Monitor.Enter(ProgramManager.lock3); // блокирую коллекцию для загрузки в нее сжатого массива
            ProgramManager.list.Add(objW);
            Monitor.Exit(ProgramManager.lock3);
        }
    }
}
