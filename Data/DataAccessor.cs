using Microsoft.EntityFrameworkCore;
using SocialMediaBackend.Data.Entities;
using SocialMediaBackend.Models.Post;
using System.Formats.Asn1;

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
        public async Task<List<Models.Post.Post>> GetPostsAsync(string? currentUserId, int page = 1, int pageSize = 5)
        {
            page = page < 1 ? 1 : page;
            Guid? userGuid = string.IsNullOrWhiteSpace(currentUserId) ? null : Guid.Parse(currentUserId);

            Task<List<Models.Post.Post>> posts = dataContext
                .Posts
                .AsNoTracking()
                .Include(p => p.Comments)
                .Include(p => p.Race)
                .Include(p => p.PostsInterests)
                    .ThenInclude(pi => pi.Interest)
                .Include(p => p.Likes)
                .Include(p => p.Saves)
                .Where(p => !p.IsPrivate)
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
                    IsLiked = userGuid != null && p.Likes.Any(l => l.UserId == userGuid.Value),
                    IsSaved = userGuid != null && p.Saves.Any(s => s.UserId == userGuid.Value),
                    LikesQnt = p.Likes.Count,
                    SharesQnt = p.SharesQnt,
                    CreatedAt = p.CreatedAt,
                    DeletedAt = p.DeletedAt,
                    Comments = p.Comments.Select(c => new Models.Comment.CommentViewModel
                    {
                        Id = c.Id,
                        UserId = c.UserId,
                        PostId = c.PostId,
                        Bio = c.Bio,
                        LikesQnt = c.Likes.Count,
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
                }).ToListAsync();

            return await posts;
        }
        public async Task<List<Models.Post.Post>> GetPrivatePostsAsync(string? currentUserId, int page = 1, int pageSize = 5)
        {
            page = page < 1 ? 1 : page;
            Guid? userGuid = string.IsNullOrWhiteSpace(currentUserId) ? null : Guid.Parse(currentUserId);

            Task<List<Models.Post.Post>> posts = dataContext
                .Posts
                .AsNoTracking()
                .Include(p => p.Comments)
                .Include(p => p.Race)
                .Include(p => p.PostsInterests)
                    .ThenInclude(pi => pi.Interest)
                .Include(p => p.Likes)
                .Include(p => p.Saves)
                .Where(p => p.IsPrivate)
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
                    LikesQnt = p.Likes.Count,
                    IsLiked = userGuid != null && p.Likes.Any(l => l.UserId == userGuid.Value),
                    IsSaved = userGuid != null && p.Saves.Any(s => s.UserId == userGuid.Value),
                    SharesQnt = p.SharesQnt,
                    CreatedAt = p.CreatedAt,
                    DeletedAt = p.DeletedAt,
                    Comments = p.Comments.Select(c => new Models.Comment.CommentViewModel
                    {
                        Id = c.Id,
                        UserId = c.UserId,
                        PostId = c.PostId,
                        Bio = c.Bio,
                        LikesQnt = c.Likes.Count,
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
        public async Task<List<Models.Post.Post>> GetUsersPostsAsync(string currentUserId, int page = 1, int pageSize = 5)
        {
            page = page < 1 ? 1 : page;
            Guid? userGuid = string.IsNullOrWhiteSpace(currentUserId) ? null : Guid.Parse(currentUserId);

            Task<List<Models.Post.Post>> posts = dataContext
                .Posts
                .AsNoTracking()
                .Include(p => p.Comments)
                .Include(p => p.Race)
                .Include(p => p.PostsInterests)
                    .ThenInclude(pi => pi.Interest)
                .Include(p => p.Likes)
                .Include(p => p.Saves)
                .Where(p => p.UserId.ToString() == currentUserId)
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
                    LikesQnt = p.Likes.Count,
                    IsLiked = userGuid != null && p.Likes.Any(l => l.UserId == userGuid.Value),
                    IsSaved = userGuid != null && p.Saves.Any(s => s.UserId == userGuid.Value),
                    SharesQnt = p.SharesQnt,
                    CreatedAt = p.CreatedAt,
                    DeletedAt = p.DeletedAt,
                    Comments = p.Comments.Select(c => new Models.Comment.CommentViewModel
                    {
                        Id = c.Id,
                        UserId = c.UserId,
                        PostId = c.PostId,
                        Bio = c.Bio,
                        LikesQnt = c.Likes.Count,
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
        public async Task<List<Models.Post.Post>> GetUserLikedPostsAsync(string currentUserId, int page = 1, int pageSize = 5)
        {
            page = page < 1 ? 1 : page;
            Guid? userGuid = string.IsNullOrWhiteSpace(currentUserId) ? null : Guid.Parse(currentUserId);

            Task<List<Models.Post.Post>> posts = dataContext
                .Posts
                .AsNoTracking()
                .Include(p => p.Comments)
                .Include(p => p.Race)
                .Include(p => p.PostsInterests)
                    .ThenInclude(pi => pi.Interest)
                .Include(p => p.Likes)
                .Include(p => p.Saves)
                .Where(p => p.Likes.Any(l => l.UserId == userGuid))
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
                    LikesQnt = p.Likes.Count,
                    IsLiked = true,
                    IsSaved = userGuid != null && p.Saves.Any(s => s.UserId == userGuid.Value),
                    SharesQnt = p.SharesQnt,
                    CreatedAt = p.CreatedAt,
                    DeletedAt = p.DeletedAt,
                    Comments = p.Comments.Select(c => new Models.Comment.CommentViewModel
                    {
                        Id = c.Id,
                        UserId = c.UserId,
                        PostId = c.PostId,
                        Bio = c.Bio,
                        LikesQnt = c.Likes.Count,
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
        public async Task<List<Models.Post.Post>> GetUserSavedPostsAsync(string currentUserId, int page = 1, int pageSize = 5)
        {
            page = page < 1 ? 1 : page;
            Guid? userGuid = string.IsNullOrWhiteSpace(currentUserId) ? null : Guid.Parse(currentUserId);

            Task<List<Models.Post.Post>> posts = dataContext
                .Posts
                .AsNoTracking()
                .Include(p => p.Comments)
                .Include(p => p.Race)
                .Include(p => p.PostsInterests)
                    .ThenInclude(pi => pi.Interest)
                .Include(p => p.Likes)
                .Include(p => p.Saves)
                .Where(p => p.Saves.Any(l => l.UserId == userGuid))
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
                    LikesQnt = p.Likes.Count,
                    IsLiked = userGuid != null && p.Likes.Any(l => l.UserId == userGuid.Value),
                    IsSaved = true,
                    SharesQnt = p.SharesQnt,
                    CreatedAt = p.CreatedAt,
                    DeletedAt = p.DeletedAt,
                    Comments = p.Comments.Select(c => new Models.Comment.CommentViewModel
                    {
                        Id = c.Id,
                        UserId = c.UserId,
                        PostId = c.PostId,
                        Bio = c.Bio,
                        LikesQnt = c.Likes.Count,
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

        public async Task AddPostAsync(Entities.Post post)
        {
            await dataContext.Posts.AddAsync(post);
            await dataContext.SaveChangesAsync();
        }

        public async Task AddPostInterestsAsync(List<PostInterest> postInterests)
        {
            await dataContext.PostsInterests.AddRangeAsync(postInterests);
            await dataContext.SaveChangesAsync();
        }
        public async Task<Entities.Post?> GetPostByIdAsync(string id)
        {
            var post = dataContext
                .Posts
                .Include(p => p.Race)
                .Include(p => p.Comments)
                .Include(p => p.PostsInterests)
                    .ThenInclude(pi => pi.Interest)
                .FirstOrDefaultAsync(p => p.Id.ToString() == id);

            if (post == null)
                throw new Exception($"Post with id {id} not found");

            return await post;
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

        public async Task<Entities.Race?> GetRaceByIdAsync(string id)
        {
            var race = dataContext
                .Races
                .FirstOrDefaultAsync(r => r.Id.ToString().ToLower() == id.ToLower());
            return await race;
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

        public async Task<List<Entities.Interest>> GetInterestByIdAsync(string[] ids)
        {
            return await dataContext.Interests
                .Where(i => ids.Contains(i.Id.ToString()))
                .ToListAsync();
        }
        public async Task DeleteUserInterestsAsync(string userId)
        {
            var interests = dataContext
                .UsersInterests
                .Where(ui => ui.UserId.ToString() == userId);
            dataContext.UsersInterests.RemoveRange(interests);
            await dataContext.SaveChangesAsync();
        }

        public async Task<Entities.User?> GetUserByLoginAsync(string login)
        {
            var user = dataContext
                .Users
                .Include(u => u.Role)
                .Include(u => u.Race)
                .Include(u => u.UserInterests)
                    .ThenInclude(ui => ui.Interest)
                .FirstOrDefaultAsync(u => u.Login.Trim().ToLower() == login.ToLower() && u.DeletedAt == null);

            return await user;
        }
        public async Task<Entities.User?> GetUserByIdAsync(string id)
        {
            var user = dataContext
                .Users
                .Include(u => u.Role)
                .Include(u => u.Race)
                .Include(u => u.UserInterests)
                    .ThenInclude(ui => ui.Interest)
                .FirstOrDefaultAsync(u => u.Id.ToString() == id && u.DeletedAt == null);
            return await user;
        }
        public async Task AddUserAsync(Entities.User user)
        {
            await dataContext.Users.AddAsync(user);
            await dataContext.SaveChangesAsync();
        }
        public async Task AddUserInterestsAsync(List<UserInterest> userInterests)
        {
            await dataContext.UsersInterests.AddRangeAsync(userInterests);
            await dataContext.SaveChangesAsync();
        }
        public async Task<Entities.User?> GetUserByEmailAsync(string email)
        {
            var user = dataContext
                .Users
                .Include(u => u.Role)
                .Include(u => u.Race)
                .FirstOrDefaultAsync(u => u.Email.Trim().ToUpper() == email.Trim().ToUpper() && u.DeletedAt == null);

            return await user;
        }
        public async Task UpdateUserAsync(Entities.User user)
        {
            dataContext.Users.Update(user);
            await dataContext.SaveChangesAsync();
        }
        public async Task<List<Entities.User>> FindUserByLoginOrUsername(string request)
        {
            var normalizedRequest = request.ToLower().Trim();
            var user = dataContext
                .Users
                .Include(u => u.Race)
                .Include(u => u.UserInterests)
                    .ThenInclude(ui => ui.Interest)
                .Where(u => u.Login.ToLower().Contains(normalizedRequest) ||
                       u.Nickname.ToLower().Contains(normalizedRequest))
                .ToListAsync();
            return await user;
        }
        public async Task<List<Entities.User>> GetUsersWhoLikedPostAsync(string postId)
        {
            var postGuid = Guid.Parse(postId);

            return await dataContext
                .PostLikes
                .AsNoTracking()
                .Where(l => l.PostId == postGuid)
                .Include(l => l.User)
                    .ThenInclude(u => u.Race)
                .Include(l => l.User)
                    .ThenInclude(u => u.UserInterests)
                        .ThenInclude(ui => ui.Interest)
                .Select(l => l.User)
                .ToListAsync();
        }
        public async Task<List<Entities.User>> GetUsersWhoLikedCommentAsync(string commentId)
        {
            var commentGuid = Guid.Parse(commentId);

            return await dataContext
                .CommentLikes
                .AsNoTracking()
                .Where(l => l.CommentId == commentGuid)
                .Include(l => l.User)
                    .ThenInclude(u => u.Race)
                .Include(l => l.User)
                    .ThenInclude(u => u.UserInterests)
                        .ThenInclude(ui => ui.Interest)
                .Select(l => l.User)
                .ToListAsync();
        }
        public async Task<List<Entities.User>> GetUsersWhoSavedPostAsync(string postId)
        {
            var postGuid = Guid.Parse(postId);

            return await dataContext
                .PostSaves
                .AsNoTracking()
                .Where(s => s.PostId == postGuid)
                .Include(s => s.User)
                    .ThenInclude(u => u.Race)
                .Include(s => s.User)
                    .ThenInclude(u => u.UserInterests)
                        .ThenInclude(ui => ui.Interest)
                .Select(s => s.User)
                .ToListAsync();
        }
        public async Task<List<Entities.User>> GetUsersWhoSharedPostAsync(string postId)
        {
            var postGuid = Guid.Parse(postId);

            return await dataContext
                .PostShares
                .AsNoTracking()
                .Where(s => s.PostId == postGuid)
                .Include(s => s.User)
                    .ThenInclude(u => u.Race)
                .Include(s => s.User)
                    .ThenInclude(u => u.UserInterests)
                        .ThenInclude(ui => ui.Interest)
                .Select(s => s.User)
                .ToListAsync();
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

        public async Task AddCommentAsync(Entities.Comment comment)
        {
            await dataContext.Comments.AddAsync(comment);
            await dataContext.SaveChangesAsync();
        }
        public async Task<Entities.Comment?> GetCommentByIdAsync(string id)
        {
            var comment = dataContext
                .Comments
                .FirstOrDefaultAsync(p => p.Id.ToString() == id);

            if (comment == null)
                throw new Exception($"Comment with id {id} not found");

            return await comment;

        }

        public async Task<bool> PostLikeExistsAsync(string userId, string postId)
        {
            return await dataContext
                .PostLikes
                .AnyAsync(l => l.UserId == Guid.Parse(userId) && l.PostId == Guid.Parse(postId));
        }
        public async Task AddPostLikeAsync(Entities.PostLike like)
        {
            await dataContext.PostLikes.AddAsync(like);
            await dataContext.SaveChangesAsync();
        }
        public async Task RemovePostLikeAsync(string userId, string postId)
        {
            var like = await dataContext.PostLikes
                .FirstOrDefaultAsync(l => l.UserId == Guid.Parse(userId) && l.PostId == Guid.Parse(postId));

            if (like is not null)
            {
                dataContext.PostLikes.Remove(like);
                await dataContext.SaveChangesAsync();
            }
        }

        public async Task<bool> PostShareExistsAsync(string userId, string postId)
        {
            return await dataContext
                .PostShares
                .AnyAsync(l => l.UserId == Guid.Parse(userId) && l.PostId == Guid.Parse(postId));
        }
        public async Task AddPostShareAsync(Entities.PostShare share)
        {
            await dataContext.PostShares.AddAsync(share);
            await dataContext.SaveChangesAsync();
        }
        public async Task RemovePostShareAsync(string userId, string postId)
        {
            var share = await dataContext.PostShares
                .FirstOrDefaultAsync(l => l.UserId == Guid.Parse(userId) && l.PostId == Guid.Parse(postId));

            if (share is not null)
            {
                dataContext.PostShares.Remove(share);
                await dataContext.SaveChangesAsync();
            }
        }

        public async Task<bool> PostSaveExistsAsync(string userId, string postId)
        {
            return await dataContext
                .PostSaves
                .AnyAsync(s => s.UserId == Guid.Parse(userId) && s.PostId == Guid.Parse(postId));
        }
        public async Task RemovePostSaveAsync(string userId, string postId)
        {
            var save = await dataContext.PostSaves
                .FirstOrDefaultAsync(s => s.UserId == Guid.Parse(userId) && s.PostId == Guid.Parse(postId));

            if (save is not null)
            {
                dataContext.PostSaves.Remove(save);
                await dataContext.SaveChangesAsync();
            }
        }
        public async Task AddPostSaveAsync(Entities.PostSave save)
        {
            await dataContext.PostSaves.AddAsync(save);
            await dataContext.SaveChangesAsync();
        }

        public async Task<bool> CommentLikeExistsAsync(string userId, string commentId)
        {
            return await dataContext
                .CommentLikes
                .AnyAsync(l => l.UserId == Guid.Parse(userId) && l.CommentId == Guid.Parse(commentId));
        }
        public async Task RemoveCommentLikeAsync(string userId, string commentId)
        {
            var like = await dataContext.CommentLikes
                .FirstOrDefaultAsync(l => l.UserId == Guid.Parse(userId) && l.CommentId == Guid.Parse(commentId));

            if (like is not null)
            {
                dataContext.CommentLikes.Remove(like);
                await dataContext.SaveChangesAsync();
            }
        }
        public async Task AddCommentLikeAsync(Entities.CommentLike like)
        {
            await dataContext.CommentLikes.AddAsync(like);
            await dataContext.SaveChangesAsync();
        }

    }
}