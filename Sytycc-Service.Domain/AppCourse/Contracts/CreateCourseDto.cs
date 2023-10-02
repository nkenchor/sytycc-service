
using System.ComponentModel.DataAnnotations;

namespace Sytycc_Service.Domain;
public class CreateCourseDto
{
   [Required(ErrorMessage = "Title is required.")]
   [StringLength(200, MinimumLength = 5, ErrorMessage = "Title should be between 5 and 200 characters.")]
   public string Title { get; set; }

   [Required(ErrorMessage = "Description is required.")]
   [StringLength(2000, MinimumLength = 10, ErrorMessage = "Description should be between 10 and 2000 characters.")]
   public string Description { get; set; }

   [Required(ErrorMessage = "Course outline is required.")]
   public string[] CourseOutLine { get; set; }

   [Required(ErrorMessage = "Level is required.")]
   public string Level { get; set; }

   [Required(ErrorMessage = "Training type is required.")]
   public string TrainingType { get; set; }

   [Required(ErrorMessage = "Facilitator reference is required.")]
   public string FacilitatorReference { get; set; }

   [Required(ErrorMessage = "Start date is required.")]
   [RegularExpression(@"^([0-2]?[1-9]|3[0-1])-([0]?[1-9]|1[0-2])-(20[0-2][0-9]|2030)$", ErrorMessage = "Start date must be in the format DD-MM-YYYY and year should not be more than 2030.")]
   public string StartDate { get; set; }

   [Required(ErrorMessage = "End date is required.")]
   [RegularExpression(@"^([0-2]?[1-9]|3[0-1])-([0]?[1-9]|1[0-2])-(20[0-2][0-9]|2030)$", ErrorMessage = "End date must be in the format DD-MM-YYYY and year should not be more than 2030.")]
   public string EndDate { get; set; }


   [Required(ErrorMessage = "Start time is required.")]
   [RegularExpression(@"^(1[0-2]|0?[1-9]):[0-5][0-9] (AM|PM)$", ErrorMessage = "Start time must be in the format h:mmAM/PM.")]
   public string StartTime { get; set; }

   [Required(ErrorMessage = "End time is required.")]
   [RegularExpression(@"^(1[0-2]|0?[1-9]):[0-5][0-9] (AM|PM)$", ErrorMessage = "End time must be in the format h:mmAM/PM.")]
   public string EndTime { get; set; }


   [Required(ErrorMessage = "Time zone is required.")]
   public string TimeZone { get; set; }

   [Required(ErrorMessage = "Currency is required.")]
   [RegularExpression(@"^GBP$", ErrorMessage = "Currency must be GBP.")]
   public string Currency { get; set; }

   [Required(ErrorMessage = "Price is required.")]
   [Range(1, int.MaxValue, ErrorMessage = "Price must be greater than 0.")]
   public int Price { get; set; }

   [Range(0, int.MaxValue, ErrorMessage = "Discount must not be negative.")]
   public int Discount { get; set; }

      [Required(ErrorMessage = "Discount end date is required.")]
   [RegularExpression(@"^([0-2]?[1-9]|3[0-1])-([0]?[1-9]|1[0-2])-(20[0-2][0-9]|2030)$", ErrorMessage = "Discount end date must be in the format DD-MM-YYYY and year should not be more than 2030.")]
   public string DiscountEndDate { get; set; }



}

