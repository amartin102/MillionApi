﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.MongoDb
{
    public class MongoSettings
    {
        public string ConnectionString { get; set; } = default!;
        public string Database { get; set; } = default!;
        public string Collection { get; set; } = "Properties";
    }
}
