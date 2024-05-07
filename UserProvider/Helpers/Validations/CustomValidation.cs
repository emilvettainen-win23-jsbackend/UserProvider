using System.ComponentModel.DataAnnotations;

namespace UserProvider.Helpers.Validations;

public class CustomValidation
{
    public class ValidationModel<T>
    {
        public bool IsValid { get; set; }
        public T? Value { get; set; }

        public IEnumerable<ValidationResult> ValidationResults { get; set; } = [];
    }

    public static ValidationModel<T> ValidateModel<T>(T model)
    {
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(model!);
        var isValid = Validator.TryValidateObject(model!, context, validationResults, true);

        return new ValidationModel<T>
        {
            IsValid = isValid,
            Value = model,
            ValidationResults = validationResults
        };
    }
}
