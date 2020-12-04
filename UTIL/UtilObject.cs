using System;
using System.Collections;
using System.Reflection;

namespace System {

    public class UtilObject {

        //Atributos
        private static UtilObject _instance;

        //Propriedades
        public static UtilObject getInstance => _instance = _instance ?? new UtilObject();


        /// <summary>
        /// Configurar atributos de um objeto com base no caminho informado
        /// </summary>
        public void setDeepProperty(object instance, string path, object value) {

            var pp = path.Split('.');

            Type t = instance.GetType();

            PropertyInfo propInfo = null;

            for (int i = 0; i < pp.Length; i++) {

                string prop = pp[i];

                string propName = prop;

                int indexLista = prop.IndexOf('[');

                if (indexLista > -1) {

                    propName = prop.Substring(0, indexLista);

                }

                propInfo = t.GetProperty(propName);

                if (propInfo == null) {

                    continue;

                }

                //Se o loop chegou ao fim, não é necessário buscar os dados abaixo
                if ((i + 1) == pp.Length) {

                    break;

                }

                instance = propInfo.GetValue(instance, null);

                t = propInfo.PropertyType;

                if (instance is ICollection) {

                    int posicaoItemLista = prop.Substring(indexLista).onlyNumber().toInt();

                    var list = instance as IList;

                    if (list != null) {

                        instance = list[posicaoItemLista];

                        t = instance.GetType();
                    }
                }
            }

            if (instance == null || propInfo == null) {

                return;

            }

            Type paramType = Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType;

            if (paramType == typeof(int) && value.isEmpty()) {

                value = "0";
            }

            var valueConverted = Convert.ChangeType(value, paramType);

            propInfo.SetValue(instance, valueConverted);

        }
    }
}
