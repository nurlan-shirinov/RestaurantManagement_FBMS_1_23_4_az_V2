using Application.CQRS.Categories.Commands.Responses;
using Common.GlobalResponse.Generics;
using MediatR;

namespace Application.CQRS.Categories.Commands.Requests;

public class CreateCategoryRequest : IRequest<ResponseModel<CreateCategoryResponse>>
{
    public string Name { get; set; }
}