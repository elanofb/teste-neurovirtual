using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Emails;

namespace DAL.Pessoas {

	//
	[Serializable]
	public class PessoaEmailResumoDTO  {
		
        public string email { get; set; }
		
		public string tipoEmail { get; set; }		
	}
}