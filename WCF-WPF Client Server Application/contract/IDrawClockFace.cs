﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ServiceModel;

namespace contract
{
    [ServiceContract]
    public interface IDrawClockFace
    {
        [OperationContract]
        Bitmap DrawClockFace(DateTime time, int width, int height);
    }
}
