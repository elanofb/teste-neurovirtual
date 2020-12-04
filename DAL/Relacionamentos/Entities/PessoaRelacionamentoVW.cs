using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Relacionamentos {

    public class PessoaRelacionamentoVW {

	    public int id { get; set; }
	    
	    public int idPessoa { get; set; }
	    
	    public int? idOrganizacao { get; set; }
	    
	    public int? idUnidade { get; set; }
	    
	    public string nomePessoa { get; set; }
	    
	    public int idOcorrenciaRelacionamento { get; set; }
	    
	    public string descricaoTipoOcorrencia { get; set; }
	    
	    public DateTime? dtOcorrencia { get; set; }
	    
	    public string observacao { get; set; }
	    
	    public DateTime? dtCadastroOcorrencia { get; set; }
	    
	    public int? idUsuarioCadastro { get; set; }
	    
	    public string nomeUsuarioCadastro { get; set; }
	    
	    public bool flagPossuiArquivo { get; set; }
	    
    }

	internal sealed class PessoaRelacionamentoVWMapper : EntityTypeConfiguration<PessoaRelacionamentoVW> {

		public PessoaRelacionamentoVWMapper() {
			
			this.ToTable("vw_pessoa_relacionamento");
			
			this.HasKey(o => o.id);

			this.Ignore(o => o.flagPossuiArquivo);
			
		}
	}
}