using Application.UseCases.Base;
using Application.UseCases.Motorcycle.Create;
using AutoBogus;
using Domain.Repositories;
using FluentValidation.TestHelper;
using Moq;
using Moq.AutoMock;

namespace Tests.UnitTests.UseCases.Motorcycle.Create
{
    public class MotorcycleCreateTests
    {
        [Fact]
        public async void MotorcycleCreate_Valid_MustSuccess()
        {
            //Arrange
            var autoMocker = new AutoMocker();
            var handler = autoMocker.CreateInstance<MotorcycleCreateHandler>();

            var request = new AutoFaker<MotorcycleCreateRequest>()
              .RuleFor(a => a.Plate, b => b.Random.AlphaNumeric(7))
              .RuleFor(a => a.Model, b => b.Random.AlphaNumeric(10))
              .RuleFor(a => a.FabricationYear, b => b.Random.Int(1900, 2024))
              .Generate();

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
        public async void MotorcycleCreate_IdExists_MustFail()
        {
            //Arrange
            var autoMocker = new AutoMocker();
            var handler = autoMocker.CreateInstance<MotorcycleCreateHandler>();

            var request = new AutoFaker<MotorcycleCreateRequest>()
              .RuleFor(a => a.Plate, b => b.Random.AlphaNumeric(7))
              .RuleFor(a => a.Model, b => b.Random.AlphaNumeric(10))
              .RuleFor(a => a.FabricationYear, b => b.Random.Int(1900, 2024))
              .Generate();

            var motorcycle = new AutoFaker<Domain.Entities.Motorcycle>()
              .RuleFor(a => a.Id, b => request.Id)
              .Generate();

            autoMocker
                .GetMock<IMotorcycleRepository>()
                .Setup(s =>
                    s.GetAsync(
                        x => x.Id == request.Id,
                        It.IsAny<CancellationToken>()
                    )
                ).ReturnsAsync(motorcycle);
            
            //Act
            var response = await handler.Handle(request, CancellationToken.None);

            //Assert
            var customResponseMessages = response.Response as List<CustomResponseMessage>;
            Assert.False(response.Success);
            Assert.Contains(customResponseMessages, c => c.Message == "Esse identificador já está em uso.");
        }

        [Fact]
        public async void MotorcycleCreate_PlateExists_MustFail()
        {
            //Arrange
            var autoMocker = new AutoMocker();
            var handler = autoMocker.CreateInstance<MotorcycleCreateHandler>();

            var request = new AutoFaker<MotorcycleCreateRequest>()
              .RuleFor(a => a.Plate, b => b.Random.AlphaNumeric(7))
              .RuleFor(a => a.Model, b => b.Random.AlphaNumeric(10))
              .RuleFor(a => a.FabricationYear, b => b.Random.Int(1900, 2024))
              .Generate();

            var motorcycle = new AutoFaker<Domain.Entities.Motorcycle>()
              .RuleFor(a => a.Plate, b => request.Plate)
              .Generate();

            autoMocker
                .GetMock<IMotorcycleRepository>()
                .Setup(s =>
                    s.GetAsync(
                        x => x.Plate == request.Plate,
                        It.IsAny<CancellationToken>()
                    )
                ).ReturnsAsync(motorcycle);

            //Act
            var response = await handler.Handle(request, CancellationToken.None);

            //Assert
            var customResponseMessages = response.Response as List<CustomResponseMessage>;
            Assert.False(response.Success);
            Assert.Contains(customResponseMessages, c => c.Message == "Essa placa já está em uso.");
        }

        [Fact]
        public async void MotorcycleCreate_CommitError_MustFail()
        {
            //Arrange
            var autoMocker = new AutoMocker();
            var handler = autoMocker.CreateInstance<MotorcycleCreateHandler>();

            var request = new AutoFaker<MotorcycleCreateRequest>()
              .RuleFor(a => a.Plate, b => b.Random.AlphaNumeric(7))
              .RuleFor(a => a.Model, b => b.Random.AlphaNumeric(10))
              .RuleFor(a => a.FabricationYear, b => b.Random.Int(1900, 2024))
              .Generate();

            //Act
            var response = await handler.Handle(request, CancellationToken.None);

            //Assert
            var customResponseMessages = response.Response as List<CustomResponseMessage>;
            Assert.False(response.Success);
            Assert.Contains(customResponseMessages, c => c.Message == "Houve um problema ao persistir suas informações. Por favor, tente novamente em instantes.");
        }


        [Fact]
        public async void MotorcycleCreate_RequestValid_MustSuccess()
        {
            //Arrange
            var validator = new MotorcycleCreateValidator();

            var request = new AutoFaker<MotorcycleCreateRequest>()
              .RuleFor(a => a.Plate, b => b.Random.AlphaNumeric(7))
              .RuleFor(a => a.Model, b => b.Random.AlphaNumeric(10))
              .RuleFor(a => a.FabricationYear, b => b.Random.Int(1900, 2024))
              .Generate();

            //Act
            var response = await validator.TestValidateAsync(request);

            //Assert
            Assert.True(response.IsValid);
        }

        [Fact]
        public async void MotorcycleCreate_PlateInvalid_MustFail()
        {
            //Arrange
            var validator = new MotorcycleCreateValidator();

            var request = new AutoFaker<MotorcycleCreateRequest>()
              .RuleFor(a => a.Plate, b => $"#{b.Random.String2(20)}")
              .Generate();

            //Act
            var response = await validator.TestValidateAsync(request);

            //Assert
            Assert.False(response.IsValid);
            Assert.Contains(response.Errors, c => c.ErrorMessage == "A placa deve conter apenas letras e números.");
            Assert.Contains(response.Errors, c => c.ErrorMessage == "A placa deve conter apenas letras e números.");
        }

        [Fact]
        public async void MotorcycleCreate_ModelInvalid_MustFail()
        {
            //Arrange
            var validator = new MotorcycleCreateValidator();

            var request = new AutoFaker<MotorcycleCreateRequest>()
              .RuleFor(a => a.Model, b => b.Random.String2(100))
              .Generate();

            //Act
            var response = await validator.TestValidateAsync(request);

            //Assert
            Assert.False(response.IsValid);
            Assert.Contains(response.Errors, c => c.ErrorMessage == "Tamanho máximo do modelo é 50 caracteres.");
        }

        [Fact]
        public async void MotorcycleCreate_FabricationYearInvalid_MustFail()
        {
            //Arrange
            var validator = new MotorcycleCreateValidator();

            var request = new AutoFaker<MotorcycleCreateRequest>()
              .RuleFor(a => a.FabricationYear, b => b.Random.Int(1700, 1850))
              .Generate();

            //Act
            var response = await validator.TestValidateAsync(request);

            //Assert
            Assert.False(response.IsValid);
            Assert.Contains(response.Errors, c => c.ErrorMessage == "O ano deve ser entre 1900 e 2024.");
        }
    }
}