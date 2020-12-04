using System.Collections.Specialized;
using System.IO;
using System.Net;

namespace BLL.Request {
	
	public interface IRequestGet {

		string doRequest(string url, NameValueCollection extraHeaders = null);
	}
}
