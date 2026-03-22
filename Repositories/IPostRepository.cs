using SWD392_MVC.Models;

namespace SWD392_MVC.Repositories;

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
