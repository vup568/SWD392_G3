using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SWD392_MVC.Models;

public partial class Post
{
    [Key]
    public int PostId { get; set; }

    [StringLength(200)]
    public string Title { get; set; } = null!;

    public string? Content { get; set; }

    public int? AuthorId { get; set; }

    public bool? Status { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("AuthorId")]
    [InverseProperty("Posts")]
    public virtual User? Author { get; set; }
}
