
using ASP_PV411.Services.Hash;
using ASP_PV411.Services.Kdf;
using ASP_PV411.Services.Random;
using ASP_PV411.Services.Salt;
using Microsoft.EntityFrameworkCore;
using SocialMediaBackend.Data;
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
            builder.Services.AddSingleton<IBlobStorageService, BlobStorageService>();

            // Session
            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(15);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            string connectionString = builder.Configuration.GetConnectionString("SocialMediaDb") ?? throw new FileNotFoundException("Connection String Configuration: key not found: SocialMediaDb");
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
                    policy.WithOrigins("http://localhost:5173")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
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
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            app.UseCors();

            app.UseAuthorization();
            app.UseSession();

            app.MapControllers();

            app.Run();
        }
    }
}
