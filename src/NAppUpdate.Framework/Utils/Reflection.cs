using System;
using System.Collections.Generic;
using System.Reflection;

namespace NAppUpdate.Framework.Utils
{
    using NAppUpdate.Framework.Tasks;
    using NAppUpdate.Framework.Conditions;
    using NAppUpdate.Framework.Common;

    public static class Reflection
    {
        internal static void FindTasksAndConditionsInAssembly(System.Reflection.Assembly assembly,
            Dictionary<string, Type> updateTasks, Dictionary<string, Type> updateConditions)
        {
            foreach (Type t in assembly.GetTypes())
            {
                if (typeof(IUpdateTask).IsAssignableFrom(t))
                {
                    updateTasks.Add(t.Name, t);
                    UpdateTaskAliasAttribute[] tasksAliases = (UpdateTaskAliasAttribute[])t.GetCustomAttributes(typeof(UpdateTaskAliasAttribute), false);
                    foreach (UpdateTaskAliasAttribute alias in tasksAliases)
                    {
                        updateTasks.Add(alias.Alias, t);
                    }
                }
                else if (typeof(IUpdateCondition).IsAssignableFrom(t))
                {
                    updateConditions.Add(t.Name, t);
                    UpdateConditionAliasAttribute[] tasksAliases = (UpdateConditionAliasAttribute[])t.GetCustomAttributes(typeof(UpdateConditionAliasAttribute), false);
                    foreach (UpdateConditionAliasAttribute alias in tasksAliases)
                    {
                        updateConditions.Add(alias.Alias, t);
                    }
                }
            }
        }

        internal static void SetNauAttributes(INauFieldsHolder fieldsHolder, Dictionary<string, string> attributes)
        {
            // Load public non-static properties
            PropertyInfo[] propertyInfos = fieldsHolder.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            string attValue = string.Empty;
            foreach (PropertyInfo pi in propertyInfos)
            {
                object[] atts = pi.GetCustomAttributes(typeof (NauFieldAttribute), false);
                if (atts.Length != 1) continue; // NauFieldAttribute doesn't allow multiples

                NauFieldAttribute nfa = (NauFieldAttribute) atts[0];

                // Get the attribute value, process it, and set the object's property with that value
                if (!attributes.TryGetValue(nfa.Alias, out attValue)) continue;
				if (pi.PropertyType == typeof(String))
				{
					pi.SetValue(fieldsHolder, attValue, null);
				}
				else if (pi.PropertyType == typeof(DateTime))
				{
					DateTime dt = DateTime.MaxValue;
                    long filetime = long.MaxValue;
					if (DateTime.TryParse(attValue, out dt))
						pi.SetValue(fieldsHolder, dt, null);
                    else if (long.TryParse(attValue, out filetime))
                    {
                        try
                        {
                            // use local time, not UTC
                            dt = DateTime.FromFileTime(filetime);
                            pi.SetValue(fieldsHolder, dt, null);
                        }
                        catch { }
                    }
				}
				// TODO: type: Uri
                else if (pi.PropertyType.IsEnum)
                {
                    object eObj = Enum.Parse(pi.PropertyType, attValue);
                    if (eObj != null)
                        pi.SetValue(fieldsHolder, eObj, null);
                }
                else
                {
                    MethodInfo mi = pi.PropertyType.GetMethod("Parse", new Type[] {typeof (String)});
                    if (mi == null) continue;
                    object o = mi.Invoke(null, new object[] {attValue});
                    
                    if (o != null)
                    	pi.SetValue(fieldsHolder, o, null);
                }
            }
        }

        internal static object GetNauAttribute(INauFieldsHolder fieldsHolder, string attributeName)
        {
            PropertyInfo pi = fieldsHolder.GetType().GetProperty(attributeName, BindingFlags.Public | BindingFlags.Instance);
            if (pi == null) return null;

            return pi.GetValue(fieldsHolder, null);
        }
    }
}
