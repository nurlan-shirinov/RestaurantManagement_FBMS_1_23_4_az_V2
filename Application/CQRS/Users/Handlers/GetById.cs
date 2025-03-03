using Application.CQRS.Users.DTOs;
using AutoMapper;
using AutoMapper.Internal.Mappers;
using Common.Exceptions;
using Common.GlobalResponse.Generics;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Users.Handlers;

public class GetById
{
    public record struct Query : IRequest<ResponseModel<GetByIdDto>>
    {
        public int Id { get; set; }

    }

    public sealed class Handler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<Query, ResponseModel<GetByIdDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<ResponseModel<GetByIdDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var currentUser = await _unitOfWork.UserRepository.GetByIdAsync(request.Id);
            if (currentUser == null)
            {
                //return new ResponseModel<GetByIdDto>() { Data = null, Errors = ["User is not exist"], IsSuccess = false };
                throw new BadRequestException("User is not exist");
            }

            //GetByIdDto reponse = new()
            //{
            //    Id = currentUser.Id,
            //    Email = currentUser.Email,
            //    Name = currentUser.Name,
            //    Phone = currentUser.Phone,
            //    Surname = currentUser.Surname,
            //};

            var mappedResponse = _mapper.Map<GetByIdDto>(currentUser);


            return new ResponseModel<GetByIdDto> { Data = mappedResponse, Errors = [], IsSuccess = true };
        }
    }
}
