﻿using ProductSearch.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductSearch.Models;
public class Supervisor : Person
{
    public string? Department {  get; set; }
    
}
