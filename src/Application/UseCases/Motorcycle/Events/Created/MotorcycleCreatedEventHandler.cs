using Application.UseCases.Base;
using Domain.Repositories;
using MediatR;
using System.Net;

namespace Application.UseCases.Motorcycle.Events.Created
{
    public class MotorcycleCreatedEventHandler : IRequestHandler<MotorcycleCreatedEventRequest, CustomResponse>, IDisposable
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MotorcycleCreatedEventHandler(INotificationRepository notificationRepository, IUnitOfWork unitOfWork)
        {
            _notificationRepository = notificationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CustomResponse> Handle(MotorcycleCreatedEventRequest request, CancellationToken cancellationToken)
        {
            var entity = request.ToEntity();
            await _notificationRepository.CreateAsync(entity);
            if (!await _unitOfWork.Commit(cancellationToken))
            {
                return new CustomResponse
                {
                    Success = false,
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Response = CustomResponseMessage.FromMessage("Houve um problema ao persistir suas informações. Por favor, tente novamente em instantes."),
                };
            }

            return new CustomResponse
            {
                Success = true,
                HttpStatusCode = HttpStatusCode.Created
            };
        }

        public void Dispose()
        {
            _notificationRepository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
