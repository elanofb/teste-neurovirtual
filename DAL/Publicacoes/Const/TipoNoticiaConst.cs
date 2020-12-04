using System;

namespace DAL.Publicacoes {

	public class TipoNoticiaConst {
		public static int NOTICIA = Convert.ToInt32(TipoNoticiaEnum.NOTICIA);
		public static int COMUNICADO = Convert.ToInt32(TipoNoticiaEnum.COMUNICADO);
		public static int VAGAESTAGIO = Convert.ToInt32(TipoNoticiaEnum.VAGAESTAGIO);
		public static int INICIACAO_CIENTIFICA = Convert.ToInt32(TipoNoticiaEnum.INICIACAO_CIENTIFICA);
		public static int ARTIGO = Convert.ToInt32(TipoNoticiaEnum.ARTIGO);
		public static int REVISTA = Convert.ToInt32(TipoNoticiaEnum.REVISTA);
        public static int TESE = Convert.ToInt32(TipoNoticiaEnum.TESE);
        public static int PODCAST = Convert.ToInt32(TipoNoticiaEnum.PODCAST);
        public static int JORNAL = Convert.ToInt32(TipoNoticiaEnum.JORNAL);
	}
}