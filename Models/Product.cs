using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProductSearch.Models;

public class Product
{

    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string? Name { get; set; }

    [StringLength(200)]
    public string? Description { get; set; }

    [Required]
    [StringLength(50)]
    public string? Department { get; set; }
    public int? Quantity { get; set; }
    public DateTime CreateDate { get; set; }
    public int CreatedById { get; set; }
    public DateTime? UpdateDate { get; set; }
    public int? UpdatedById { get; set; }
    public int SupervisorId { get; set; }
    public Supervisor? Supervisor { get; set; }
    public Supervisor? CreatedBy { get; set; }
    public Supervisor? UpdatedBy { get; set; }

    public int CoSupervisorId { get; set; }
    public Cosupervisor? CoSupervisor { get; set; }

    public ICollection<Keyword>? Keywords { get; set; }

}

