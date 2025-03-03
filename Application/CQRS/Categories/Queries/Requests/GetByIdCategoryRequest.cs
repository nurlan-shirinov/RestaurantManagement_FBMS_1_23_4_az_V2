using Application.CQRS.Categories.Queries.Responses;
using Common.GlobalResponse.Generics;
using MediatR;

namespace Application.CQRS.Categories.Queries.Requests;

public class GetByIdCategoryRequest:IRequest<ResponseModel<GetByIdCategoryResponse>>
{
    public int Id { get; set; }
}
