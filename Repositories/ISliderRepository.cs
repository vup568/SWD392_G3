using SWD392_MVC.Models;

namespace SWD392_MVC.Repositories;

public interface ISliderRepository : IRepository<Slider>
{
    IList<Slider> GetSliders();
    IList<Slider> GetActiveSliders();
    bool CreateSlider(Slider slider);
    bool UpdateSlider(Slider slider);
    bool ToggleStatus(int id);
    int  CountActiveSliders();
}
