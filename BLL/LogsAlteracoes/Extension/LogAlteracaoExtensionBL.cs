using System;

namespace BLL.LogsAlteracoes {

    public static class LogAlteracaoExtensionBL {
        
        //Carregar registro a partir do ID
        public static string alterarValorCampo<T>(this T item, string nomeCampo, string novoValor) {

            try {

                var classType = item.GetType();

                var field = classType.GetProperty(nomeCampo);

                if (field == null) {
                    return null;
                }

                var valorAntigo = field.GetValue(item, null)?.ToString() ?? "";

                if (novoValor.isEmpty()) {
                    field.SetValue(item, null, null);

                    return valorAntigo;
                }

                Type t = Nullable.GetUnderlyingType(field.PropertyType) ?? field.PropertyType;

                var valor = Convert.ChangeType(novoValor, t);
                field.SetValue(item, valor, null);

                return valorAntigo;

            } catch (Exception ex) {
                UtilLog.saveError(ex, "");
                return null;
            }
        }

        public static string getValorCampo<T>(this T item, string nomeCampo) {

            try {

                var classType = item.GetType();

                var field = classType.GetProperty(nomeCampo);

                if (field == null) {
                    return null;
                }

                var valor = field.GetValue(item, null)?.ToString() ?? "";

                return valor;

            } catch (Exception ex) {
                UtilLog.saveError(ex, "");
                return null;
            }
        }
    }
}