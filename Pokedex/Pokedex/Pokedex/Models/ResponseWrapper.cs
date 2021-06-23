using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Pokedex.Models
{
	public class ResponseWrapper
	{
		public HttpStatusCode StatusCode { get; set; }

		public string Content { get; set; }

		public string ReasonPhrase { get; internal set; }
	}
}
