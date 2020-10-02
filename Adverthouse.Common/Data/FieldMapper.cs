using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adverthouse.Common.Data
{
    public static class FieldMapper
    {
        public static void MapAllFields(object source, object dst)
        {
            System.Reflection.PropertyInfo[] ps = source.GetType().GetProperties();
            foreach (var item in ps)
            {
                var o = item.GetValue(source);
                var p = dst.GetType().GetProperty(item.Name);
                if (p != null)
                {
                    Type t = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
                    object safeValue = (o == null) ? null : Convert.ChangeType(o, t);
                    p.SetValue(dst, safeValue);
                }
            }
        }

        public static void MapAllFields(string fieldPrefix, object source, object dst)
        {

            System.Reflection.PropertyInfo[] ps = source.GetType().GetProperties().Where(a => a.Name.StartsWith(fieldPrefix)).ToArray();
            foreach (var item in ps)
            {
                var o = item.GetValue(source);
                var p = dst.GetType().GetProperty(item.Name);
                if (p != null)
                {
                    Type t = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
                    object safeValue = (o == null) ? null : Convert.ChangeType(o, t);
                    p.SetValue(dst, safeValue);
                }
            }
        }


    }
}
