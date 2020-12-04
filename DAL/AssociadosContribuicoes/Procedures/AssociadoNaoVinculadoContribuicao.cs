using System;

namespace DAL.AssociadosContribuicoes.Procedures {

    public class AssociadoNaoVinculadoContribuicao {

        public int idAssociado { get; set; }

        public int idPessoa { get; set; }

        public int? idTipoAssociado { get; set; }

        public int? idContribuicaoPadrao { get; set; }

        public DateTime? dtAdmissao { get; set; }

        public DateTime? dtUltimaContribuicao { get; set; }

    }
}
