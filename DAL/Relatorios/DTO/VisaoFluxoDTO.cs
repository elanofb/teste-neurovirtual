namespace DAL.Relatorios {

	public class VisaoFluxoDTO {
		public int ano { get; set; }
		public int mes { get; set; }
        public int tipo { get; set; }
		public decimal valorReceita { get; set; }
        public decimal valorDespesa { get; set; }
	}
}