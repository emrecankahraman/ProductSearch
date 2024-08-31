using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductSearch.Models;

public partial class Admin
{
   
    public int AdminId { get; set; }


    public string? Username { get; set; }

    public string? Password { get; set; }


    public string ? Role { get; set; } // Admin rolü için


    public byte[]  ? Salt { get; set; } // Add this line for storing the salt

}
