using Microsoft.EntityFrameworkCore;
using SWD392_MVC.Models;

namespace SWD392_MVC.Repositories;

// ─────────────────────────────────────────────────────────────────────────────
// POST
// ─────────────────────────────────────────────────────────────────────────────
public interface IPostRepository : IRepository<Post>
{
    IList<Post> GetPosts(string? search = null, bool? status = null);
    IList<Post> GetPublishedPosts(string? search = null);
    Post? GetPostDetails(int id);
    bool CreatePost(Post post, int authorId);
    bool UpdatePost(Post post);
    bool ToggleStatus(int id);
    bool DeletePost(int id);
    int  CountPosts();
}

public class PostRepository : BaseRepository<Post>, IPostRepository
{
    public PostRepository(OnlineShopContext context) : base(context) { }

    public IList<Post> GetPosts(string? search = null, bool? status = null)
    {
        var q = _context.Posts.Include(p => p.Author).AsQueryable();
        if (!string.IsNullOrWhiteSpace(search)) q = q.Where(p => p.Title.Contains(search));
        if (status.HasValue) q = q.Where(p => p.Status == status);
        return q.OrderByDescending(p => p.CreatedAt).ToList();
    }

    public IList<Post> GetPublishedPosts(string? search = null)
    {
        var q = _context.Posts.Include(p => p.Author).Where(p => p.Status == true).AsQueryable();
        if (!string.IsNullOrWhiteSpace(search)) q = q.Where(p => p.Title.Contains(search));
        return q.OrderByDescending(p => p.CreatedAt).ToList();
    }

    public Post? GetPostDetails(int id)
        => _context.Posts.Include(p => p.Author).FirstOrDefault(p => p.PostId == id);

    public bool CreatePost(Post post, int authorId)
    {
        post.AuthorId  = authorId;
        post.CreatedAt = DateTime.Now;
        post.Status    = true;
        _context.Posts.Add(post);
        _context.SaveChanges();
        return true;
    }

    public bool UpdatePost(Post post)
    {
        var existing = _context.Posts.Find(post.PostId);
        if (existing == null) return false;
        existing.Title   = post.Title;
        existing.Content = post.Content;
        _context.SaveChanges();
        return true;
    }

    public bool ToggleStatus(int id)
    {
        var post = _context.Posts.Find(id);
        if (post == null) return false;
        post.Status = !(post.Status ?? true);
        _context.SaveChanges();
        return true;
    }

    public bool DeletePost(int id)
    {
        var post = _context.Posts.Find(id);
        if (post == null) return false;
        _context.Posts.Remove(post);
        _context.SaveChanges();
        return true;
    }

    public int CountPosts() => _context.Posts.Count(p => p.Status == true);
}

// ─────────────────────────────────────────────────────────────────────────────
// SLIDER
// ─────────────────────────────────────────────────────────────────────────────
public interface ISliderRepository : IRepository<Slider>
{
    IList<Slider> GetSliders();
    IList<Slider> GetActiveSliders();
    bool CreateSlider(Slider slider);
    bool UpdateSlider(Slider slider);
    bool ToggleStatus(int id);
    int  CountActiveSliders();
}

public class SliderRepository : BaseRepository<Slider>, ISliderRepository
{
    public SliderRepository(OnlineShopContext context) : base(context) { }

    public IList<Slider> GetSliders()       => _context.Sliders.ToList();
    public IList<Slider> GetActiveSliders() => _context.Sliders.Where(s => s.Status == true).ToList();

    public bool CreateSlider(Slider slider)
    {
        _context.Sliders.Add(slider);
        _context.SaveChanges();
        return true;
    }

    public bool UpdateSlider(Slider slider)
    {
        var existing = _context.Sliders.Find(slider.SliderId);
        if (existing == null) return false;
        existing.Title    = slider.Title;
        existing.ImageUrl = slider.ImageUrl;
        existing.Link     = slider.Link;
        _context.SaveChanges();
        return true;
    }

    public bool ToggleStatus(int id)
    {
        var s = _context.Sliders.Find(id);
        if (s == null) return false;
        s.Status = !(s.Status ?? true);
        _context.SaveChanges();
        return true;
    }

    public int CountActiveSliders() => _context.Sliders.Count(s => s.Status == true);
}

// ─────────────────────────────────────────────────────────────────────────────
// SETTING
// ─────────────────────────────────────────────────────────────────────────────
public interface ISettingRepository : IRepository<Setting>
{
    IList<Setting> GetSettings(string? search = null);
    bool CreateSetting(Setting setting);
    bool UpdateSetting(Setting setting);
    bool ToggleStatus(int id);
}

public class SettingRepository : BaseRepository<Setting>, ISettingRepository
{
    public SettingRepository(OnlineShopContext context) : base(context) { }

    public IList<Setting> GetSettings(string? search = null)
    {
        var q = _context.Settings.AsQueryable();
        if (!string.IsNullOrWhiteSpace(search)) q = q.Where(s => s.SettingKey!.Contains(search));
        return q.ToList();
    }

    public bool CreateSetting(Setting setting)
    {
        _context.Settings.Add(setting);
        _context.SaveChanges();
        return true;
    }

    public bool UpdateSetting(Setting setting)
    {
        var existing = _context.Settings.Find(setting.SettingId);
        if (existing == null) return false;
        existing.SettingKey   = setting.SettingKey;
        existing.SettingValue = setting.SettingValue;
        _context.SaveChanges();
        return true;
    }

    public bool ToggleStatus(int id)
    {
        var s = _context.Settings.Find(id);
        if (s == null) return false;
        s.Status = !(s.Status ?? true);
        _context.SaveChanges();
        return true;
    }
}
