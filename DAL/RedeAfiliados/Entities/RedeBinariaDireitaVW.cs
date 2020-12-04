using System.Data.Entity.ModelConfiguration;

namespace DAL.RedeAfiliados {

    public class RedeBinariaDireitaVW : RedeBinariaBase{
        
        public override bool flagDireita => true;
        
        public override bool flagEsquerda => false;
        
    }
    

    /// <summary>
    /// 
    /// </summary>
    internal sealed class RedeBinariaDireitaVWMapper : EntityTypeConfiguration<RedeBinariaDireitaVW> {

        public RedeBinariaDireitaVWMapper() {

            this.ToTable("vw_rede_binaria_direita");

            this.HasKey(o => o.idMembro);

            this.Ignore(x => x.flagEsquerda);
            
            this.Ignore(x => x.flagDireita);
        }
    }    

}
