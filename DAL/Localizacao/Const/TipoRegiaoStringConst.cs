namespace DAL.Localizacao {
	public static class TipoRegiaoStringConst {

		public static readonly string CAPITAL = "Capital";
		public static readonly string INTERIOR_1 = "Interior I";
		public static readonly string INTERIOR_2 = "Interior II";
		public static readonly string METROPOLITANO = "Metropolitana";

        public static byte getTipoRegiaoIntConst(string descTipoRegiao) {

            if (descTipoRegiao.Equals(CAPITAL)) { 
                return TipoRegiaoConst.CAPITAL;
            }

            if (descTipoRegiao.Equals(INTERIOR_1)) { 
                return TipoRegiaoConst.INTERIOR_1;
            }

            if (descTipoRegiao.Equals(INTERIOR_2)) { 
                return TipoRegiaoConst.INTERIOR_2;
            }

            if (descTipoRegiao.Equals(METROPOLITANO)) { 
                return TipoRegiaoConst.METROPOLITANO;
            }

            return 0;
        }

	}
}
