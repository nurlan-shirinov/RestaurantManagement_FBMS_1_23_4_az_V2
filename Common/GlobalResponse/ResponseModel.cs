namespace Common.GlobalResponse;

public class ResponseModel
{
	public bool IsSuccess { get; set; }
	public List<string> Errors { get; set; }

	public ResponseModel(List<string> messages) 
	{
		Errors = messages;
		IsSuccess = false;
	}

	public ResponseModel() 
	{
		IsSuccess = true;
		Errors = null;
	}
}
