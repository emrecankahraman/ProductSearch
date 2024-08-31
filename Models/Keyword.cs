using System;
using System.Collections.Generic;

namespace ProductSearch.Models;

public partial class Keyword
{
    public int KeywordId { get; set; }

    public string? KeywordText { get; set; }

    public int ProductId { get; set; }

    public virtual  Product ? Product { get; set; }
}

