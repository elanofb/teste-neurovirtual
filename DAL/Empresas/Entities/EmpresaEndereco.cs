using System.Data.Entity.ModelConfiguration;
using DAL.Enderecos;

namespace DAL.Empresas {
	/**
	*
	*/

	public class EmpresaEndereco : Endereco {

		public int idEmpresa { get; set; }

		public int idTipoEndereco { get; set; }
	}

	/**
	*
	*/

	internal sealed class EmpresaEnderecoMapper : EntityTypeConfiguration<EmpresaEndereco> {

		public EmpresaEnderecoMapper() {
			this.ToTable("tb_empresa_endereco");
			this.HasKey(o => o.id);
			//FKs
			this.HasRequired(u => u.Cidade).WithMany().HasForeignKey(o => o.idCidade);
		}
	}
}