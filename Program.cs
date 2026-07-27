
using ASP_PV411.Services.Hash;
using ASP_PV411.Services.Kdf;
using ASP_PV411.Services.Random;
using ASP_PV411.Services.Salt;
using Microsoft.EntityFrameworkCore;
using SocialMediaBackend.Data;
using SocialMediaBackend.Data.Entities;
using SocialMediaBackend.Hubs;
using SocialMediaBackend.Middleware;
using SocialMediaBackend.Services.AppService;
using SocialMediaBackend.Services.BlobStorage;

namespace SocialMediaBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRandom();
            builder.Services.AddHash();
            builder.Services.AddKdf();
            builder.Services.AddSalt();
            builder.Services.AddScoped<IAppService, AppService>();
            builder.Services.AddSingleton<AvatarStorageService>();
            builder.Services.AddSingleton<PostImageStorageService>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSignalR();

            // Session
            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(3);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.IOTimeout = TimeSpan.FromDays(3);
            });

            string connectionString = builder.Configuration.GetConnectionString("SocialMediaDatabase") ?? throw new FileNotFoundException("Connection String Configuration: key not found: SocialMediaDatabase");
            builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString, options =>
            {
                options.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null
                    );
            }));
            builder.Services.AddCors(options =>
                options.AddDefaultPolicy(policy =>
                    policy.WithOrigins("http://localhost:5173", "https://zealous-coast-02bfa1803.7.azurestaticapps.net")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                ));
            builder.Services.AddScoped<DataAccessor>();

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
                if (dbContext.Database.IsRelational())
                {
                    dbContext.Database.Migrate();

                    if (!dbContext.UserRoles.Any())
                        dbContext.UserRoles.AddRange(SeedData.UserRoles());

                    if (!dbContext.Races.Any())
                        dbContext.Races.AddRange(SeedData.Races());

                    if (!dbContext.Users.Any())
                        dbContext.Users.AddRange(SeedData.Users(scope.ServiceProvider.GetRequiredService<IKdfService>()));

                    if (!dbContext.Posts.Any())
                        dbContext.Posts.AddRange(SeedData.Posts());

                    if (!dbContext.Comments.Any())
                        dbContext.Comments.AddRange(SeedData.Comments());

                    if (!dbContext.Interests.Any())
                        dbContext.Interests.AddRange(SeedData.Interests());

                    if (!dbContext.UsersInterests.Any())
                        dbContext.UsersInterests.AddRange(SeedData.UsersInterests());

                    if (!dbContext.PostsInterests.Any())
                        dbContext.PostsInterests.AddRange(SeedData.PostsInterests());

                    dbContext.SaveChanges();

                    var dbUsers = dbContext.Users.ToArray();
                    var dbPosts = dbContext.Posts.ToArray();

                    if (dbUsers.Length > 0 &&  dbPosts.Length > 0)
                    {
                        if (!dbContext.Set<PostLike>().Any())
                        {
                            var generatedLikes = SeedData.PostsLikes(dbUsers, dbPosts);
                            dbContext.Set<PostLike>().AddRange(generatedLikes);

                            dbContext.SaveChanges();
                        }
                    }
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }
            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseCors();

            app.UseSession();
            app.UseAuthSession();
            app.MapHub<ChatHub>("/hubs/chat");
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { _ = endpoints.MapControllers(); });

            app.MapControllers();

            app.Run();
        }
    }
}
