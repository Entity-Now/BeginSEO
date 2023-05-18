using BeginSEO.Data.DataEnum;
using BeginSEO.SQL;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BeginSEO.ModelView
{
    public class SettingsViewModel : ObservableObject
    {
        public readonly dataBank Db;
        public SettingsViewModel(dataBank _db) 
        {
            Db = _db;
        }
        public string _OpenAiKey;
        public string OpenAiKey
        {
            get
            {

                return getValue(ref _OpenAiKey, SettingsEnum.OpenAi.ToString());
            }
            set
            {
                setValue(ref _OpenAiKey, SettingsEnum.OpenAi.ToString(), value);
            }
        }
        /// <summary>
        /// 智能原创KEY
        /// </summary>
        public string _Original;
        public string Original
        {
            get{

                return getValue(ref _Original, SettingsEnum.Original.ToString());
            }
            set{
                setValue(ref _Original, SettingsEnum.Original.ToString(), value);
            }
        }
        /// <summary>
        /// 相似度查询
        /// </summary>
        public string _Contrast;
        public string Contrast
        {
            get
            {
                return getValue(ref _Contrast, SettingsEnum.Contrast.ToString());
            }
            set 
            {
                setValue(ref _Contrast, SettingsEnum.Contrast.ToString(), value);
            }
        }
        /// <summary>
        /// 一键换词
        /// </summary>
        public string _ReplaceKeyWord;
        public string ReplaceKeyWord
        {
            get
            {
                return getValue(ref _ReplaceKeyWord, SettingsEnum.ReplaceKeyWord.ToString());
            }
            set
            {
                setValue(ref _ReplaceKeyWord, SettingsEnum.ReplaceKeyWord.ToString(), value);
            }
        }
        /// <summary>
        /// 原创度检测
        /// </summary>
        public string _Detection;
        public string Detection
        {
            get
            {
                return getValue(ref _Detection, SettingsEnum.Detection.ToString());
            }
            set
            {
                setValue(ref _Detection, SettingsEnum.Detection.ToString(), value);
            }
        }

        void setValue(ref string field, string name, string value)
        {
            Inser(name, value);
            SetProperty(ref field, value);
        }
        string getValue(ref string field, string name)
        {
            if (string.IsNullOrEmpty(field))
            {
                field = Get(name);
            }
            return field;
        }
        string Get(string name)
        {
            var data = Db.Set<Data.Settings>().FirstOrDefault(I => I.Name == name);
            return data != null ? data.Value : string.Empty;
        }
        void Inser(string name, string value)
        {
            Db.InsertOrUpdate<Data.Settings>(new Data.Settings()
            {
                Name = name,
                Value = value
            }, I => I.Name == name);
        }
    }
}
