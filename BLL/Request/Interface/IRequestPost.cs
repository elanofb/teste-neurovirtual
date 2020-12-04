using System.Collections.Specialized;
using System.IO;
using System.Threading.Tasks;
using System.Net;

namespace BLL.Request {
	
	public interface IRequestPost {

        string postRequest(string url, byte[] data, NameValueCollection extraHeaders = null);
	}
}
