using Application.UseCases.Base;
using Application.UseCases.DeliveryPerson.Create;
using AutoBogus;
using Bogus.Extensions.Brazil;
using Domain.Enums;
using Domain.Repositories;
using Domain.Services;
using FluentValidation.TestHelper;
using Moq;
using Moq.AutoMock;
using System.Linq.Expressions;

namespace Tests.UnitTests.UseCases.DeliveryPerson.Create
{
    public class DeliveryPersonCreateTests
    {
        private const string _driverLicenseImage = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAUA\r\nAAAAFCAIAAAoXIZ2AAAAEElEQVR42mP8//8";

        [Fact]
        public async void DeliveryPersonCreate_Valid_MustSuccess()
        {
            //Arrange
            var autoMocker = new AutoMocker();
            var handler = autoMocker.CreateInstance<DeliveryPersonCreateHandler>();

            var request = new AutoFaker<DeliveryPersonCreateRequest>()
              .RuleFor(a => a.Name, b => b.Person.FirstName)
              .RuleFor(a => a.DriverLicenseType, b => DriverLicenseType.A)
              .RuleFor(a => a.DriverLicenseNumber, b => b.Random.String2(11))
              .RuleFor(a => a.DriverLicenseImage, b => _driverLicenseImage)
              .RuleFor(a => a.Document, b => b.Company.Cnpj(false))
              .Generate();

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
        public async void DeliveryPersonCreate_IdExists_MustFail()
        {
            //Arrange
            var autoMocker = new AutoMocker();
            var handler = autoMocker.CreateInstance<DeliveryPersonCreateHandler>();

            var request = new AutoFaker<DeliveryPersonCreateRequest>()
              .RuleFor(a => a.Name, b => b.Person.FirstName)
              .RuleFor(a => a.DriverLicenseType, b => DriverLicenseType.A)
              .RuleFor(a => a.DriverLicenseNumber, b => b.Random.String2(11))
              .RuleFor(a => a.DriverLicenseImage, b => _driverLicenseImage)
              .RuleFor(a => a.Document, b => b.Company.Cnpj(false))
              .Generate();

            var deliveryPerson = new AutoFaker<Domain.Entities.DeliveryPerson>()
              .RuleFor(a => a.Id, b => request.Id)
              .Generate();

            autoMocker
                .GetMock<IDeliveryPersonRepository>()
                .Setup(s =>
                    s.GetAsync(
                        It.IsAny<Expression<Func<Domain.Entities.DeliveryPerson, bool>>>(),
                        It.IsAny<CancellationToken>()
                    )
                ).ReturnsAsync(deliveryPerson);

            //Act
            var response = await handler.Handle(request, CancellationToken.None);

            //Assert
            var customResponseMessages = response.Response as List<CustomResponseMessage>;
            Assert.False(response.Success);
            Assert.Contains(customResponseMessages, c => c.Message == "Esse identificador já está em uso.");
        }

        [Fact]
        public async void DeliveryPersonCreate_DriverLicenseNumberExists_MustFail()
        {
            //Arrange
            var autoMocker = new AutoMocker();
            var handler = autoMocker.CreateInstance<DeliveryPersonCreateHandler>();

            var request = new AutoFaker<DeliveryPersonCreateRequest>()
              .RuleFor(a => a.Name, b => b.Person.FirstName)
              .RuleFor(a => a.DriverLicenseType, b => DriverLicenseType.A)
              .RuleFor(a => a.DriverLicenseNumber, b => b.Random.String2(11))
              .RuleFor(a => a.DriverLicenseImage, b => _driverLicenseImage)
              .RuleFor(a => a.Document, b => b.Company.Cnpj(false))
              .Generate();

            var deliveryPerson = new AutoFaker<Domain.Entities.DeliveryPerson>()
              .RuleFor(a => a.DriverLicenseNumber, b => request.DriverLicenseNumber)
              .Generate();

            autoMocker
                .GetMock<IDeliveryPersonRepository>()
                .Setup(s =>
                    s.GetAsync(
                        It.IsAny<Expression<Func<Domain.Entities.DeliveryPerson, bool>>>(),
                        It.IsAny<CancellationToken>()
                    )
                ).ReturnsAsync(deliveryPerson);

            //Act
            var response = await handler.Handle(request, CancellationToken.None);

            //Assert
            var customResponseMessages = response.Response as List<CustomResponseMessage>;
            Assert.False(response.Success);
            Assert.Contains(customResponseMessages, c => c.Message == "Esse número da CNH já está em uso.");
        }

        [Fact]
        public async void DeliveryPersonCreate_DocumentExists_MustFail()
        {
            //Arrange
            var autoMocker = new AutoMocker();
            var handler = autoMocker.CreateInstance<DeliveryPersonCreateHandler>();

            var request = new AutoFaker<DeliveryPersonCreateRequest>()
              .RuleFor(a => a.Name, b => b.Person.FirstName)
              .RuleFor(a => a.DriverLicenseType, b => DriverLicenseType.A)
              .RuleFor(a => a.DriverLicenseNumber, b => b.Random.String2(11))
              .RuleFor(a => a.DriverLicenseImage, b => _driverLicenseImage)
              .RuleFor(a => a.Document, b => b.Company.Cnpj(false))
              .Generate();

            var deliveryPerson = new AutoFaker<Domain.Entities.DeliveryPerson>()
              .RuleFor(a => a.Document, b => request.Document)
              .Generate();

            autoMocker
                .GetMock<IDeliveryPersonRepository>()
                .Setup(s =>
                    s.GetAsync(
                        It.IsAny<Expression<Func<Domain.Entities.DeliveryPerson, bool>>>(),
                        It.IsAny<CancellationToken>()
                    )
                ).ReturnsAsync(deliveryPerson);

            //Act
            var response = await handler.Handle(request, CancellationToken.None);

            //Assert
            var customResponseMessages = response.Response as List<CustomResponseMessage>;
            Assert.False(response.Success);
            Assert.Contains(customResponseMessages, c => c.Message == "Esse CNPJ já está em uso.");
        }

        [Fact]
        public async void DeliveryPersonCreate_UploadDriverLicenseImageError_MustFail()
        {
            //Arrange
            var autoMocker = new AutoMocker();
            var handler = autoMocker.CreateInstance<DeliveryPersonCreateHandler>();

            var request = new AutoFaker<DeliveryPersonCreateRequest>()
              .RuleFor(a => a.Name, b => b.Person.FirstName)
              .RuleFor(a => a.DriverLicenseType, b => DriverLicenseType.A)
              .RuleFor(a => a.DriverLicenseNumber, b => b.Random.String2(11))
              .RuleFor(a => a.DriverLicenseImage, b => _driverLicenseImage)
              .RuleFor(a => a.Document, b => b.Company.Cnpj(false))
              .Generate();

            //Act
            var response = await handler.Handle(request, CancellationToken.None);

            //Assert
            var customResponseMessages = response.Response as List<CustomResponseMessage>;
            Assert.False(response.Success);
            Assert.Contains(customResponseMessages, c => c.Message == "Houve um erro ao realizar upload da imagem.");
        }

        [Fact]
        public async void DeliveryPersonCreate_CommitError_MustFail()
        {
            //Arrange
            var autoMocker = new AutoMocker();
            var handler = autoMocker.CreateInstance<DeliveryPersonCreateHandler>();

            var request = new AutoFaker<DeliveryPersonCreateRequest>()
              .RuleFor(a => a.Name, b => b.Person.FirstName)
              .RuleFor(a => a.DriverLicenseType, b => DriverLicenseType.A)
              .RuleFor(a => a.DriverLicenseNumber, b => b.Random.String2(11))
              .RuleFor(a => a.DriverLicenseImage, b => _driverLicenseImage)
              .RuleFor(a => a.Document, b => b.Company.Cnpj(false))
              .Generate();

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
        public async void DeliveryPersonCreate_RequestValid_MustSuccess()
        {
            //Arrange
            var validator = new DeliveryPersonCreateValidator();

            var request = new AutoFaker<DeliveryPersonCreateRequest>()
              .RuleFor(a => a.Name, b => b.Person.FirstName)
              .RuleFor(a => a.DriverLicenseType, b => DriverLicenseType.A)
              .RuleFor(a => a.DriverLicenseNumber, b => b.Random.String2(11))
              .RuleFor(a => a.DriverLicenseImage, b => _driverLicenseImage)
              .RuleFor(a => a.Document, b => b.Company.Cnpj(false))
              .Generate();

            //Act
            var response = await validator.TestValidateAsync(request);

            //Assert
            Assert.True(response.IsValid);
        }

        [Fact]
        public async void DeliveryPersonCreate_FabricationYearInvalid_MustFail()
        {
            //Arrange
            var validator = new DeliveryPersonCreateValidator();

            var request = new AutoFaker<DeliveryPersonCreateRequest>()
              .RuleFor(a => a.DateOfBirth, b => DateTime.Now.AddYears(-101))
              .Generate();

            //Act
            var response = await validator.TestValidateAsync(request);

            //Assert
            Assert.False(response.IsValid);
            Assert.Contains(response.Errors, c => c.ErrorMessage == $"A data de nascimento deve ser entre {DateTime.Now.AddYears(-100).ToShortDateString()} e {DateTime.Now.ToShortDateString()}.");
        }

        [Fact]
        public async void DeliveryPersonCreate_DriverLicenseNumberInvalid_MustFail()
        {
            //Arrange
            var validator = new DeliveryPersonCreateValidator();

            var request = new AutoFaker<DeliveryPersonCreateRequest>()
              .RuleFor(a => a.DriverLicenseNumber, b => b.Random.String2(20))
              .Generate();

            //Act
            var response = await validator.TestValidateAsync(request);

            //Assert
            Assert.False(response.IsValid);
            Assert.Contains(response.Errors, c => c.ErrorMessage == $"Tamanho do número da CNH tem que ter 11 caracteres.");
        }

        [Fact]
        public async void DeliveryPersonCreate_DriverLicenseImageInvalid_MustFail()
        {
            //Arrange
            var validator = new DeliveryPersonCreateValidator();

            var request = new AutoFaker<DeliveryPersonCreateRequest>()
              .RuleFor(a => a.DriverLicenseImage, b => b.Random.String2(1))
              .Generate();

            //Act
            var response = await validator.TestValidateAsync(request);

            //Assert
            Assert.False(response.IsValid);
            Assert.Contains(response.Errors, c => c.ErrorMessage == $"Imagem da CNH inválida.");
        }

        [Fact]
        public async void DeliveryPersonCreate_DocumentInvalid_MustFail()
        {
            //Arrange
            var validator = new DeliveryPersonCreateValidator();

            var request = new AutoFaker<DeliveryPersonCreateRequest>()
              .RuleFor(a => a.Document, b => b.Random.String2(20))
              .Generate();

            //Act
            var response = await validator.TestValidateAsync(request);

            //Assert
            Assert.False(response.IsValid);
            Assert.Contains(response.Errors, c => c.ErrorMessage == $"CNPJ inválido! Deve conter apenas números sem máscara.");
        }

        [Fact]
        public async void DeliveryPersonCreate_NameInvalid_MustFail()
        {
            //Arrange
            var validator = new DeliveryPersonCreateValidator();

            var request = new AutoFaker<DeliveryPersonCreateRequest>()
              .RuleFor(a => a.Name, b => $"#{b.Random.String2(105)}")
              .Generate();

            //Act
            var response = await validator.TestValidateAsync(request);

            //Assert
            Assert.False(response.IsValid);
            Assert.Contains(response.Errors, c => c.ErrorMessage == $"O nome deve conter apenas letras e números.");
            Assert.Contains(response.Errors, c => c.ErrorMessage == $"Tamanho máximo do nome é 100 caracteres.");
        }
    }
}
