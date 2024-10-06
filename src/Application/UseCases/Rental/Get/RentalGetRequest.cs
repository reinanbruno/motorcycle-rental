﻿using MediatR;
using Application.UseCases.Base;

namespace Application.UseCases.Rental.List
{
    public class RentalGetRequest : IRequest<CustomResponse>
    {
        public Guid Id { get; set; }
    }
}
