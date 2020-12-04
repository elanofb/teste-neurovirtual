using System;

namespace DAL.Atendimentos {

	public class AtendimentoStatusConst {

        public static readonly int EM_ABERTO = 1;

        public static readonly int EM_ATENDIMENTO = 2;

        public static readonly int AGUARDANDO_RETORNO = 3;

        public static readonly int RETORNO_REALIZADO = 4;

	    public static readonly int PENDENTE = 5;

	    public static readonly int FINALIZADO = 6;

    }
}