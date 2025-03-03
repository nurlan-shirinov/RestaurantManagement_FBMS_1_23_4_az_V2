using Application.CQRS.Categories.Commands.Requests;
using Application.CQRS.Categories.Commands.Responses;
using AutoMapper;
using Common.GlobalResponse.Generics;
using Domain.Entites;
using FluentValidation;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Categories.Handlers.CommandHandlers;

public class CreateCategoryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateCategoryRequest, ResponseModel<CreateCategoryResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<ResponseModel<CreateCategoryResponse>> Handle(CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        var mappedRequest = _mapper.Map<Category>(request);

        await _unitOfWork.CategoryRepository.AddAsync(mappedRequest);

        var response = _mapper.Map<CreateCategoryResponse>(mappedRequest);

        return new ResponseModel<CreateCategoryResponse>
        {
            Data = response,
            Errors = [],
            IsSuccess = true
        };

    }
}
