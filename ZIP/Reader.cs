using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ZIP
{
    class Reader
    {
        public FileStream sourceStream = new FileStream(ProgramManager.sourceFile, FileMode.Open, FileAccess.Read);
        ObjForZipper objZ = new ObjForZipper();
        int readNumber = 0;
        public ObjForZipper ReadPortion()
        {
            sourceStream.Read(objZ.array,0,ProgramManager.size);
            objZ.readNumber = readNumber;
            readNumber++;
            return objZ;
        }

    }
}
