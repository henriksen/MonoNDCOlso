using FluentResults;
using Library.Catalog.Application.Author;
using Library.Catalog.Application.Behaviours;
using Library.Catalog.Contracts;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Library.Tests.Catalog.Application.Integration;

[TestFixture]
public class LoggingBehaviourTests : IntegrationTestsBase
{
    // These are always initialized in SetupMockLogger, initialized here to avoid nullability warnings
    private ServiceProvider _serviceProvider = null!;
    private IMediator _mediator = null!;

    private readonly ILogger<LoggingBehaviour<Create.Command, Result<AuthorId>>> _logger = Substitute.For<ILogger<LoggingBehaviour<Create.Command, Result<AuthorId>>>>();

    [SetUp]
    public void SetUpMockLogger()
    {
        // We need a mock logger to verify that the logging is called
        // We're using NSubstitute to create the mock. The parameters have to match exactly what
        // the LoggingBehaviour expects in this particular case.
        Services.AddSingleton(_logger);

        Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));

        _serviceProvider = Services.BuildServiceProvider();
        _mediator = _serviceProvider.GetRequiredService<IMediator>();
    }


    [Test]
    public async Task WhenCallingAPipeLine_ShouldLogStartAndFinish()
    {
        // Arrange
        var command = new Create.Command("Test Author", "Biography", DateTimeOffset.Now.AddYears(-30), null);

        // Act
        await _mediator.Send(command);

        // Assert
        _logger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(o => o.ToString()!.Contains("Handling Library.Catalog.Application.Author.Create+Command")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());

        _logger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(o => o.ToString()!.Contains("Handled Library.Catalog.Application.Author.Create+Command")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    [Test]
    public async Task WhenCallingAPipeLine_WithAFailedResult_ShouldLogWarning()
    {
        // Arrange
        // Intentionally passing invalid dates to trigger a failed result
        var command = new Create.Command("Test Author", "Biography",
            DateTimeOffset.Now.AddYears(30), DateTimeOffset.Now);

        // Act
        var result = await _mediator.Send(command);

        // Assert
        Assert.That(result.IsFailed, Is.True);
        _logger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Any<object>(),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    [TearDown]
    public void TearDown()
    {
        _serviceProvider.Dispose();
    }
}