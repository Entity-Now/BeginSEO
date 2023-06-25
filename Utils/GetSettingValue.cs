using BeginSEO.Data;
using BeginSEO.Data.DataEnum;
using BeginSEO.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeginSEO.Utils
{
    public static class SettingsExpand
    {
        public static string GetSettingValue(this IEnumerable<Settings> db, SettingsEnum type)
        {
            var findData = db.FirstOrDefault(I => I.Name == type.ToString());
            return findData == null ? "Undefined" :findData.Value;
        }
    }
}
