
using System.Globalization;
using MongoDB.Bson.Serialization.Attributes;

namespace Sytycc_Service.Domain;
public class Course
{
    [BsonId]
    public string Reference { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string[] CourseOutLine { get; set; }
    public string Level { get; set; }
    public string TrainingType { get; set; }
    public string FacilitatorReference { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public string Duration { get;  set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public string TimeZone { get; set; }
    public string Currency { get; set; }
    public int Price { get; set; }
    public int Discount { get; set; }
    public string DiscountEndDate { get; set; }

    public Course(CreateCourseDto course)
    {
        Reference = Guid.NewGuid().ToString();
        Title = course.Title;
        Description = course.Description;
        CourseOutLine = course.CourseOutLine;
        Level = course.Level;
        TrainingType = course.TrainingType;
        FacilitatorReference = course.FacilitatorReference;
        Currency = course.Currency;
        Price = course.Price;
        Discount = course.Discount;
        StartDate = course.StartDate;
        EndDate = course.EndDate;
        StartTime = course.StartTime;
        EndTime = course.EndTime;
        TimeZone = course.TimeZone;
        DiscountEndDate = course.DiscountEndDate;
    }

    public Course(UpdateCourseDto course)
    {
        Title = course.Title;
        Description = course.Description;
        CourseOutLine = course.CourseOutLine;
        Level = course.Level;
        TrainingType = course.TrainingType;
        FacilitatorReference = course.FacilitatorReference;
        Currency = course.Currency;
        Price = course.Price;
        Discount = course.Discount;
        StartDate = course.StartDate;
        EndDate = course.EndDate;
        Duration = CalculateDuration();
        StartTime = course.StartTime;
        EndTime = course.EndTime;
        TimeZone = course.TimeZone;
        DiscountEndDate = course.DiscountEndDate;
        
    }

    private string CalculateDuration()
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

    public Course() { }
}

