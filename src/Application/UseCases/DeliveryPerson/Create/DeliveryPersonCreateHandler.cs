using MediatR;
using Application.UseCases.Base;
using Domain.Repositories;
using Domain.Services;
using System.Net;

namespace Application.UseCases.DeliveryPerson.Create
{
    public class DeliveryPersonCreateHandler : IRequestHandler<DeliveryPersonCreateRequest, CustomResponse>, IDisposable
    {
        private readonly IDeliveryPersonRepository _deliveryPersonRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;

        public DeliveryPersonCreateHandler(IDeliveryPersonRepository deliveryPersonRepository, IUnitOfWork unitOfWork, IFileService fileService)
        {
            _deliveryPersonRepository = deliveryPersonRepository;
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task<CustomResponse> Handle(DeliveryPersonCreateRequest request, CancellationToken cancellationToken)
        {
            if (await _deliveryPersonRepository.GetAsync(x => x.Id == request.Id, cancellationToken) is not null)
            {
                return new CustomResponse
                {
                    Success = false,
                    HttpStatusCode = HttpStatusCode.Conflict,
                    Response = CustomResponseMessage.FromMessage("Esse identificador já está em uso."),
                };
            }

            if (await _deliveryPersonRepository.GetAsync(x => x.DriverLicenseNumber == request.DriverLicenseNumber, cancellationToken) is not null)
            {
                return new CustomResponse
                {
                    Success = false,
                    HttpStatusCode = HttpStatusCode.Conflict,
                    Response = CustomResponseMessage.FromMessage("Esse número da CNH já está em uso."),
                };
            }

            if (await _deliveryPersonRepository.GetAsync(x => x.Document == request.Document, cancellationToken) is not null)
            {
                return new CustomResponse
                {
                    Success = false,
                    HttpStatusCode = HttpStatusCode.Conflict,
                    Response = CustomResponseMessage.FromMessage("Esse CNPJ já está em uso."),
                };
            }

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

            var entity = request.ToEntity(driverLicenseImage);
            await _deliveryPersonRepository.CreateAsync(entity);
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
            _deliveryPersonRepository?.Dispose();
            _unitOfWork?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
