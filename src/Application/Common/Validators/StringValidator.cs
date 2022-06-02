using Application.Common.Exceptions;

namespace Application.Common.Validators;

public class StringValidator
{
    public string Property { get; }
    public string PropertyName { get; }
    
    public StringValidator(string property, string propertyName)
    {
        Property = property;
        PropertyName = propertyName;
    }
    public StringValidator NotEmpty()
    {
        if (string.IsNullOrEmpty(Property))
        {
            throw new BadRequestException($"{PropertyName} cannot be empty");
        }

        return this;
    }

    public StringValidator MaxLength(int maxLength)
    {
        if (Property.Length > maxLength)
        {
            throw new BadRequestException(
                $"{PropertyName} must be less than {maxLength} characters. {Property.Length} characters entered.");
        }
        
        return this;
    }
}