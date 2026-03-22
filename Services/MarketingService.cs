using SWD392_MVC.Models;
using SWD392_MVC.Repositories;

namespace SWD392_MVC.Services;

public interface IMarketingService
{
    // Sliders
    IList<Slider> GetAllSliders();
    Slider? GetSliderById(int id);
    bool CreateSlider(Slider slider);
    bool UpdateSlider(Slider slider);
    bool ToggleSliderStatus(int id);

    // Customers (read-only for Marketing)
    IList<User> GetCustomers(string? search = null);
    User? GetCustomerById(int id);

    // Feedbacks — Marketing can view & hide/show product reviews
    IList<Feedback> GetAllFeedbacks(string? search = null, int? productId = null, bool? status = null);
    bool ToggleFeedbackStatus(int id);
}

public class MarketingService : IMarketingService
{
    private readonly ISliderRepository   _sliderRepo;
    private readonly IUserRepository     _userRepo;
    private readonly IFeedbackRepository _feedbackRepo;

    public MarketingService(
        ISliderRepository   sliderRepo,
        IUserRepository     userRepo,
        IFeedbackRepository feedbackRepo)
    {
        _sliderRepo   = sliderRepo;
        _userRepo     = userRepo;
        _feedbackRepo = feedbackRepo;
    }

    public IList<Slider> GetAllSliders() => _sliderRepo.GetSliders();
    public Slider? GetSliderById(int id)  => _sliderRepo.GetById(id);
    public bool CreateSlider(Slider s)    => _sliderRepo.CreateSlider(s);
    public bool UpdateSlider(Slider s)    => _sliderRepo.UpdateSlider(s);
    public bool ToggleSliderStatus(int id) => _sliderRepo.ToggleStatus(id);

    public IList<User> GetCustomers(string? search = null)
    {
        var all = _userRepo.GetCustomers();
        if (!string.IsNullOrWhiteSpace(search))
            all = all.Where(u => u.FullName.Contains(search) || u.Email.Contains(search)).ToList();
        return all;
    }

    public User? GetCustomerById(int id) => _userRepo.GetCustomerWithOrders(id);

    public IList<Feedback> GetAllFeedbacks(string? search = null, int? productId = null, bool? status = null)
        => _feedbackRepo.GetFeedbacks(search, productId, status);

    public bool ToggleFeedbackStatus(int id) => _feedbackRepo.ToggleStatus(id);
}
