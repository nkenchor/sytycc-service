using System.Globalization;

namespace Sytycc_Service.Domain;
public class CourseDto
{ 
    public Course Course { get; set; }
    public Facilitator Facilitator { get; set; }
    public int FinalPrice { get; set; }

    public CourseDto(Course course, Facilitator facilitator)
    {
        Course = course;
        Facilitator = facilitator;

        // Parse DiscountEndDate into a DateTime object, and specify it's in UTC.
        // Assuming "dd-MM-yyyy" format, adjust if necessary.
        bool isDateParsed = DateTime.TryParseExact(course.DiscountEndDate, "dd-MM-yyyy",
                                                  CultureInfo.InvariantCulture,
                                                  DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                                                  out DateTime discountEndDate);
        if (!isDateParsed)
        {
            // Handle the error - maybe log it or set a default value.
            // Here, we're just using the base price.
            FinalPrice = course.Price;
            return;
        }

        // Adjust discountEndDate to the end of the day
        discountEndDate = discountEndDate.Date.AddTicks(TimeSpan.TicksPerDay - 1);

        if (course.Discount > 0 && DateTime.UtcNow <= discountEndDate)
        {
            // Apply the discount if it exists and the current date hasn't exceeded the discount end date
            FinalPrice = course.Price - course.Discount;
        }
        else
        {
            // No discount to apply
            FinalPrice = course.Price;
        }
    }
}
