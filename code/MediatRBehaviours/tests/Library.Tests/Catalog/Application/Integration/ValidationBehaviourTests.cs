using FluentValidation;
using Library.Catalog.Application.Author;
using Library.Catalog.Application.Behaviours;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Tests.Catalog.Application.Integration;

[TestFixture]
public class ValidationBehaviourTests : IntegrationTestsBase
{
    // These are always initialized in SetupMockLogger, initialized here to avoid nullability warnings
    private ServiceProvider _serviceProvider = null!;
    private IMediator _mediator = null!;

    [SetUp]
    public void SetUpServiceProvider()
    {
        Services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehaviour<,>));
        Services.AddTransient<IValidator<Create.Command>, Create.CommandRequiredFieldsValidator>();
        Services.AddTransient<IValidator<Create.Command>, Create.CommandBirthdateValidator>();
        _serviceProvider = Services.BuildServiceProvider();
        _mediator = _serviceProvider.GetRequiredService<IMediator>();
    }

    [Test]
    public void WhenCallingAPipeLineReturningResults_WithAInvalidCommand_ShouldThrow()
    {
        // Arrange
        // Intentionally passing blank title and invalid dates to trigger a failed result
        var command = new Create.Command("", "Biography",
            DateTimeOffset.Now.AddYears(30), DateTimeOffset.Now);

        // Act & Assert
        Assert.ThrowsAsync<ValidationException>(async () =>
        {
            var result = await _mediator.Send(command);
        });
    }

    [TearDown]
    public void TearDown()
    {
        _serviceProvider.Dispose();
    }
}