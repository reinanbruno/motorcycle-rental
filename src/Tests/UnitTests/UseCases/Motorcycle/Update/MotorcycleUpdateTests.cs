using Application.UseCases.Base;
using Application.UseCases.Motorcycle.Create;
using Application.UseCases.Motorcycle.Update;
using AutoBogus;
using Domain.Repositories;
using FluentValidation.TestHelper;
using Moq;
using Moq.AutoMock;

namespace Tests.UnitTests.UseCases.Motorcycle.Update
{
    public class MotorcycleUpdateTests
    {
        [Fact]
        public async void MotorcycleUpdate_Valid_MustSuccess()
        {
            //Arrange
            var autoMocker = new AutoMocker();
            var handler = autoMocker.CreateInstance<MotorcycleUpdateHandler>();

            var request = new AutoFaker<MotorcycleUpdateRequest>()
              .RuleFor(a => a.Plate, b => b.Random.AlphaNumeric(7))
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
        public async void MotorcycleUpdate_IdNotFound_MustFail()
        {
            //Arrange
            var autoMocker = new AutoMocker();
            var handler = autoMocker.CreateInstance<MotorcycleUpdateHandler>();

            var request = new AutoFaker<MotorcycleUpdateRequest>()
              .RuleFor(a => a.Plate, b => b.Random.AlphaNumeric(7))
              .Generate();

            //Act
            var response = await handler.Handle(request, CancellationToken.None);

            //Assert
            var customResponseMessages = response.Response as List<CustomResponseMessage>;
            Assert.False(response.Success);
            Assert.Contains(customResponseMessages, c => c.Message == "Identificador não localizado.");
        }

        [Fact]
        public async void MotorcycleUpdate_PlateExists_MustFail()
        {
            //Arrange
            var autoMocker = new AutoMocker();
            var handler = autoMocker.CreateInstance<MotorcycleUpdateHandler>();

            var request = new AutoFaker<MotorcycleUpdateRequest>()
              .RuleFor(a => a.Plate, b => b.Random.AlphaNumeric(7))
              .Generate();

            var motorcycle = new AutoFaker<Domain.Entities.Motorcycle>()
              .RuleFor(a => a.Id, b => request.Id)
              .RuleFor(a => a.Plate, b => request.Plate)
              .Generate();

            autoMocker
                .GetMock<IMotorcycleRepository>()
                .Setup(s =>
                    s.GetAsync(
                        x => x.Id == request.Id,
                        It.IsAny<CancellationToken>()
                    )
                ).ReturnsAsync(motorcycle);

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
        public async void MotorcycleUpdate_CommitError_MustFail()
        {
            //Arrange
            var autoMocker = new AutoMocker();
            var handler = autoMocker.CreateInstance<MotorcycleUpdateHandler>();

            var request = new AutoFaker<MotorcycleUpdateRequest>()
              .RuleFor(a => a.Plate, b => b.Random.AlphaNumeric(7))
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
            Assert.Contains(customResponseMessages, c => c.Message == "Houve um problema ao persistir suas informações. Por favor, tente novamente em instantes.");
        }

        [Fact]
        public async void MotorcycleUpdate_RequestValid_MustSuccess()
        {
            //Arrange
            var validator = new MotorcycleUpdateValidator();

            var request = new AutoFaker<MotorcycleUpdateRequest>()
              .RuleFor(a => a.Plate, b => b.Random.AlphaNumeric(7))
              .Generate();

            //Act
            var response = await validator.TestValidateAsync(request);

            //Assert
            Assert.True(response.IsValid);
        }

        [Fact]
        public async void MotorcycleUpdate_PlateInvalid_MustFail()
        {
            //Arrange
            var validator = new MotorcycleUpdateValidator();

            var request = new AutoFaker<MotorcycleUpdateRequest>()
              .RuleFor(a => a.Plate, b => $"#{b.Random.String2(20)}")
              .Generate();

            //Act
            var response = await validator.TestValidateAsync(request);

            //Assert
            Assert.False(response.IsValid);
            Assert.Contains(response.Errors, c => c.ErrorMessage == "A placa deve conter apenas letras e números.");
            Assert.Contains(response.Errors, c => c.ErrorMessage == "A placa deve conter apenas letras e números.");
        }
    }
}
