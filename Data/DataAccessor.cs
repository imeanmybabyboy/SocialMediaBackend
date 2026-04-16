using Microsoft.EntityFrameworkCore;

namespace SocialMediaBackend.Data
{
    public record PagedResult<T>(
        IReadOnlyList<T> Items,
        int TotalCount,
        int Page,
        int PageSize
        )
    {
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
        public bool HasPreviousPage => Page > 1;
        public bool HasNextPage => Page < TotalPages;
    }

    public class DataAccessor(DataContext dataContext)
    {
        public async Task<List<Models.Post.Post>> GetPostsAsync(int page = 1, int pageSize = 10)
        {
            page = page < 1 ? 1 : page;

            Task<List<Models.Post.Post>> posts = dataContext
                .Posts
                .AsNoTracking()
                .Include(p => p.Comments)
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new Models.Post.Post
                {
                    Id = p.Id,
                    UserId = p.UserId,
                    Title = p.Title,
                    ImageUrl = p.ImageUrl,
                    Bio = p.Bio,
                    LikesQnt = p.LikesQnt,
                    SharesQnt = p.SharesQnt,
                    CreatedAt = p.CreatedAt,
                    DeletedAt = p.DeletedAt,
                    Comments = p.Comments.Select(c => new Models.Comment.Comment
                    {
                        Id = c.Id,
                        UserId = c.UserId,
                        PostId = c.PostId,
                        Bio = c.Bio,
                        LikesQnt = c.LikesQnt,
                        CreatedAt = c.CreatedAt,
                        DeletedAt = c.DeletedAt,
                        IsEdited = c.IsEdited,
                        EditedAt = c.EditedAt,
                    }).ToList()
                })
                .ToListAsync();

            return await posts;
        }

        public async Task<List<Models.Race.Race>> GetRacesAsync()
        {
            Task<List<Models.Race.Race>> raceTask = dataContext
                .Races
                .AsNoTracking()
                .Select(r => new Models.Race.Race
                {
                    Id = r.Id,
                    Name = r.Name,
                    ThemeColorHex = r.ThemeColorHex
                })
                .ToListAsync();

            return await raceTask;
        }

        public async Task<Entities.User?> GetUserAsyncByLogin(string login)
        {
            var user = dataContext
                .Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Login.Trim() == login && u.DeletedAt == null);
            return await user;
        }
    }
}
