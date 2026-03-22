using Microsoft.EntityFrameworkCore;
using SWD392_MVC.Models;

namespace SWD392_MVC.Repositories;

public class FeedbackRepository : BaseRepository<Feedback>, IFeedbackRepository
{
    public FeedbackRepository(OnlineShopContext context) : base(context) { }

    public IList<Feedback> GetFeedbacks(string? search = null, int? productId = null, bool? status = null)
    {
        var q = _context.Feedbacks
            .Include(f => f.User)
            .Include(f => f.Product)
            .Include(f => f.Order)
            .AsQueryable();
        if (!string.IsNullOrWhiteSpace(search)) q = q.Where(f => f.Content!.Contains(search));
        if (productId.HasValue) q = q.Where(f => f.ProductId == productId);
        if (status.HasValue) q = q.Where(f => f.Status == status);
        return q.OrderByDescending(f => f.CreatedAt).ToList();
    }

    public IList<Feedback> GetFeedbackByProduct(int productId)
        => _context.Feedbacks.Include(f => f.User)
            .Where(f => f.ProductId == productId && f.Status == true)
            .OrderByDescending(f => f.CreatedAt).ToList();

    public bool AddFeedback(Feedback feedback)
    {
        feedback.CreatedAt = DateTime.Now;
        feedback.Status    = true;
        _context.Feedbacks.Add(feedback);
        _context.SaveChanges();
        return true;
    }

    public bool ToggleStatus(int id)
    {
        var f = _context.Feedbacks.Find(id);
        if (f == null) return false;
        f.Status = !(f.Status ?? true);
        _context.SaveChanges();
        return true;
    }

    public bool DeleteFeedback(int id)
    {
        var f = _context.Feedbacks.Find(id);
        if (f == null) return false;
        _context.Feedbacks.Remove(f);
        _context.SaveChanges();
        return true;
    }

    public int CountFeedbacks() => _context.Feedbacks.Count();
}
