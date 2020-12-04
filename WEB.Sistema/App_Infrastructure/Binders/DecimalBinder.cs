using System;
using System.Web.Mvc;

namespace WEB.App_Infrastructure {

    public class DecimalModelBinder : DefaultModelBinder{

        //
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext){
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

	        string valorDecimal = valueProviderResult.AttemptedValue;

            return valueProviderResult == null || valueProviderResult.AttemptedValue == "" ? base.BindModel(controllerContext, bindingContext) : Convert.ToDecimal(valorDecimal);
            // of course replace with your custom conversion logic
        }    
    }
}