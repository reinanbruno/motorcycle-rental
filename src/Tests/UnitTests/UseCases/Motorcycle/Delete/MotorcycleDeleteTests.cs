using Application.UseCases.Base;
using Application.UseCases.Motorcycle.Delete;
using AutoBogus;
using Domain.Repositories;
using Moq;
using Moq.AutoMock;

namespace Tests.UnitTests.UseCases.Motorcycle.Delete
{
    public class MotorcycleDeleteTests
    {
        [Fact]
        public async void MotorcycleDelete_Valid_MustSuccess()
        {
            //Arrange
            var autoMocker = new AutoMocker();
            var handler = autoMocker.CreateInstance<MotorcycleDeleteHandler>();

            var request = new AutoFaker<MotorcycleDeleteRequest>()
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
        public async void MotorcycleDelete_IdNotFound_MustFail()
        {
            //Arrange
            var autoMocker = new AutoMocker();
            var handler = autoMocker.CreateInstance<MotorcycleDeleteHandler>();

            var request = new AutoFaker<MotorcycleDeleteRequest>()
              .Generate();

            //Act
            var response = await handler.Handle(request, CancellationToken.None);

            //Assert
            var customResponseMessages = response.Response as List<CustomResponseMessage>;
            Assert.False(response.Success);
            Assert.Contains(customResponseMessages, c => c.Message == "Identificador não localizado.");
        }


        [Fact]
        public async void MotorcycleDelete_RentalExists_MustFail()
        {
            //Arrange
            var autoMocker = new AutoMocker();
            var handler = autoMocker.CreateInstance<MotorcycleDeleteHandler>();

            var request = new AutoFaker<MotorcycleDeleteRequest>()
              .Generate();

            var motorcycle = new AutoFaker<Domain.Entities.Motorcycle>()
              .RuleFor(a => a.Id, b => request.Id)
              .Generate();

            var rental = new AutoFaker<Domain.Entities.Rental>()
              .RuleFor(a => a.MotorcycleId, b => request.Id)
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
                .GetMock<IRentalRepository>()
                .Setup(s =>
                    s.GetAsync(
                        x => x.MotorcycleId == request.Id,
                        It.IsAny<CancellationToken>()
                    )
                ).ReturnsAsync(rental);

            //Act
            var response = await handler.Handle(request, CancellationToken.None);

            //Assert
            var customResponseMessages = response.Response as List<CustomResponseMessage>;
            Assert.False(response.Success);
            Assert.Contains(customResponseMessages, c => c.Message == "Não é possível excluir essa moto pois ela tem locações.");
        }

        [Fact]
        public async void MotorcycleDelete_CommitError_MustFail()
        {
            //Arrange
            var autoMocker = new AutoMocker();
            var handler = autoMocker.CreateInstance<MotorcycleDeleteHandler>();

            var request = new AutoFaker<MotorcycleDeleteRequest>()
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
    }
}
