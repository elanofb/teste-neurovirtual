namespace DAL.ImportacoesErros {

    public class MensagensErroConst {
        
        public static string campoObrigatorio(string nomeCampo = "valor") {
            return $"O(a) { nomeCampo } deve ser informado(a)";
        }

        public static string campoInvalido(string nomeCampo = "valor") {
            return $"O(a) { nomeCampo } informado(a) é inválido(a)";
        }

        public static string minimoCaracteres(int numeroMinimo, string nomeCampo = "valor") {
            return $"O(a) { nomeCampo } deve conter no mínimo { numeroMinimo } caracteres";
        }

        public static string maximoCaracteres(int numeroMaximo, string nomeCampo = "valor") {
            return $"O(a) { nomeCampo } pode conter no máximo { numeroMaximo } caracteres";
        }

        public static string intervaloTamanhoCaracteres(int numeroMinimo, int numeroMaximo, string nomeCampo = "valor") {
            return $"O(a) { nomeCampo } deve conter entre { numeroMinimo } e { numeroMaximo } caracteres";
        }

    }

}
