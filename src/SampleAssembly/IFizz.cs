﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleAssembly
{
    public interface IFizz
    {
        IEnumerable<IBaz<int>> Services { get; }
    }
}
