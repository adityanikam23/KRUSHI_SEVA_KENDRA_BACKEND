using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace KrushiSevaKendraMiniProject.Models;

[Table("crops")]
public partial class Crop
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(100)]
    public string? Name { get; set; }

    [InverseProperty("Crop")]
    public virtual ICollection<Recommendation> Recommendations { get; set; } = new List<Recommendation>();
}
