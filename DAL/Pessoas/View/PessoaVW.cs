using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Pessoas {

    //
    public class PessoaVW {

        public Guid id { get; set; }

        public int? idOrganizacao { get; set; }

        public int? idUnidade { get; set; }

        public int idReferencia { get; set; }

        public int idPessoa { get; set; }

        public string flagTipoPessoa { get; set; }

        public string nroDocumento { get; set; }

        public string nome { get; set; }

        public string razaoSocial { get; set; }
        
        public string telefone { get; set; }
        
        public string email { get; set; }

        public string flagCategoriaPessoa { get; set; }

        public string descricaoCategoriaPessoa { get; set; }

        public PessoaVW() {
        }
    }

    //
    internal sealed class PessoaVWMapper : EntityTypeConfiguration<PessoaVW> {

        public PessoaVWMapper() {

            this.ToTable("vw_pessoa");
            this.HasKey(o => o.id);
        }
    }
}