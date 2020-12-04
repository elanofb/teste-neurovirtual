
namespace System {

    public static class ObjectExtensions {

        //Exibicao das datas
        public static object getValueByString<T>(this T source, string key) {

            return source.GetType()
                         .GetProperty(key)
                        ?.GetValue(source, null);
            
        }


    }
}