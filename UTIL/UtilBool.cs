
namespace System {

    public static class UtilConvert {

        //
        public static bool? toBool(string str) {

            if (string.IsNullOrEmpty(str)) {
                return null;
            }

            bool retorno;

            if (Boolean.TryParse(str, out retorno)) {
                return retorno;
            }

            return null;
        }

    }
}
