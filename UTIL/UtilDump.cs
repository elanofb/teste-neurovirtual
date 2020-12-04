using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace System {
    public static class UtilDump {


        //
        public static string dumpInfo(NameValueCollection form) {
            StringBuilder log = new StringBuilder();

            string[] keys = form.AllKeys.ToArray();

            foreach (var key in keys) {
                if (form[key] != null) {
                    log.AppendLine(String.Concat(key, ": ", form[key]));
                }
            }

            return log.ToString();
        }

        //
        public static string printProperties(object obj) {
            string type = obj.GetType().ToString();
            return "<b>" + type + "</b>" + printProperties(obj, 0);
        }

        //
        public static string printProperties(object obj, int indent) {
            if (obj == null) return "null";
            string indentString = new string(' ', indent);
            Type objType = obj.GetType();
            PropertyInfo[] properties = objType.GetProperties();

            StringBuilder text = new StringBuilder();
            foreach (PropertyInfo property in properties) {
                object propValue = property.GetValue(obj, null);
                if (property.PropertyType.Assembly == objType.Assembly) {
                    text.Append("<br /><p style=\"padding-left:" + indent + "0px;font-weight:bold;margin:0px;\">").AppendFormat("{0}{1}:", indentString, property.Name).Append("</p>");
                    text.AppendLine(printProperties(propValue, indent + 2));
                } else {
                    text.Append("<p style=\"padding-left:" + indent + "0px;margin:0px;\">").AppendFormat("{0}{1}: {2}", indentString, property.Name, propValue).Append("</p>");
                }
            }
            return text.ToString();
        }

        //
        public static void dump(object obj) {
            dump(obj, 0);
        }

        //
        private static void dump(object obj, int tab) {
            if (obj == null) {
                dump_write(0, "NULL\n");
                return;
            }

            if (obj is ICollection) {
                dump_collection((ICollection)obj, tab);
                return;
            }

            if (obj is IDictionary) {
                dump_dictionary((IDictionary)obj, tab);
                return;
            }

            if (obj is IEnumerable && !(obj is string)) {
                dump_enumerable((IEnumerable)obj, tab);
                return;
            }
            dump_write(0, obj + "\n");
        }

        //
        private static void dump_collection(ICollection collection, int tab) {
            dump_write(0, "Collection(" + collection.Count + ") {\n");
            int i = 0;
            foreach (object obj in collection) {
                dump_write(tab + 1, "[" + i + "]=> ");
                dump(obj, tab + 1);
                i++;
            }
            dump_write(tab, "}\n");
        }

        //
        private static void dump_dictionary(IDictionary dictionary, int tab) {
            dump_write(0, "Dictionary(" + dictionary.Count + ") {\n");
            foreach (object key in dictionary.Keys) {
                dump_write(tab + 1, "[" + key + "]=> ");
                dump(dictionary[key], tab + 1);
            }
            dump_write(tab, "}\n");
        }

        //
        private static void dump_enumerable(IEnumerable enumerable, int tab) {
            dump_collection(enumerable.Cast<object>().Select(x => x).ToArray(), tab);
        }

        //
        private static void dump_write(int num, string str) {
            HttpContext.Current.Response.Write("<pre>");
            HttpContext.Current.Response.Write((new string(' ', num * 2) + str));
            HttpContext.Current.Response.Write("</pre>");
        }

        //
        public static void displayProperties(object objectA, string objName = "") {

            if (objectA == null) {
                return;
            }

            Type objectType;

            objectType = objectA.GetType();

            foreach (PropertyInfo propertyInfo in objectType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(x => x.CanRead)) {

                object valueA = propertyInfo.GetValue(objectA, null);

                if (typeof (IComparable).IsAssignableFrom(propertyInfo.PropertyType) || propertyInfo.PropertyType.IsPrimitive || propertyInfo.PropertyType.IsValueType) {
                    Console.WriteLine(objName + (objName == "" ? "" : ".") + propertyInfo.ReflectedType.Name + "." + propertyInfo.Name);
                } else {
                    displayProperties(valueA, objName + (objName == "" ? "" : ".") + objectType.Name);
                }
            }
        }
    }
}