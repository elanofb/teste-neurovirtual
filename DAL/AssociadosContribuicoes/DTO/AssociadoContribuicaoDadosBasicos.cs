using System;

namespace DAL.AssociadosContribuicoes.DTO {

    public class AssociadoContribuicaoDadosBasicos {

        public int id { get; set; }

        public int idContribuicao { get; set; }

        public string descricaoContribuicao { get; set; }

        public int idAssociado { get; set; }

        public int idPessoa { get; set; }

        public string nomeAssociado { get; set; }

        public bool? flagCobrancaGerada { get; set; }

        public bool? flagIsento { get; set; }

        public int? idTipoAssociado { get; set; }

        public string descricaoTipoAssociado { get; set; }

        public DateTime? dtVencimentoOriginal { get; set; }

        public DateTime? dtVencimentoAtual { get; set; }

        public DateTime? dtPagamento { get; set; }

        public decimal? valorOriginal { get; set; }

        public decimal? valorAtual { get; set; }

    }
}
