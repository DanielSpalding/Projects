using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FRIAS.BL.Interfaces
{
    public interface IComponentManager : IFactoryManager
    {
        ArrayList FetchPagedList(int id, object type, int startNum, int endNum);
    }
}
