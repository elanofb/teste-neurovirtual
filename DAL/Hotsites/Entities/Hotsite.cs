using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Eventos;
using DAL.Permissao;

namespace DAL.Hotsites {
	
    //
    public class Hotsite {

        public int id { get; set; }
	    
        public int? idOrganizacao { get; set; }
	    
        public string tituloPagina { get; set; }
	    
        public bool? flagContagemRegressiva { get; set; }
	    
        public int? idEventoGaleriaFoto { get; set; }
	    
	    //public virtual Evento EventoGaleriaFoto { get; set; }
	    
        public string conteudoApresentacao { get; set; }
	    
        public int idEvento { get; set; }
	    
        //public virtual Evento Evento { get; set; }
	    
        public string dominios { get; set; }
	    
	    public string cssFormatacao { get; set; }
	    
	    public string scriptJS { get; set; }
	    
	    public string scriptAdicional { get; set; }
	    
	    public int? idArquivoBannerPrincipal { get; set; }
	    
        public DateTime dtCadastro { get; set; }
	    
        public int idUsuarioCadastro { get; set; }
	    
	    public virtual UsuarioSistema UsuarioCadastro { get; set; }
	    
        public bool? ativo { get; set; }
	    
        public DateTime? dtAlteracao { get; set; }
	    
        public int? idUsuarioAlteracao { get; set; }
	    
        public int? idUsuarioExclusao { get; set; }
	    
        public DateTime? dtExclusao { get; set; }
	    
	    public string htmlApresentacao { get; set; }
	    
	    public string htmlRodape { get; set; }
	}

	/**
	*
	*/

	internal sealed class HotsiteMapper : EntityTypeConfiguration<Hotsite> {

		public HotsiteMapper() {

			this.ToTable("tb_hotsite");
			
			this.HasKey(o => o.id);

			this.HasRequired(x => x.UsuarioCadastro).WithMany().HasForeignKey(x => x.idUsuarioCadastro);

		}
	}
}