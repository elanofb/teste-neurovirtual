using DAL.Entities;
using DAL.Permissao;
using DAL.Pessoas;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.MateriaisApoio {
    public class MaterialApoioPessoa : DefaultEntity {

        public int idMaterialApoio { get; set; }
        public virtual MaterialApoio MaterialApoio { get; set; }

        public int idPessoa { get; set; }
        public virtual Pessoa Pessoa { get; set; }

    }


    public class MaterialApoioPessoaMapper : EntityTypeConfiguration<MaterialApoioPessoa> {
        public MaterialApoioPessoaMapper() {
            this.ToTable("tb_material_apoio_pessoa");
            this.HasKey(x => x.id);
            this.HasRequired(x => x.MaterialApoio).WithMany(x => x.listaPessoasPermitidas).HasForeignKey(x => x.idMaterialApoio);
            this.HasRequired(x => x.Pessoa).WithMany().HasForeignKey(x => x.idPessoa);
        }
    }
}
