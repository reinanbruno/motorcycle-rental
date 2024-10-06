using MediatR;
using Application.UseCases.Base;
using Domain.Repositories;
using Domain.Services;
using System.Net;

namespace Application.UseCases.DeliveryPerson.Update
{
    public class DeliveryPersonUpdateHandler : IRequestHandler<DeliveryPersonUpdateRequest, CustomResponse>
    {
        private readonly IDeliveryPersonRepository _deliveryPersonRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;

        public DeliveryPersonUpdateHandler(IDeliveryPersonRepository deliveryPersonRepository, IUnitOfWork unitOfWork, IFileService fileService)
        {
            _deliveryPersonRepository = deliveryPersonRepository;
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task<CustomResponse> Handle(DeliveryPersonUpdateRequest request, CancellationToken cancellationToken)
        {
            Domain.Entities.DeliveryPerson entity = await _deliveryPersonRepository.GetAsync(x => x.Id == request.Id, cancellationToken);
            if (entity is null)
            {
                return new CustomResponse
                {
                    Success = false,
                    HttpStatusCode = HttpStatusCode.NotFound,
                    Response = CustomResponseMessage.FromMessage("Identificador não localizado."),
                };
            }

            _fileService.DeleteBase64Image(entity.DriverLicenseImage);

            var driverLicenseImage = await _fileService.SaveBase64Image(request.DriverLicenseImage, $"cnh_{request.Id}");
            if (driverLicenseImage is null)
            {
                return new CustomResponse
                {
                    Success = false,
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Response = CustomResponseMessage.FromMessage("Houve um erro ao realizar upload da imagem."),
                };
            }

            entity.DriverLicenseImage = driverLicenseImage;
            _deliveryPersonRepository.Update(entity);
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
                HttpStatusCode = HttpStatusCode.OK
            };
        }

        public void Dispose()
        {
            _deliveryPersonRepository?.Dispose();
            _unitOfWork?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
