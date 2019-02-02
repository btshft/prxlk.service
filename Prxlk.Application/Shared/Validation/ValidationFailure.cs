namespace Prxlk.Application.Shared.Validation
{
    public class ValidationFailure
    {
        public string ErrorMessage { get; }
        public string PropertyName { get; }
        public object AttemptedValue { get; }

        public ValidationFailure(string errorMessage, string propertyName = null, object attemptedValue = null)
        {
            ErrorMessage = errorMessage;
            PropertyName = propertyName;
            AttemptedValue = attemptedValue;
        }

        public override string ToString() => ErrorMessage;
    }
}