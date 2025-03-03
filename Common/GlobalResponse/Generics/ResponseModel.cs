using Common.GlobalResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.GlobalResponse.Generics;

public class ResponseModel<T> : ResponseModel
{
	public T? Data { get; set; }

	public ResponseModel(List<string> message) : base(message)
	{

	}

	public ResponseModel()
	{
		
	}


}
