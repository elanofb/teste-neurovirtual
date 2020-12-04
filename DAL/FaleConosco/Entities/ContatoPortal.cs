using System.Data.Entity.ModelConfiguration;
using DAL.Entities;

namespace DAL.FaleConosco {

    public class ContatoPortal {
        
        public int idOrganizacao { get; set; }

        public string nome { get; set; }

        public string email { get; set; }

        public string telefone { get; set; }

        public string assunto { get; set; }

        public string mensagem { get; set; }

    }

    //internal sealed class TipoContatoMapper : EntityTypeConfiguration<TipoContato> {

    //	public TipoContatoMapper() {
    //		this.ToTable("tb_tipo_contato");
    //		this.HasKey(o => o.id);
    //	}
    //}
}