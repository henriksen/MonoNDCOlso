using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Library.Catalog.Application.Behaviours
{
    public class LoggingBehaviour<TRequest, TResponse>(
        ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestType = typeof(TRequest);
            var poorExcuseForACorrelationId = Guid.NewGuid();
            logger.LogInformation("Handling {Request} ({Guid})", requestType.FullName, poorExcuseForACorrelationId);
            var response = await next();

            if (response is IResultBase { IsFailed: true } result)
            {
                foreach (var error in result.Errors)
                {
                    logger.LogError("Handler {Request} failed with error: {Error} ({Guid})", requestType.FullName, error.Message, poorExcuseForACorrelationId);
                }
            }

            logger.LogInformation("Handled {Request} ({Guid})", requestType.FullName, poorExcuseForACorrelationId);

            return response;
        }
    }
}