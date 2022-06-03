using Application.Common.Exceptions;
using Application.Common.Validators;
using NUnit.Framework;

namespace ApplicationTests.Validators;

public class StringValidatorTests
{
    [Test]
    [TestCase("Title", "Title")]
    [TestCase("Description", "Description")]
    public void ValidatingNotEmptyStringShouldNotThrowException(string property, string propertyName)
    {
        // Arrange
        var stringValidator = new StringValidator(property, propertyName);

        // Act
        // Assert
        Assert.DoesNotThrow(() => stringValidator.NotEmpty());
    }
    
    [Test]
    [TestCase("", "Title")]
    [TestCase(null, "Description")]
    public void ValidatingNotEmptyStringShouldThrowException(string property, string propertyName)
    {
        // Arrange
        var stringValidator = new StringValidator(property, propertyName);

        // Act
        // Assert
        Assert.Throws<BadRequestException>(() => stringValidator.NotEmpty());
    }
    
    [Test]
    [TestCase("Title", "Title", 50)]
    [TestCase("Description", "Description", 100)]
    public void ValidatingMaxLengthStringShouldNotThrowException(string property, string propertyName, int maxLength)
    {
        // Arrange
        var stringValidator = new StringValidator(property, propertyName);

        // Act
        // Assert
        Assert.DoesNotThrow(() => stringValidator.MaxLength(maxLength));
    }
    
    [Test]
    [TestCase("TitleTitleTitleTitleTitleTitleTitleTitleTitleTitleTitle", "Title", 50)]
    [TestCase("DescriptionDescriptionDescriptionDescriptionDescription" +
              "DescriptionDescriptionDescriptionDescriptionDescriptionDescription", "Description", 100)]
    public void ValidatingMaxLengthStringShouldThrowException(string property, string propertyName, int maxLength)
    {
        // Arrange
        var stringValidator = new StringValidator(property, propertyName);

        // Act
        // Assert
        Assert.Throws<BadRequestException>(() => stringValidator.MaxLength(maxLength));
    }
}