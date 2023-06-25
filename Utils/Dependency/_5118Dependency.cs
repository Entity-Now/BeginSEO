using BeginSEO.Data;
using BeginSEO.Data.DataEnum;
using BeginSEO.SQL;
using BeginSEO.Utils._5118;
using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeginSEO.Utils.Dependency
{
    public class _5118Dependency
    {
        public readonly _5118Request ROriginal;
        public readonly _5118Request RAkey;
        public readonly _5118Request RNewOriginal;
        public _5118Dependency(dataBank db)
        {
            // 获取5118API的key
            var keys = db.Set<Settings>().ToList();
            ROriginal = _5188Tools.Original(keys.GetSettingValue(SettingsEnum.Original));
            RAkey = _5188Tools.Akey(keys.GetSettingValue(SettingsEnum.ReplaceKeyWord));
            RNewOriginal = _5188Tools.NewOriginal(keys.GetSettingValue(SettingsEnum.SeniorRewrite));
        }
    }
}
