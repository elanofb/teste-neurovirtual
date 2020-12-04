using System.Data.Entity.ModelConfiguration;

namespace DAL.RedeAfiliados {

    public class RedeBinariaEsquerdaVW : RedeBinariaBase {

        public override bool flagDireita => false;
        
        public override bool flagEsquerda => true;
    }
    

    /// <summary>
    /// 
    /// </summary>
    internal sealed class RedeBinariaEsquerdaVWMapper : EntityTypeConfiguration<RedeBinariaEsquerdaVW> {

        public RedeBinariaEsquerdaVWMapper() {

            this.ToTable("vw_rede_binaria_esquerda");

            this.HasKey(o => o.idMembro);

            this.Ignore(x => x.flagEsquerda);
            
            this.Ignore(x => x.flagDireita);

        }
    }    

}
