using System.IO;
using System.Threading.Tasks;
using System.Net;

namespace BLL.Request {
	
	public interface IRequestAsync {

		Task<TextReader> doRequestAsync(WebRequest OWebRequest);

        Task<TextReader> doRequestAsync(string url);

        Task<TextReader> postRequestAsync(string url, byte[] data);
	}
}
