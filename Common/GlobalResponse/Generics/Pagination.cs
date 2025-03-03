namespace Common.GlobalResponse.Generics;

public class Pagination<T>
{
	public List<T> Data { get; set; }
	public int TotalDataCount { get; set; }




	public Pagination(List<T> data, int totalDataCount)
	{
		Data = data;
		TotalDataCount = totalDataCount;
	}

	public Pagination() 
	{
		Data = [];
		TotalDataCount = 0;

	}
}
