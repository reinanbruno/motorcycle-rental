using Application.UseCases.Base;
using Application.UseCases.Rental.Create;
using Application.UseCases.Rental.Update;
using AutoBogus;
using Domain.Repositories;
using FluentValidation.TestHelper;
using Moq;
using Moq.AutoMock;

namespace Tests.UnitTests.UseCases.Rental.Update
{
    public class RentalUpdateTests
    {
        [Fact]
        public async void RentalUpdate_Valid_MustSuccess()
        {
            //Arrange
            var autoMocker = new AutoMocker();
            var handler = autoMocker.CreateInstance<RentalUpdateHandler>();

            var request = new AutoFaker<RentalUpdateRequest>()
              .RuleFor(a => a.ReturnDate, b => DateTime.Now)
              .Generate();

            var rentalPlan = new AutoFaker<Domain.Entities.RentalPlan>()
              .RuleFor(a => a.DurationDays, b => 7)
              .RuleFor(a => a.DailyAmount, b => 30)
              .RuleFor(a => a.FinePercentage, b => 20)
              .Generate();

            var rental = new AutoFaker<Domain.Entities.Rental>()
              .RuleFor(a => a.Id, b => request.Id)
              .RuleFor(a => a.ReturnDate, b => null)
              .RuleFor(a => a.RentalPlanId, b => rentalPlan.Id)
              .RuleFor(a => a.RentalPlan, b => rentalPlan)
              .Generate();

            autoMocker
                .GetMock<IRentalRepository>()
                .Setup(s =>
                    s.GetAsync(
                        x => x.Id == request.Id,
                        It.IsAny<CancellationToken>(),
                        i => i.RentalPlan
                    )
                ).ReturnsAsync(rental);

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
        public async void RentalUpdate_IdNotExists_MustFail()
        {
            //Arrange
            var autoMocker = new AutoMocker();
            var handler = autoMocker.CreateInstance<RentalUpdateHandler>();

            var request = new AutoFaker<RentalUpdateRequest>()
              .RuleFor(a => a.ReturnDate, b => DateTime.Now)
              .Generate();

            //Act
            var response = await handler.Handle(request, CancellationToken.None);

            //Assert
            var customResponseMessages = response.Response as List<CustomResponseMessage>;
            Assert.False(response.Success);
            Assert.Contains(customResponseMessages, c => c.Message == "Identificador não localizado.");
        }

        [Fact]
        public async void RentalUpdate_ReturnDataDifferentThanNull_MustFail()
        {
            //Arrange
            var autoMocker = new AutoMocker();
            var handler = autoMocker.CreateInstance<RentalUpdateHandler>();

            var request = new AutoFaker<RentalUpdateRequest>()
              .RuleFor(a => a.ReturnDate, b => DateTime.Now)
              .Generate();

            var rentalPlan = new AutoFaker<Domain.Entities.RentalPlan>()
              .RuleFor(a => a.DurationDays, b => 7)
              .RuleFor(a => a.DailyAmount, b => 30)
              .RuleFor(a => a.FinePercentage, b => 20)
              .Generate();

            var rental = new AutoFaker<Domain.Entities.Rental>()
              .RuleFor(a => a.Id, b => request.Id)
              .RuleFor(a => a.ReturnDate, b => DateTime.Now)
              .RuleFor(a => a.RentalPlanId, b => rentalPlan.Id)
              .RuleFor(a => a.RentalPlan, b => rentalPlan)
              .Generate();

            autoMocker
                .GetMock<IRentalRepository>()
                .Setup(s =>
                    s.GetAsync(
                        x => x.Id == request.Id,
                        It.IsAny<CancellationToken>(),
                        i => i.RentalPlan
                    )
                ).ReturnsAsync(rental);

            //Act
            var response = await handler.Handle(request, CancellationToken.None);

            //Assert
            var customResponseMessages = response.Response as List<CustomResponseMessage>;
            Assert.False(response.Success);
            Assert.Contains(customResponseMessages, c => c.Message == "Locação não disponível para devolução.");
        }

        [Fact]
        public async void RentalUpdate_CommitError_MustFail()
        {
            //Arrange
            var autoMocker = new AutoMocker();
            var handler = autoMocker.CreateInstance<RentalUpdateHandler>();

            var request = new AutoFaker<RentalUpdateRequest>()
              .RuleFor(a => a.ReturnDate, b => DateTime.Now)
              .Generate();

            var rentalPlan = new AutoFaker<Domain.Entities.RentalPlan>()
              .RuleFor(a => a.DurationDays, b => 7)
              .RuleFor(a => a.DailyAmount, b => 30)
              .RuleFor(a => a.FinePercentage, b => 20)
              .Generate();

            var rental = new AutoFaker<Domain.Entities.Rental>()
              .RuleFor(a => a.Id, b => request.Id)
              .RuleFor(a => a.ReturnDate, b => null)
              .RuleFor(a => a.RentalPlanId, b => rentalPlan.Id)
              .RuleFor(a => a.RentalPlan, b => rentalPlan)
              .Generate();

            autoMocker
                .GetMock<IRentalRepository>()
                .Setup(s =>
                    s.GetAsync(
                        x => x.Id == request.Id,
                        It.IsAny<CancellationToken>(),
                        i => i.RentalPlan
                    )
                ).ReturnsAsync(rental);

            //Act
            var response = await handler.Handle(request, CancellationToken.None);

            //Assert
            var customResponseMessages = response.Response as List<CustomResponseMessage>;
            Assert.False(response.Success);
            Assert.Contains(customResponseMessages, c => c.Message == "Houve um problema ao persistir suas informações. Por favor, tente novamente em instantes.");
        }

        [Fact]
        public async void RentalUpdate_RequestValid_MustSuccess()
        {
            //Arrange
            var validator = new RentalUpdateValidator();

            var request = new AutoFaker<RentalUpdateRequest>()
              .RuleFor(a => a.ReturnDate, b => DateTime.Now)
              .Generate();

            //Act
            var response = await validator.TestValidateAsync(request);

            //Assert
            Assert.True(response.IsValid);
        }

        [Fact]
        public async void RentalUpdate_ReturnDataInvalid_MustFail()
        {
            //Arrange
            var validator = new RentalUpdateValidator();

            var request = new AutoFaker<RentalUpdateRequest>()
              .RuleFor(a => a.ReturnDate, b => DateTime.Now.AddDays(-2))
              .Generate();

            //Act
            var response = await validator.TestValidateAsync(request);

            //Assert
            Assert.False(response.IsValid);
            Assert.Contains(response.Errors, c => c.ErrorMessage == "A data de devolução tem que ser maior ou igual a data de hoje.");
        }
    }
}
