namespace Application.CQRS.Categories.Queries.Responses;

public class GetByIdCategoryResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedDate { get; set; }
}