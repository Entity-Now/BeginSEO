using BeginSEO.Data;
using BeginSEO.Data.DataEnum;
using BeginSEO.SQL;
using BeginSEO.Utils._5118;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeginSEO.Services
{
        public class _5118Service
        {
            protected readonly dataBank db;
            public _5118Service(dataBank _db)
            {
                db = _db;
            }
            public string Get(SettingsEnum type)
            {
                var data = db.Set<Settings>().FirstOrDefault(i => i.Name == type.ToString());
                return data.Value;
            }
            public void Set(SettingsEnum type, string value)
            {
                var data = db.Set<Settings>().FirstOrDefault(i => i.Name == type.ToString());
                data.Value = value;
                db.SaveChanges();
            }
        }
}
