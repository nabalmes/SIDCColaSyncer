﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIDCColaSyncer.Model.DTOs.SIDCAPIs
{
    public class SIDCBaseResponse
    {
        public bool Failed { get; set; }
        public bool Succeeded { get; set; }
        public string Message { get; set; }
    }
}
