using Application.UseCases.Base;
using Application.UseCases.Motorcycle.Create;
using Application.UseCases.Rental.Create;
using AutoBogus;
using Domain.Enums;
using Domain.Repositories;
using FluentValidation.TestHelper;
using Moq;
using Moq.AutoMock;

namespace Tests.UnitTests.UseCases.Rental.Create
{
    public class RentalCreateTests
    {
        [Fact]
        public async void RentalCreate_Valid_MustSuccess()
        {
            //Arrange
            var autoMocker = new AutoMocker();
            var handler = autoMocker.CreateInstance<RentalCreateHandler>();

            var request = new AutoFaker<RentalCreateRequest>()
              .RuleFor(a => a.Plan, b => 7)
              .RuleFor(a => a.StartDate, b => DateTime.Now)
              .RuleFor(a => a.EndDate, b => DateTime.Now.AddYears(7))
              .RuleFor(a => a.ExpectedEndDate, b => DateTime.Now.AddYears(7))
              .Generate();

            var deliveryPerson = new AutoFaker<Domain.Entities.DeliveryPerson>()
              .RuleFor(a => a.Id, b => request.DeliveryPersonId)
              .RuleFor(a => a.DriverLicenseType, b => DriverLicenseType.A)
              .Generate();

            var motorcycle = new AutoFaker<Domain.Entities.Motorcycle>()
              .RuleFor(a => a.Id, b => request.MotorcycleId)
              .Generate();

            var rentalPlan = new AutoFaker<Domain.Entities.RentalPlan>()
              .RuleFor(a => a.DurationDays, b => request.Plan)
              .Generate();

            autoMocker
                .GetMock<IDeliveryPersonRepository>()
                .Setup(s =>
                    s.GetAsync(
                        x => x.Id == request.DeliveryPersonId,
                        It.IsAny<CancellationToken>()
                    )
                ).ReturnsAsync(deliveryPerson);

            autoMocker
                .GetMock<IMotorcycleRepository>()
                .Setup(s =>
                    s.GetAsync(
                        x => x.Id == request.MotorcycleId,
                        It.IsAny<CancellationToken>()
                    )
                ).ReturnsAsync(motorcycle);

            autoMocker
                .GetMock<IRentalPlanRepository>()
                .Setup(s =>
                    s.GetAsync(
                        x => x.DurationDays == request.Plan,
                        It.IsAny<CancellationToken>()
                    )
                ).ReturnsAsync(rentalPlan);

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
        public async void RentalCreate_MotorcycleIdNotExists_MustFail()
        {
            //Arrange
            var autoMocker = new AutoMocker();
            var handler = autoMocker.CreateInstance<RentalCreateHandler>();

            var request = new AutoFaker<RentalCreateRequest>()
              .RuleFor(a => a.Plan, b => 7)
              .RuleFor(a => a.StartDate, b => DateTime.Now)
              .RuleFor(a => a.EndDate, b => DateTime.Now.AddYears(7))
              .RuleFor(a => a.ExpectedEndDate, b => DateTime.Now.AddYears(7))
              .Generate();

            //Act
            var response = await handler.Handle(request, CancellationToken.None);

            //Assert
            var customResponseMessages = response.Response as List<CustomResponseMessage>;
            Assert.False(response.Success);
            Assert.Contains(customResponseMessages, c => c.Message == "Não existe nenhuma moto com esse identificador.");
        }

        [Fact]
        public async void RentalCreate_DeliveryPersonIdNotExists_MustFail()
        {
            //Arrange
            var autoMocker = new AutoMocker();
            var handler = autoMocker.CreateInstance<RentalCreateHandler>();

            var request = new AutoFaker<RentalCreateRequest>()
              .RuleFor(a => a.Plan, b => 7)
              .RuleFor(a => a.StartDate, b => DateTime.Now)
              .RuleFor(a => a.EndDate, b => DateTime.Now.AddYears(7))
              .RuleFor(a => a.ExpectedEndDate, b => DateTime.Now.AddYears(7))
              .Generate();

            var motorcycle = new AutoFaker<Domain.Entities.Motorcycle>()
              .RuleFor(a => a.Id, b => request.MotorcycleId)
              .Generate();

            autoMocker
                .GetMock<IMotorcycleRepository>()
                .Setup(s =>
                    s.GetAsync(
                        x => x.Id == request.MotorcycleId,
                        It.IsAny<CancellationToken>()
                    )
                ).ReturnsAsync(motorcycle);

            //Act
            var response = await handler.Handle(request, CancellationToken.None);

            //Assert
            var customResponseMessages = response.Response as List<CustomResponseMessage>;
            Assert.False(response.Success);
            Assert.Contains(customResponseMessages, c => c.Message == "Não existe nenhum entregador com esse identificador.");
        }

        [Fact]
        public async void RentalCreate_RentalExistsForDeliveryPersonId_MustFail()
        {
            //Arrange
            var autoMocker = new AutoMocker();
            var handler = autoMocker.CreateInstance<RentalCreateHandler>();

            var request = new AutoFaker<RentalCreateRequest>()
              .RuleFor(a => a.Plan, b => 7)
              .RuleFor(a => a.StartDate, b => DateTime.Now)
              .RuleFor(a => a.EndDate, b => DateTime.Now.AddYears(7))
              .RuleFor(a => a.ExpectedEndDate, b => DateTime.Now.AddYears(7))
              .Generate();

            var deliveryPerson = new AutoFaker<Domain.Entities.DeliveryPerson>()
              .RuleFor(a => a.Id, b => request.DeliveryPersonId)
              .RuleFor(a => a.DriverLicenseType, b => DriverLicenseType.A)
              .Generate();

            var motorcycle = new AutoFaker<Domain.Entities.Motorcycle>()
              .RuleFor(a => a.Id, b => request.MotorcycleId)
              .Generate();

            var rental = new AutoFaker<Domain.Entities.Rental>()
              .RuleFor(a => a.DeliveryPersonId, b => request.DeliveryPersonId)
              .RuleFor(a => a.ReturnDate, b => null)
              .Generate();

            autoMocker
                .GetMock<IDeliveryPersonRepository>()
                .Setup(s =>
                    s.GetAsync(
                        x => x.Id == request.DeliveryPersonId,
                        It.IsAny<CancellationToken>()
                    )
                ).ReturnsAsync(deliveryPerson);

            autoMocker
                .GetMock<IMotorcycleRepository>()
                .Setup(s =>
                    s.GetAsync(
                        x => x.Id == request.MotorcycleId,
                        It.IsAny<CancellationToken>()
                    )
                ).ReturnsAsync(motorcycle);

            autoMocker
                .GetMock<IRentalRepository>()
                .Setup(s =>
                    s.GetAsync(
                        x => x.DeliveryPersonId == request.DeliveryPersonId && x.ReturnDate == null,
                        It.IsAny<CancellationToken>()
                    )
                ).ReturnsAsync(rental);

            //Act
            var response = await handler.Handle(request, CancellationToken.None);

            //Assert
            var customResponseMessages = response.Response as List<CustomResponseMessage>;
            Assert.False(response.Success);
            Assert.Contains(customResponseMessages, c => c.Message == "Esse entregador já tem uma locação, é necessário finalizar uma locação para solicitar outra.");
        }

        [Fact]
        public async void RentalCreate_DriverLicenseTypeDifferentThanA_MustFail()
        {
            //Arrange
            var autoMocker = new AutoMocker();
            var handler = autoMocker.CreateInstance<RentalCreateHandler>();

            var request = new AutoFaker<RentalCreateRequest>()
              .RuleFor(a => a.Plan, b => 7)
              .RuleFor(a => a.StartDate, b => DateTime.Now)
              .RuleFor(a => a.EndDate, b => DateTime.Now.AddYears(7))
              .RuleFor(a => a.ExpectedEndDate, b => DateTime.Now.AddYears(7))
              .Generate();

            var deliveryPerson = new AutoFaker<Domain.Entities.DeliveryPerson>()
              .RuleFor(a => a.Id, b => request.DeliveryPersonId)
              .RuleFor(a => a.DriverLicenseType, b => DriverLicenseType.B)
              .Generate();

            var motorcycle = new AutoFaker<Domain.Entities.Motorcycle>()
              .RuleFor(a => a.Id, b => request.MotorcycleId)
              .Generate();

            autoMocker
                .GetMock<IDeliveryPersonRepository>()
                .Setup(s =>
                    s.GetAsync(
                        x => x.Id == request.DeliveryPersonId,
                        It.IsAny<CancellationToken>()
                    )
                ).ReturnsAsync(deliveryPerson);

            autoMocker
                .GetMock<IMotorcycleRepository>()
                .Setup(s =>
                    s.GetAsync(
                        x => x.Id == request.MotorcycleId,
                        It.IsAny<CancellationToken>()
                    )
                ).ReturnsAsync(motorcycle);

            //Act
            var response = await handler.Handle(request, CancellationToken.None);

            //Assert
            var customResponseMessages = response.Response as List<CustomResponseMessage>;
            Assert.False(response.Success);
            Assert.Contains(customResponseMessages, c => c.Message == "Somente entregadores do tipo de categoria A pode fazer uma locação.");
        }

        [Fact]
        public async void RentalCreate_PlanNotExists_MustFail()
        {
            //Arrange
            var autoMocker = new AutoMocker();
            var handler = autoMocker.CreateInstance<RentalCreateHandler>();

            var request = new AutoFaker<RentalCreateRequest>()
              .RuleFor(a => a.Plan, b => 7)
              .RuleFor(a => a.StartDate, b => DateTime.Now)
              .RuleFor(a => a.EndDate, b => DateTime.Now.AddYears(7))
              .RuleFor(a => a.ExpectedEndDate, b => DateTime.Now.AddYears(7))
              .Generate();

            var deliveryPerson = new AutoFaker<Domain.Entities.DeliveryPerson>()
              .RuleFor(a => a.Id, b => request.DeliveryPersonId)
              .RuleFor(a => a.DriverLicenseType, b => DriverLicenseType.A)
              .Generate();

            var motorcycle = new AutoFaker<Domain.Entities.Motorcycle>()
              .RuleFor(a => a.Id, b => request.MotorcycleId)
              .Generate();

            var rentalPlan = new AutoFaker<Domain.Entities.RentalPlan>()
              .RuleFor(a => a.DurationDays, b => request.Plan)
              .Generate();

            autoMocker
                .GetMock<IDeliveryPersonRepository>()
                .Setup(s =>
                    s.GetAsync(
                        x => x.Id == request.DeliveryPersonId,
                        It.IsAny<CancellationToken>()
                    )
                ).ReturnsAsync(deliveryPerson);

            autoMocker
                .GetMock<IMotorcycleRepository>()
                .Setup(s =>
                    s.GetAsync(
                        x => x.Id == request.MotorcycleId,
                        It.IsAny<CancellationToken>()
                    )
                ).ReturnsAsync(motorcycle);

            //Act
            var response = await handler.Handle(request, CancellationToken.None);

            //Assert
            var customResponseMessages = response.Response as List<CustomResponseMessage>;
            Assert.False(response.Success);
            Assert.Contains(customResponseMessages, c => c.Message == "Não existe nenhum plano com essa quantidade de dias.");
        }

        [Fact]
        public async void RentalCreate_CommitError_MustFail()
        {
            //Arrange
            var autoMocker = new AutoMocker();
            var handler = autoMocker.CreateInstance<RentalCreateHandler>();

            var request = new AutoFaker<RentalCreateRequest>()
              .RuleFor(a => a.Plan, b => 7)
              .RuleFor(a => a.StartDate, b => DateTime.Now)
              .RuleFor(a => a.EndDate, b => DateTime.Now.AddYears(7))
              .RuleFor(a => a.ExpectedEndDate, b => DateTime.Now.AddYears(7))
              .Generate();

            var deliveryPerson = new AutoFaker<Domain.Entities.DeliveryPerson>()
              .RuleFor(a => a.Id, b => request.DeliveryPersonId)
              .RuleFor(a => a.DriverLicenseType, b => DriverLicenseType.A)
              .Generate();

            var motorcycle = new AutoFaker<Domain.Entities.Motorcycle>()
              .RuleFor(a => a.Id, b => request.MotorcycleId)
              .Generate();

            var rentalPlan = new AutoFaker<Domain.Entities.RentalPlan>()
              .RuleFor(a => a.DurationDays, b => request.Plan)
              .Generate();

            autoMocker
                .GetMock<IDeliveryPersonRepository>()
                .Setup(s =>
                    s.GetAsync(
                        x => x.Id == request.DeliveryPersonId,
                        It.IsAny<CancellationToken>()
                    )
                ).ReturnsAsync(deliveryPerson);

            autoMocker
                .GetMock<IMotorcycleRepository>()
                .Setup(s =>
                    s.GetAsync(
                        x => x.Id == request.MotorcycleId,
                        It.IsAny<CancellationToken>()
                    )
                ).ReturnsAsync(motorcycle);

            autoMocker
                .GetMock<IRentalPlanRepository>()
                .Setup(s =>
                    s.GetAsync(
                        x => x.DurationDays == request.Plan,
                        It.IsAny<CancellationToken>()
                    )
                ).ReturnsAsync(rentalPlan);

            //Act
            var response = await handler.Handle(request, CancellationToken.None);

            //Assert
            var customResponseMessages = response.Response as List<CustomResponseMessage>;
            Assert.False(response.Success);
            Assert.Contains(customResponseMessages, c => c.Message == "Houve um problema ao persistir suas informações. Por favor, tente novamente em instantes.");
        }

        [Fact]
        public async void RentalCreate_RequestValid_MustSuccess()
        {
            //Arrange
            var validator = new RentalCreateValidator();

            var request = new AutoFaker<RentalCreateRequest>()
              .RuleFor(a => a.Plan, b => 7)
              .RuleFor(a => a.StartDate, b => DateTime.Now)
              .RuleFor(a => a.EndDate, b => DateTime.Now.AddYears(7))
              .RuleFor(a => a.ExpectedEndDate, b => DateTime.Now.AddYears(7))
              .Generate();

            //Act
            var response = await validator.TestValidateAsync(request);

            //Assert
            Assert.True(response.IsValid);
        }

        [Fact]
        public async void RentalCreate_PlanInvalid_MustFail()
        {
            //Arrange
            var validator = new RentalCreateValidator();

            var request = new AutoFaker<RentalCreateRequest>()
              .RuleFor(a => a.Plan, b => 0)
              .RuleFor(a => a.StartDate, b => DateTime.Now.AddDays(7))
              .RuleFor(a => a.EndDate, b => DateTime.Now.AddDays(7))
              .RuleFor(a => a.ExpectedEndDate, b => DateTime.Now.AddDays(7))
              .Generate();

            //Act
            var response = await validator.TestValidateAsync(request);

            //Assert
            Assert.False(response.IsValid);
            Assert.Contains(response.Errors, c => c.ErrorMessage == "O plano tem que ser maior que 0.");
        }

        [Fact]
        public async void RentalCreate_StartDateInvalid_MustFail()
        {
            //Arrange
            var validator = new RentalCreateValidator();

            var request = new AutoFaker<RentalCreateRequest>()
              .RuleFor(a => a.Plan, b => 7)
              .RuleFor(a => a.StartDate, b => DateTime.Now.AddDays(-2))
              .RuleFor(a => a.EndDate, b => DateTime.Now.AddDays(7))
              .RuleFor(a => a.ExpectedEndDate, b => DateTime.Now.AddDays(7))
              .Generate();

            //Act
            var response = await validator.TestValidateAsync(request);

            //Assert
            Assert.False(response.IsValid);
            Assert.Contains(response.Errors, c => c.ErrorMessage == $"A data de inicio da locação tem que ser maior que {DateTime.Now.ToShortDateString()}.");
        }

        [Fact]
        public async void RentalCreate_StartEndInvalid_MustFail()
        {
            //Arrange
            var validator = new RentalCreateValidator();

            var request = new AutoFaker<RentalCreateRequest>()
              .RuleFor(a => a.Plan, b => 7)
              .RuleFor(a => a.StartDate, b => DateTime.Now)
              .RuleFor(a => a.EndDate, b => DateTime.Now.AddDays(-2))
              .RuleFor(a => a.ExpectedEndDate, b => DateTime.Now)
              .Generate();

            //Act
            var response = await validator.TestValidateAsync(request);

            //Assert
            Assert.False(response.IsValid);
            Assert.Contains(response.Errors, c => c.ErrorMessage == "A data de término tem que ser maior ou igual a data de inicio + dias de plano.");
        }

        [Fact]
        public async void RentalCreate_ExpectedEndDateInvalid_MustFail()
        {
            //Arrange
            var validator = new RentalCreateValidator();

            var request = new AutoFaker<RentalCreateRequest>()
              .RuleFor(a => a.Plan, b => 7)
              .RuleFor(a => a.StartDate, b => DateTime.Now)
              .RuleFor(a => a.EndDate, b => DateTime.Now.AddDays(-2))
              .RuleFor(a => a.ExpectedEndDate, b => DateTime.Now.AddDays(-2))
              .Generate();

            //Act
            var response = await validator.TestValidateAsync(request);

            //Assert
            Assert.False(response.IsValid);
            Assert.Contains(response.Errors, c => c.ErrorMessage == "A previsão de data de término tem que ser maior ou igual a data de inicio + dias de plano.");
        }
    }
}
