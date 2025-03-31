
using System.ComponentModel.DataAnnotations;

namespace FCSP.DTOs.Rating
{
    public class BadWordValidationAttribute : ValidationAttribute
    {
        private static readonly HashSet<string> BadWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "badword1", "badword2", "fuck", "shit" // Thêm danh sách từ cấm
    };

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            string comment = value.ToString();
            if (string.IsNullOrWhiteSpace(comment))
                return ValidationResult.Success;

            var words = comment.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            foreach (var word in words)
            {
                if (BadWords.Contains(word))
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}