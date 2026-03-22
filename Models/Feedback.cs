using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWD392_MVC.Models;

public partial class Feedback
{
    [Key]
    public int FeedbackId { get; set; }

    public int? UserId    { get; set; }
    public int? ProductId { get; set; }

    // SDD: linked to a specific completed order (for customer review flow)
    public int? OrderId   { get; set; }

    public string? Content { get; set; }
    public int?    Rating  { get; set; }
    public bool?   Status  { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("Feedbacks")]
    public virtual Product? Product { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Feedbacks")]
    public virtual User? User { get; set; }

    [ForeignKey("OrderId")]
    public virtual Order? Order { get; set; }
}
