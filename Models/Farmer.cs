﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace KrushiSevaKendraMiniProject.Models;

[Table("farmers")]
public partial class Farmer
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(100)]
    public string? Name { get; set; }

    [Column("address")]
    [StringLength(1000)]
    public string? Address { get; set; }

    [Column("mobileno")]
    [StringLength(15)]
    public string? Mobileno { get; set; }

    [Column("email")]
    [StringLength(100)]
    public string? Email { get; set; }

    [Column("townid")]
    public int? Townid { get; set; }

    [InverseProperty("Farmer")]
    public virtual ICollection<Recommendation>? Recommendations { get; set; } = new List<Recommendation>();

    [ForeignKey("Townid")]
    [InverseProperty("Farmers")]
    public virtual Town? Town { get; set; } = null!;
}
