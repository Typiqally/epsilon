﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epsilon.Abstractions.Model
{
    public class ExportData
    {
        public IEnumerable<CourseModule> CourseModules { get; set; } = Enumerable.Empty<CourseModule>();
    }
}
