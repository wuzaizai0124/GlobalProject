﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalProject.Infrastructure
{
  public class AuthResponse
  {
    public int Code { get; set; }
    public string Message { get; set; }
    public bool Success { get; set; }
  }
}
