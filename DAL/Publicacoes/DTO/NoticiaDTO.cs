using System;

namespace DAL.Publicacoes {

	[Serializable]
	public class NoticiaDTO {

		public int id { get; set; }

		public string titulo { get; set; }

		public string autor { get; set; }

		public DateTime? dtNoticia { get; set; }

		public string chamada { get; set; }

		public string descricao { get; set; }

		//
		public string path { get; set; }

		public string pathThumb { get; set; }
	}
}