using Application.UseCases.Base;
using Application.UseCases.DeliveryPerson.Update;
using AutoBogus;
using Domain.Repositories;
using Domain.Services;
using FluentValidation.TestHelper;
using Moq;
using Moq.AutoMock;

namespace Tests.UnitTests.UseCases.DeliveryPerson.Update
{
    public class DeliveryPersonUpdateTests
    {
        private const string _driverLicenseImage = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAUA\r\nAAAAFCAIAAAoXIZ2AAAAEElEQVR42mP8//8";

        [Fact]
        public async void DeliveryPersonUpdate_Valid_MustSuccess()
        {
            //Arrange
            var autoMocker = new AutoMocker();
            var handler = autoMocker.CreateInstance<DeliveryPersonUpdateHandler>();

            var request = new AutoFaker<DeliveryPersonUpdateRequest>()
              .RuleFor(a => a.DriverLicenseImage, b => _driverLicenseImage)
              .Generate();

            var deliveryPerson = new AutoFaker<Domain.Entities.DeliveryPerson>()
              .RuleFor(a => a.Id, b => request.Id)
              .Generate();

            autoMocker
                .GetMock<IDeliveryPersonRepository>()
                .Setup(s =>
                    s.GetAsync(
                        x => x.Id == request.Id,
                        It.IsAny<CancellationToken>()
                    )
                ).ReturnsAsync(deliveryPerson);

            autoMocker
                .GetMock<IFileService>()
                .Setup(s =>
                    s.SaveBase64Image(It.IsAny<string>(), It.IsAny<string>())
                ).ReturnsAsync(_driverLicenseImage);

            autoMocker
                .GetMock<IUnitOfWork>()
                .Setup(s =>
                    s.Commit(CancellationToken.None)
                ).ReturnsAsync(true);

            //Act
            var response = await handler.Handle(request, CancellationToken.None);

            //Assert
            Assert.True(response.Success);
        }



        [Fact]
        public async void DeliveryPersonUpdate_IdNotFound_MustFail()
        {
            //Arrange
            var autoMocker = new AutoMocker();
            var handler = autoMocker.CreateInstance<DeliveryPersonUpdateHandler>();

            var request = new AutoFaker<DeliveryPersonUpdateRequest>()
              .RuleFor(a => a.DriverLicenseImage, b => _driverLicenseImage)
              .Generate();

            //Act
            var response = await handler.Handle(request, CancellationToken.None);

            //Assert
            var customResponseMessages = response.Response as List<CustomResponseMessage>;
            Assert.False(response.Success);
            Assert.Contains(customResponseMessages, c => c.Message == "Identificador não localizado.");
        }

        [Fact]
        public async void DeliveryPersonUpdate_UploadDriverLicenseImageError_MustFail()
        {
            //Arrange
            var autoMocker = new AutoMocker();
            var handler = autoMocker.CreateInstance<DeliveryPersonUpdateHandler>();

            var request = new AutoFaker<DeliveryPersonUpdateRequest>()
              .RuleFor(a => a.DriverLicenseImage, b => _driverLicenseImage)
              .Generate();

            var deliveryPerson = new AutoFaker<Domain.Entities.DeliveryPerson>()
              .RuleFor(a => a.Id, b => request.Id)
              .Generate();

            autoMocker
                .GetMock<IDeliveryPersonRepository>()
                .Setup(s =>
                    s.GetAsync(
                        x => x.Id == request.Id,
                        It.IsAny<CancellationToken>()
                    )
                ).ReturnsAsync(deliveryPerson);

            //Act
            var response = await handler.Handle(request, CancellationToken.None);

            //Assert
            var customResponseMessages = response.Response as List<CustomResponseMessage>;
            Assert.False(response.Success);
            Assert.Contains(customResponseMessages, c => c.Message == "Houve um erro ao realizar upload da imagem.");
        }

        [Fact]
        public async void DeliveryPersonUpdate_CommitError_MustFail()
        {
            //Arrange
            var autoMocker = new AutoMocker();
            var handler = autoMocker.CreateInstance<DeliveryPersonUpdateHandler>();

            var request = new AutoFaker<DeliveryPersonUpdateRequest>()
              .RuleFor(a => a.DriverLicenseImage, b => _driverLicenseImage)
              .Generate();

            var deliveryPerson = new AutoFaker<Domain.Entities.DeliveryPerson>()
              .RuleFor(a => a.Id, b => request.Id)
              .Generate();

            autoMocker
                .GetMock<IDeliveryPersonRepository>()
                .Setup(s =>
                    s.GetAsync(
                        x => x.Id == request.Id,
                        It.IsAny<CancellationToken>()
                    )
                ).ReturnsAsync(deliveryPerson);

            autoMocker
                .GetMock<IFileService>()
                .Setup(s =>
                    s.SaveBase64Image(It.IsAny<string>(), It.IsAny<string>())
                ).ReturnsAsync(_driverLicenseImage);

            //Act
            var response = await handler.Handle(request, CancellationToken.None);

            //Assert
            var customResponseMessages = response.Response as List<CustomResponseMessage>;
            Assert.False(response.Success);
            Assert.Contains(customResponseMessages, c => c.Message == "Houve um problema ao persistir suas informações. Por favor, tente novamente em instantes.");
        }

        [Fact]
        public async void DeliveryPersonUpdate_RequestValid_MustSuccess()
        {
            //Arrange
            var validator = new DeliveryPersonUpdateValidator();

            var request = new AutoFaker<DeliveryPersonUpdateRequest>()
              .RuleFor(a => a.DriverLicenseImage, b => _driverLicenseImage)
              .Generate();

            //Act
            var response = await validator.TestValidateAsync(request);

            //Assert
            Assert.True(response.IsValid);
        }

        [Fact]
        public async void DeliveryPersonUpdate_DriverLicenseImageInvalid_MustFail()
        {
            //Arrange
            var validator = new DeliveryPersonUpdateValidator();

            var request = new AutoFaker<DeliveryPersonUpdateRequest>()
              .RuleFor(a => a.DriverLicenseImage, b => b.Random.String2(1))
              .Generate();

            //Act
            var response = await validator.TestValidateAsync(request);

            //Assert
            Assert.False(response.IsValid);
            Assert.Contains(response.Errors, c => c.ErrorMessage == $"Imagem da CNH inválida.");
        }
    }
}
