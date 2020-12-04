using System;
using System.Collections;
using System.Linq.Expressions;
using FastMember;

namespace UTIL.Extensions {

    public static class FastMemberExtensions {

        public static void assignValueToProperty(this ObjectAccessor accessor, string property, object value) {

            string propertyName = property;

            var index = propertyName.IndexOf('.');

            int indexLista = propertyName.IndexOf('[');

            //Se for um item com posicao de array
            if (indexLista > -1 && (indexLista < index || index == -1)) {

                string subPropName = propertyName.Substring(0, indexLista);

                int indiceItemLista = propertyName.Substring(indexLista).onlyNumber().toInt();

                var listaProperty = accessor[subPropName] as IList;

                if (listaProperty != null && listaProperty.Count > indiceItemLista) {

                    if (index == -1) {

                        listaProperty[indiceItemLista] = value;

                        accessor[subPropName] = listaProperty;

                        return;
                    }

                    accessor = ObjectAccessor.Create(listaProperty[indiceItemLista]);

                    assignValueToProperty(accessor, propertyName.Substring(index + 1), value);

                    return;
                }

            }

            //Se nao tiver mais subobjetos para acessar
            if (index == -1) {

                var targetType = Expression.Parameter(accessor.Target.GetType());

                var targetProperty = Expression.Property(targetType, propertyName);

                var type = targetProperty.Type;

                type = Nullable.GetUnderlyingType(type) ?? type;

                value = value == null ? getDefaulByTypet(type) : Convert.ChangeType(value, type);

                accessor[propertyName] = value;

                return;
            }

            string propName = propertyName.Substring(0, index);

            accessor = ObjectAccessor.Create(accessor[propName]);

            assignValueToProperty(accessor, propertyName.Substring(index + 1), value);

        }

        /// <summary>
        /// 
        /// </summary>
        public static object getValueProperty(this ObjectAccessor accessor, string property) {

            string propertyName = property;

            var index = propertyName.IndexOf('.');

            int indexLista = propertyName.IndexOf('[');

            //Se for um item com posicao de array
            if (indexLista > -1 && (indexLista < index || index == -1)) {

                string subPropName = propertyName.Substring(0, indexLista);

                int indiceItemLista = propertyName.Substring(indexLista).onlyNumber().toInt();

                var listaProperty = accessor[subPropName] as IList;

                if (listaProperty != null && listaProperty.Count > indiceItemLista) {

                    if (index == -1){

                        return listaProperty[indiceItemLista];
                    }

                    accessor = ObjectAccessor.Create(listaProperty[indiceItemLista]);

                    return getValueProperty(accessor, propertyName.Substring(index + 1));

                }

                return "";
            }

            if (accessor == null) {

                return string.Empty;

            }

            if (index == -1) {

                return accessor[propertyName];

            }

            string propName = propertyName.Substring(0, index);

            indexLista = propName.IndexOf('[');

            if (indexLista > -1) {

                string subPropName = propName.Substring(0, indexLista);

                int indiceItemLista = propName.Substring(indexLista).onlyNumber().toInt();

                var listaProperty = accessor[subPropName] as IList;

                if (listaProperty != null && listaProperty.Count > 0 && listaProperty.Count > indiceItemLista) {

                    accessor = ObjectAccessor.Create(listaProperty[indiceItemLista]);

                } else {

                    return string.Empty;
                }


            } else {

                var itemAccessor = accessor[propName];

                if (itemAccessor == null) {

                    return string.Empty;

                }

                accessor = ObjectAccessor.Create(itemAccessor);

            }

            return getValueProperty(accessor, propertyName.Substring(index + 1));

        }

        private static object getDefaulByTypet(Type type) {

            return type.IsValueType ? Activator.CreateInstance(type) : null;

        }
    }
}
