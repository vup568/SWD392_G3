using SWD392_MVC.Models;
using SWD392_MVC.Repositories;

namespace SWD392_MVC.Services;

public interface IBlogService
{
    IList<Post> GetActivePosts(string? search = null);
    IList<Post> GetAllPosts(string? search = null, bool? status = null);
    Post? GetById(int id);
    bool Create(Post post, int authorId);
    bool Update(Post post);
    bool ToggleStatus(int id);
    bool Delete(int id);
}

public class BlogService : IBlogService
{
    private readonly IPostRepository _postRepo;
    public BlogService(IPostRepository postRepo) { _postRepo = postRepo; }

    public IList<Post> GetActivePosts(string? search = null) => _postRepo.GetPublishedPosts(search);
    public IList<Post> GetAllPosts(string? search = null, bool? status = null) => _postRepo.GetPosts(search, status);
    public Post? GetById(int id) => _postRepo.GetPostDetails(id);
    public bool Create(Post post, int authorId) => _postRepo.CreatePost(post, authorId);
    public bool Update(Post post) => _postRepo.UpdatePost(post);
    public bool ToggleStatus(int id) => _postRepo.ToggleStatus(id);
    public bool Delete(int id) => _postRepo.DeletePost(id);
}
