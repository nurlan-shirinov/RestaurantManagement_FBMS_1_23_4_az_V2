namespace Application.CQRS.Categories.Commands.Responses;

public record struct DeleteCategoryResponse
{
    public string Message { get; set; }
}