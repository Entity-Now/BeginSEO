using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.ComponentModel;

namespace 替换关键词.Model
{
    public class EmployData: ObservableObject
    {
        private string url;
        private string status;
        public string Url
        {
            get => url;
            set
            {
                SetProperty(ref url, value);
            }
        }
        public string Status
        {
            get => status;
            set => SetProperty(ref status, value);
        }
    }
}
