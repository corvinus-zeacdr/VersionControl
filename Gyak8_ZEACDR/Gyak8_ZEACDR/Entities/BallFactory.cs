﻿using Gyak8_ZEACDR.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gyak8_ZEACDR.Entities
{
    public class BallFactory : IToyFactory
    {
    public Toy CreateNew()
    {
        return new Ball();
    }

    }
}
