using System.Data.Entity.ModelConfiguration;

namespace DAL.ConfiguracoesAssociados {

    public class FuncaoFiltro {

        public short id { get; set; }

        public string descricao { get; set; }

        public string nomeFuncao { get; set; }

        public bool? ativo { get; set; }
    }


    //
    internal sealed class FuncaoFiltroMapper : EntityTypeConfiguration<FuncaoFiltro> {

        public FuncaoFiltroMapper() {

            this.ToTable("datatb_funcao_filtro");

            this.HasKey(x => x.id);
        }
    }
}
