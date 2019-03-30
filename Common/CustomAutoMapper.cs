using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class CustomAutoMapper<TDestination> where TDestination : class, new()
    {
        /// <summary>
        /// گرفتن مدلی که قرار است تبدیل شود
        /// </summary>
        /// <param name="source">نمونه ای از مدل مبدا</param>
        /// <param name="specials"></param>
        /// <returns></returns>
        public static TDestination GetFrom(object source, List<string> NotMap = null, params object[] specials)
        {
            var destination = new TDestination();
            foreach (var item in destination.GetType().GetProperties())
            {
                var special = specials.FirstOrDefault(p => p.GetType().GetProperty(item.Name) != null);
                if (special != null)
                {
                    var prop = destination.GetType().GetProperty(item.Name);
                    if (prop != null)
                    {
                        prop.SetValue(destination, special.GetType().GetProperty(item.Name).GetValue(special));
                    }
                    else
                    {
                        throw new Exception("نام فیلد ها با یک دیگر تطابق ندارند");
                    }
                }
                else
                {
                    var prop = destination.GetType().GetProperty(item.Name);
                    if (prop == null) continue;
                    var sp = source.GetType().GetProperty(item.Name);
                    if (sp == null) continue;
                    var value = sp.GetValue(source);
                    if (prop.Name == "Id" && (int)value == 0)
                        continue;
                    if (NotMap != null && NotMap.Any(x => x == prop.Name))
                        continue;
                    prop.SetValue(destination, value);
                }
            }
            return destination;
        }
    }
}
