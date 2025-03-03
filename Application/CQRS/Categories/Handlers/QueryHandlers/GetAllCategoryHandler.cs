using Application.CQRS.Categories.Queries.Requests;
using Application.CQRS.Categories.Queries.Responses;
using Common.GlobalResponse.Generics;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Categories.Handlers.QueryHandlers;

public class GetAllCategoryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllCategoryRequest, ResponseModelPagination<GetAllCategoryResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ResponseModelPagination<GetAllCategoryResponse>> Handle(GetAllCategoryRequest request, CancellationToken cancellationToken)
    {
        var categories = _unitOfWork.CategoryRepository.GetAll();

        if (!categories.Any())
        {
            return new ResponseModelPagination<GetAllCategoryResponse>() { Data = null, Errors = [],IsSuccess=true };
        };


        var totalCount = categories.Count();
        categories = categories.Skip((request.Page - 1) * request.Limit).Take(request.Limit);
        var mappedCategories = new List<GetAllCategoryResponse>();
        foreach (var category in categories)
        {
            var mapped = new GetAllCategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                CreatedDate = category.CreatedDate??DateTime.MinValue,
                DeletedDate = category.DeletedDate ?? DateTime.MinValue,
                UpdatedDate = category?.UpdatedDate ?? DateTime.MinValue,
            };
            mappedCategories.Add(mapped);
        }

        var response = new Pagination<GetAllCategoryResponse>() { Data = mappedCategories,TotalDataCount = totalCount };

        return new ResponseModelPagination<GetAllCategoryResponse>
        {
            Data = response,
            Errors = [],
        };
    }
}