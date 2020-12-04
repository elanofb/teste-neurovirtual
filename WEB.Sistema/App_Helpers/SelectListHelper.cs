using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using DAL.Permissao.Security.Extensions;

namespace WEB.Helpers {

    public static class SelectListHelper {

        //Gerar o Html do Link
        public static SelectList selectListDinamico(string namespaceHelper, string nameMethod, string parametros, object selected, NameValueCollection dados) {

            nameMethod = string.IsNullOrEmpty(nameMethod) ? "selectList" : nameMethod;

            Type helper = Type.GetType(namespaceHelper);

            MethodInfo methodHelper = helper?.GetMethod(nameMethod);

            if (methodHelper == null) {
                return null;
            }

            var listaParametros = parametrosMetodo(methodHelper, parametros, selected, dados);

            MethodInfo methodGetInstance = helper?.GetMethod("get_getInstance");

            if (methodGetInstance == null) {

                return methodHelper.Invoke(null, listaParametros?.ToArray()) as SelectList;
            }

            return methodHelper.Invoke(methodGetInstance.Invoke(null, null), listaParametros?.ToArray()) as SelectList;
        }

        //Gerar o Html do Link
        public static MultiSelectList multiSelectListDinamico(string namespaceHelper, string nameMethod, string parametros, object selected, NameValueCollection dados) {

            nameMethod = string.IsNullOrEmpty(nameMethod) ? "multiSelectList" : nameMethod;

            Type helper = Type.GetType(namespaceHelper);

            MethodInfo methodHelper = helper?.GetMethod(nameMethod);

            if (methodHelper == null) {
                return null;
            }

            var listaParametros = parametrosMetodo(methodHelper, parametros, selected, dados);

            MethodInfo methodGetInstance = helper?.GetMethod("get_getInstance");

            if (methodGetInstance == null) {

                return methodHelper.Invoke(null, listaParametros?.ToArray()) as MultiSelectList;
            }

            return methodHelper.Invoke(methodGetInstance.Invoke(null, null), listaParametros?.ToArray()) as MultiSelectList;
        }

        /// <summary>
        /// Capturar a lista de parametros de um método
        /// </summary>
        public static List<object> parametrosMetodo(MethodInfo OMetodo, string parametros, object selected, NameValueCollection dados) {

            if (parametros.isEmpty()) {
                return null;
            }

            var paramsEnviados = parametros.Split(',').ToArray();

            ParameterInfo[] paramsMethod = OMetodo.GetParameters();

            if (paramsMethod.Length == 0) {

                return null;
            }

            List<object> listaParametros = new List<object>();

            for (int i = 0; i < paramsMethod.Length; i++) {

                var ParamInfo = paramsMethod[i];

                var paramValor = paramsEnviados.Length > i ? paramsEnviados[i] : null;

                try {

                    if (paramValor.stringOrEmptyLower().StartsWith("request")) {

                        string keyRequest = paramValor.stringOrEmpty().Replace("request_", "");

                        paramValor = dados[keyRequest] ?? "";
                    }

                    if (ParamInfo.Name.Contains("selected") && paramValor.isEmpty() || paramValor == "0") {

                        paramValor = selected.stringOrEmpty();

                    }

                    Type paramType = Nullable.GetUnderlyingType(ParamInfo.ParameterType) ?? ParamInfo.ParameterType;

                    if (paramType == typeof(int) && (paramValor.isEmpty() || paramValor.stringOrEmptyLower() == "null")) {

                        paramValor = "0";
                    }

                    if (paramType == typeof(bool) && (paramValor.isEmpty() || paramValor.stringOrEmptyLower() == "null")) {

                        listaParametros.Add(null);

                        continue;
                    }

                    var paramConverted = Convert.ChangeType(paramValor, paramType);

                    listaParametros.Add(paramConverted);

                } catch (Exception ex) {

                    var User = HttpContextFactory.Current.User;

                    var extra = $"ID SISTEMA: { User.idOrganizacao() } | ID AREA ASSOCIADO: 0 | ID CHECKOUT: { User.idOrganizacaoCheckout() }";

                    UtilLog.saveError(ex, $"Erro ao converter {OMetodo.Name} {parametros} {paramValor} em formato {ParamInfo.ParameterType} em: {extra}");

                    listaParametros.Add(null);
                }

            }

            return listaParametros;
        }
    }
}