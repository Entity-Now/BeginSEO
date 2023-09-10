using BeginSEO.Data.DataEnum;
using BeginSEO.Services;
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
        public readonly _5118Service _5118;
        public SettingsViewModel(_5118Service __5118) 
        {
            _5118 = __5118;
        }
        public string _OpenAiKey;
        public string OpenAiKey
        {
            get
            {

                return getValue(ref _OpenAiKey, SettingsEnum.OpenAi);
            }
            set
            {
                setValue(ref _OpenAiKey, SettingsEnum.OpenAi, value);
            }
        }
        /// <summary>
        /// 智能原创KEY
        /// </summary>
        public string _Original;
        public string Original
        {
            get{

                return getValue(ref _Original, SettingsEnum.Original);
            }
            set{
                setValue(ref _Original, SettingsEnum.Original, value);
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
                return getValue(ref _Contrast, SettingsEnum.Contrast);
            }
            set 
            {
                setValue(ref _Contrast, SettingsEnum.Contrast, value);
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
                return getValue(ref _ReplaceKeyWord, SettingsEnum.ReplaceKeyWord);
            }
            set
            {
                setValue(ref _ReplaceKeyWord, SettingsEnum.ReplaceKeyWord, value);
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
                return getValue(ref _Detection, SettingsEnum.Detection);
            }
            set
            {
                setValue(ref _Detection, SettingsEnum.Detection, value);
            }
        }
        /// <summary>
        /// 智能原创升级版
        /// </summary>
        public string _SeniorRewrite;
        public string SeniorRewrite
        {
            get
            {
                return getValue(ref _SeniorRewrite, SettingsEnum.SeniorRewrite);
            }
            set
            {
                setValue(ref _SeniorRewrite, SettingsEnum.SeniorRewrite, value);
            }
        }
        void setValue(ref string field, SettingsEnum type, string value)
        {
            SetProperty(ref field, value);
            _5118.Set(type, value);
        }
        string getValue(ref string field, SettingsEnum type)
        {
            if (string.IsNullOrEmpty(field))
            {
                field = _5118.Get(type);
            }
            return field;
        }
    }
}
