using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;

namespace WEB.Extensions {
    public static class FormExtensions{

        // helper
        public static IDictionary<string, object> ToDictionary(this object obj) {
            IDictionary<string, object> result = new Dictionary<string, object>();

            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(obj);
            foreach (PropertyDescriptor property in properties){
                result.Add(property.Name, property.GetValue(obj));
            }
            return result;
        }

        //
        public static IDictionary<string, object> AddProperty(this object obj, string name, object value){
            var dictionary = obj.ToDictionary();
            dictionary.Add(name, value);
            return dictionary;
        }

        //
        public static IDictionary<string, object> isDisabled (this object obj, bool disabled){
            return disabled ? obj.AddProperty ("disabled", "disabled") : obj.ToDictionary();
        }

        //
        public static IDictionary<string, object> isReadOnly (this object obj, bool disabled){
            return disabled ? obj.AddProperty ("readonly", "readonly") : obj.ToDictionary();
        }

        //
        public static IDictionary<string, object> isChecked(this object obj, bool isChecked) {
            return isChecked ? obj.AddProperty("checked", "checked") : obj.ToDictionary();
        }

		//
        public static IDictionary<string, object> isDateTime (this object obj, DateTime dtValue, bool showHour = false){
			string strDate = (dtValue == DateTime.MinValue? "": dtValue.ToShortDateString() );
			if(showHour){
				strDate = (dtValue == DateTime.MinValue? "": dtValue.ToString());
			}
            return obj.AddProperty ("Value", strDate);
        }

        //Capturar os erros 
        public static string retornarErros(this ModelStateDictionary OModelState){
            string erros = "";
            var query = from state in OModelState.Values  
            from error in state.Errors  
            select error.ErrorMessage;

            erros = String.Join("<br />", query.ToList());
            return erros;
        }

    }
}