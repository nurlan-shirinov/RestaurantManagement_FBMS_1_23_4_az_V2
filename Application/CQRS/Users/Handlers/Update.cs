using Application.CQRS.Users.DTOs;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponse;
using Common.GlobalResponse.Generics;
using MediatR;
using Repository.Common;
using System.Numerics;
using System.Runtime.InteropServices.Marshalling;
using Common.GlobalResponse.Generics;

namespace Application.CQRS.Users.Handlers;

public class Update
{
    public record struct UpdateCommand:IRequest<ResponseModel<UpdateDto>>
    {
        public int Id {  get; set; }
        public string Name { get; set; }
        public string Surname {  get; set; } 
        public string Email { get; set; }
        public string Phone { get; set; }


    }


    public sealed class Handler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateCommand, ResponseModel<UpdateDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task<ResponseModel<UpdateDto>> Handle(UpdateCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _unitOfWork.UserRepository.GetByIdAsync(request.Id);
            if (currentUser == null) 
            {
                throw new BadRequestException("User does not exist with provided id");
            }

            currentUser.Name = request.Name;
            currentUser.Surname = request.Surname;
            currentUser.Email = request.Email;
            currentUser.Phone = request.Phone;

            _unitOfWork.UserRepository.Update(currentUser);
            await _unitOfWork.SaveChanges();

            var response = _mapper.Map<UpdateDto>(currentUser);

            return new ResponseModel<UpdateDto>
            {
                Data = response,
                Errors = [],
                IsSuccess = true
            };


        }
    }
}
