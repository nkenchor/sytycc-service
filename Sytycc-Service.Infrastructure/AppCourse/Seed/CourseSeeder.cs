
using System.Globalization;
using Sytycc_Service.Domain;


namespace Sytycc_Service.Api;
public class CourseSeeder
{
    private readonly ICourseRepository _courseRepository;

    public CourseSeeder(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }
    public async Task SeedCourseAsync()
    {
        var courses = new List<Course>
        {
            new Course
            {
                Reference = Guid.NewGuid().ToString(),
                Title = "RSM, RPO, and ICP-APM Certification",
                Description = "3-in-1 combo Training for Examination and Certification to become a Registered Scrum Master, Product Owner, and Agile Project & Delivery Management",
                CourseOutLine =  new[] 
                    {
                        "Introduction to Scrum and Agile",
                        "Roles and Responsibilities",
                        "Scrum Events",
                        "Product Backlog Management",
                        "Sprint Planning and Execution",
                        "Scrum Artifacts",
                        "Scrum in Large Teams",
        
                    },
                Level = "Intermediate",
                TrainingType = "Instructor-Led",
                FacilitatorReference = "b1f394c7-717f-4b33-b1ef-f73e970005cd",
                Currency = "GBP",
                Price = 1200, // Assuming that RegularCost corresponds to Price in the new structure.
                Discount = 300, // Price difference between RegularCost and EarlyBirdCost.
                StartDate = "18-09-" + DateTime.Now.Year,
                EndDate = "13-10-" + DateTime.Now.Year,
                Duration = CalculateDuration("18-09-" + DateTime.Now.Year, "13-10-" + DateTime.Now.Year),
                StartTime = "10:00 AM",
                EndTime = "04:00 PM",
                TimeZone = "BST",
                DiscountEndDate = "30-08-" + DateTime.Now.Year,
            },
        };

        foreach (var course in courses)
        {
            await _courseRepository.CreateCourse(course);
        }
    }
    private string CalculateDuration(string StartDate, string EndDate)
    {
        // Convert StartDate and EndDate strings to DateTime
        DateTime start = DateTime.ParseExact(StartDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
        DateTime end = DateTime.ParseExact(EndDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);

        int workingDays = 0;
        while (start <= end)
        {
            if (start.DayOfWeek != DayOfWeek.Saturday && start.DayOfWeek != DayOfWeek.Sunday)
            {
                workingDays++;
            }
            start = start.AddDays(1);
        }

        
        return  $"{workingDays} days"; // e.g., "5 days"
    }

}


