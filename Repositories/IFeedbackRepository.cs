using SWD392_MVC.Models;

namespace SWD392_MVC.Repositories;

public interface IFeedbackRepository : IRepository<Feedback>
{
    IList<Feedback> GetFeedbacks(string? search = null, int? productId = null, bool? status = null);
    IList<Feedback> GetFeedbackByProduct(int productId);
    bool AddFeedback(Feedback feedback);
    bool ToggleStatus(int id);
    bool DeleteFeedback(int id);
    int  CountFeedbacks();
}
