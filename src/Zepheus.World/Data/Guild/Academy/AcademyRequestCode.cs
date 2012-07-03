using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zepheus.World.Data
{
    enum AcademyRequestCode : ushort
    {
        Sucess = 6024,
        AlreadyExists = 6025,
        NotFound = 6027,
        AcademyFull = 6028,
        dbEror = 6029,
    }
}
