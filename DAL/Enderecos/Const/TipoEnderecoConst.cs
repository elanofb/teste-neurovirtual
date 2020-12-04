using System;

namespace DAL.Enderecos {

	public class TipoEnderecoConst {
		public static readonly byte PRINCIPAL = Convert.ToByte(TipoEnderecoEnum.PRINCIPAL);
		public static readonly byte COMERCIAL = Convert.ToByte(TipoEnderecoEnum.COMERCIAL);
		public static readonly byte RESIDENCIAL = Convert.ToByte(TipoEnderecoEnum.RESIDENCIAL);
		public static readonly byte HOSPITAL = Convert.ToByte(TipoEnderecoEnum.HOSPITAL);
	}
}