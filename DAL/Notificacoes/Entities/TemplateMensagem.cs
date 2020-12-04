using DAL.Permissao;
using DAL.Pessoas;
using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Organizacoes;

namespace DAL.Notificacoes {

    public class TemplateMensagem {

		public int id { get; set; }
	    
		public int? idOrganizacao { get; set; }
	    
		public string titulo { get; set; }
	    
		public string corpoHTML { get; set; }
	    
		public string corpoTexto { get; set; }
	    
		public bool? ativo { get; set; }
	    
		public DateTime dtCadastro { get; set; }
	    
		public int idUsuarioCadastro { get; set; }
	    
		public DateTime? dtExclusao { get; set; }
	    
		public int? idUsuarioExclusao { get; set; }

    }


    public class TemplateMensagemMapper : EntityTypeConfiguration<TemplateMensagem> {

        public TemplateMensagemMapper() {

            this.ToTable("tb_template_mensagem");

            this.HasKey(x => x.id);

        }
    }
}
