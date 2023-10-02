
using Sytycc_Service.Domain;

namespace Sytycc_Service.Api.Extensions;

public static class DatabaseSeedExtensions
{
   public static IHost SeedDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var facilitatorRepo = scope.ServiceProvider.GetRequiredService<IFacilitatorRepository>();
                var facilitators = facilitatorRepo.GetFacilitatorList(1).Result;
                if (facilitators.Count == 0)
                {
                    var seeder = scope.ServiceProvider.GetRequiredService<FacilitatorSeeder>();
                    seeder.SeedFacilitatorAsync().Wait();
                }

                var courseRepo = scope.ServiceProvider.GetRequiredService<ICourseRepository>();
                var courses = courseRepo.GetCourseList(1).Result;
                if (courses.Count == 0)
                {
                    var seeder = scope.ServiceProvider.GetRequiredService<CourseSeeder>();
                    seeder.SeedCourseAsync().Wait();
                }

                var userRepo = scope.ServiceProvider.GetRequiredService<IUserRepository>();
                var users = userRepo.GetUserList(1).Result;
                if (users.Count == 0)
                {
                    var seeder = scope.ServiceProvider.GetRequiredService<UserSeeder>();
                    seeder.SeedUserAsync().Wait();
                }
            }
            return host;
        }
}