using FluentValidation;
using MediatR;
using Application.UseCases.Base;
using System.Net;

namespace Application.Middlewares
{
    public class ValidationMiddleware<TRequest, TResponse> : IPipelineBehavior<TRequest, CustomResponse>
    {
        private readonly IValidator<TRequest> _validator;

        public ValidationMiddleware(IValidator<TRequest> validator = null)
        {
            _validator = validator;
        }

        public async Task<CustomResponse> Handle(TRequest request, RequestHandlerDelegate<CustomResponse> next, CancellationToken cancellationToken)
        {
            if (_validator is null)
                return await next();

            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return new CustomResponse
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Success = false,
                    Response = validationResult.Errors.Select(e => new CustomResponseMessage
                    {
                        Message = e.ErrorMessage
                    })
                };
            }

            return await next();
        }
    }
}
