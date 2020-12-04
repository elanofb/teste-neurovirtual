using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace DAL.RedeAfiliados {

	//
	public class RedeLinearVW {
		
		public int  idMembro       { get; set; }
		
		public int? nroMembro      { get; set; }
		public byte? idTipoCadastroMembro { get; set; }
		public string nomeMembro { get; set; }
		public string nroDocumentoMembro { get; set; }
		public int? idPlanoCarreiraMembro { get; set; }
		
		public int? idIndicador01  { get; set; }
		public int? nroIndicador01 { get; set; }
		public int? idIndicador02  { get; set; }
		public int? nroIndicador02 { get; set; }
		public int? idIndicador03  { get; set; }
		public int? nroIndicador03 { get; set; }
		public int? idIndicador04  { get; set; }
		public int? nroIndicador04 { get; set; }
		public int? idIndicador05  { get; set; }
		public int? nroIndicador05 { get; set; }
		public int? idIndicador06  { get; set; }
		public int? nroIndicador06 { get; set; }
		public int? idIndicador07  { get; set; }
		public int? nroIndicador07 { get; set; }
		public int? idIndicador08  { get; set; }
		public int? nroIndicador08 { get; set; }
		public int? idIndicador09  { get; set; }
		public int? nroIndicador09 { get; set; }
		public int? idIndicador10  { get; set; }
		public int? nroIndicador10 { get; set; }
		public int? idIndicador11  { get; set; }
		public int? nroIndicador11 { get; set; }
		public int? idIndicador12  { get; set; }
		public int? nroIndicador12 { get; set; }
		public int? idIndicador13  { get; set; }
		public int? nroIndicador13 { get; set; }
		public int? idIndicador14  { get; set; }
		public int? nroIndicador14 { get; set; }
		public int? idIndicador15  { get; set; }
		public int? nroIndicador15 { get; set; }
		
	}

	/// <summary>
	/// 
	/// </summary>
	internal sealed class RedeLinearVWMapper : EntityTypeConfiguration<RedeLinearVW> {

		public RedeLinearVWMapper() {

			this.ToTable("vw_rede_linear");

			this.HasKey(o => o.idMembro);


		}
	}
}

