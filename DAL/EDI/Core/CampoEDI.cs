using System;

namespace DAL.EDI.Core {

    public class CampoEDI {

        public int id { get; set; }

        public string nomeCampo { get; set; }

        public string descricaoCampo { get; set; }

        public short idTipoCampo { get; set; }
        
        public bool? flagObrigatorio { get; set; }

        public short? minCaracteres { get; set; }

        public short? maxCaracteres { get; set; }

        public string codigoLinha { get; set; }

        public int? nroColuna { get; set; }

        public int? posicaoInicio { get; set; }

        public int? posicaoFim { get; set; }

        public string valorFixo { get; set; }

        public string valorDinamico { get; set; }

        public object valor { get; set; }

        //tratamento para retornar o valor em formato string
        public string valorToString() {
            
            return valor.stringOrEmpty();
        }

        //tratamento para retornar o valor em formato string
        public string valorToStringUpper() {
            
            return valor.stringOrEmptyUpper();
        }

        //tratamento para retornar o valor em formato string
        public string valorToStringLowe() {
            
            return valor.stringOrEmptyLower();
        }

        //tratamento para retornar valor em formato int
        public int valorToInt() {

            return valor.toInt();
        }

        /// <summary>
        /// Se for um campo definido por posicao, calcular o tamanho da informacao
        /// </summary>
		private int getTamanho() {
            return (posicaoFim.toInt() - posicaoInicio.toInt()) + 1;
        }

        /// <summary>
        /// Se for um campo definido por posicao, calcular posicao inico
        /// </summary>
		private int getStartIndex() {
            return (this.posicaoInicio.toInt() - 1);
        }

        /// <summary>
        /// Se for um campo definido por posicao, calcular posicao fim
        /// </summary>
		private int getStopIndex() {
            return (this.posicaoFim.toInt() - 1);
        }

        /// <summary>
        /// Se for um campo definido por posicao, definir o valor com base nas posicoes
        /// </summary>
		public void setValorLinha(string line) {

            if(line.isEmpty()) {
                return;
            }

            if(!string.IsNullOrEmpty(this.valorFixo)) {

                this.valor = valorFixo;

                return;
            }

            try {
                
                if(line.Length < this.getStartIndex()) {

                    this.valor = String.Empty;

                    return;

                } 

                string valorLinha = line.Substring(this.getStartIndex());

                if(valorLinha.Length < this.getTamanho()) {

                    this.valor = line.Substring(0, valorLinha.Length).Trim();

                    return;

                } 

                this.valor = line.Substring(this.getStartIndex(), this.getTamanho()).Trim();

            } catch(ArgumentOutOfRangeException ex) {
                
                string obs = String.Concat("linha: ", line, "\n", this.getStartIndex(), "-", this.getTamanho());
                
                UtilLog.saveError(ex, obs);
                
                this.valor = line.Substring(this.getStartIndex(), (line.Length - this.getStartIndex()));
            }
        }

        /// <summary>
        /// Ajustar o valor da informação para criar a linha de ocorrencia
        /// </summary>
		public string buildValorParaLinha() {

            string value = !string.IsNullOrEmpty(this.valorFixo) ? valorFixo.stringOrEmptyUpper() : this.valor.stringOrEmptyUpper();

            value = value.removerAcentuacao();

            if(this.idTipoCampo == TipoCampoEDIConst.NUMERICO) {

                var padLeft = value.PadLeft(this.getTamanho(), '0');

                value = padLeft;

            } else {

                var padRight = value.PadRight(this.getTamanho(), ' ');

                value = padRight;

            }


            if(value.Length > this.getTamanho()) {

                value = value.Substring(0, this.getTamanho());

            }

            return value;
        }
    }

}
