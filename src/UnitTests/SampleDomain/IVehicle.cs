﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Hiro.UnitTests.SampleDomain
{
    public interface IVehicle
    {
        IPerson Driver { get; set; }
    }
}
