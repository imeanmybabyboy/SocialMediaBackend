using Microsoft.EntityFrameworkCore;
using SocialMediaBackend.Data.Entities;

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
                .Include(p => p.Race)
                .Include(p => p.PostsInterests)
                    .ThenInclude(pi => pi.Interest)
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new Models.Post.Post
                {
                    Id = p.Id,
                    UserId = p.UserId,
                    Race = new Models.Race.Race
                    {
                        Id = p.User!.Race.Id,
                        Name = p.User.Race.Name,
                        ThemeColorHex = p.User.Race.ThemeColorHex,
                    },
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
                    }).ToList(),
                    Interests = p.PostsInterests
                    .Select(pi => new Models.Interest.Interest
                    {
                        Id = pi.Interest.Id,
                        Name = pi.Interest.Name,
                        Emoji = pi.Interest.Emoji,
                        Color = pi.Interest.Color,
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

        public async Task<List<Models.Interest.Interest>> GetInterestsAsync()
        {
            Task<List<Models.Interest.Interest>> interestsTask = dataContext
                .Interests
                .AsNoTracking()
                .Select(i => new Models.Interest.Interest
                {
                    Id = i.Id,
                    Name = i.Name,
                    Emoji = i.Emoji,
                    Color = i.Color
                })
                .ToListAsync();

            return await interestsTask;

        }

        public async Task<Entities.User?> GetUserByLoginAsync(string login)
        {
            var user = dataContext
                .Users
                .Include(u => u.Role)
                .Include(u => u.UserInterests)
                    .ThenInclude(ui => ui.Interest)
                .FirstOrDefaultAsync(u => u.Login.Trim() == login && u.DeletedAt == null);
            return await user;
        }

        public async Task<Entities.Race?> GetRaceByNameAsync(string raceName)
        {
            var race = dataContext
                .Races
                .FirstOrDefaultAsync(r => r.Name.Trim().ToLower() == raceName.Trim().ToLower());
            return await race;
        }

        public async Task<Entities.UserRole?> GetUserRoleAsync()
        {
            var role = dataContext
                .UserRoles
                .FirstOrDefaultAsync(r => r.Title.Trim().ToLower() == "user");

            return await role;
        }

        public async Task<Entities.UserRole?> GetAdminRoleAsync()
        {
            var role = dataContext
                .UserRoles
                .FirstOrDefaultAsync(r => r.Title.Trim().ToLower() == "admin");

            return await role;
        }

        public async Task AddUserAsync(Entities.User user)
        {
            await dataContext.Users.AddAsync(user);
            await dataContext.SaveChangesAsync();
        }

        public async Task<Entities.Interest?> GetInterestByNameAsync(string name)
        {
            return await dataContext.Interests
                .FirstOrDefaultAsync(i => i.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public async Task AddUserInterestAsync(UserInterest userInterest)
        {
            //await dataContext.UsersInterests.AddAsync(userInterest);
            await dataContext.SaveChangesAsync();
        }

        public async Task<Entities.User?> GetUserByEmailAsync(string email)
        {
            var user = dataContext
                .Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email.Trim().ToUpper() == email.Trim().ToUpper() && u.DeletedAt == null);

            return await user;
        }
    }
}