var StringJs = {
    
    Class: function(){
        
        this.onlyAlpha = function(str){
            var newStr = str.replace(/[^a-z0-9]/gi, '');
            return newStr;
        }
        
        this.toDecimal = function(str){
            var dec = str;
            if(str.toString().indexOf(",") != "-1"){
                dec = str.replace(".", "").replace(",", ".");
            }
            return Number(dec).toFixed(2);
        }
        
        
        this.toInt = function(str){
            var integer = 0;
            if($.isNumeric(str)){
                integer = str;
            }
            return Number(integer);
        }
    }
};
var StringJs = new StringJs.Class();

