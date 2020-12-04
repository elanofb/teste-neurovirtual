using System;

namespace DAL.Documentos {

	public class TipoDocumentoConst {
		public static readonly int CPF = Convert.ToInt32(TipoDocumentoEnum.CPF);
		public static readonly int RG = Convert.ToInt32(TipoDocumentoEnum.RG);
		public static readonly int PASSAPORTE = Convert.ToInt32(TipoDocumentoEnum.PASSAPORTE);
		public static readonly int CNPJ = Convert.ToInt32(TipoDocumentoEnum.CNPJ);
	}
}