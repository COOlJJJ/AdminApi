﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminApi.Context.DomainModel
{
    public class Role:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
