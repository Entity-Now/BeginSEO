using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BeginSEO.Components;

namespace BeginSEO.ModelView
{
    public class MainModelView: ObservableObject
    {
        KeyWordReplice k_c = new KeyWordReplice();
        Employ E_c = new Employ();
        GPT G_c = new GPT();
        public MainModelView()
        {
            Content = k_c;
            NavSelector = new RelayCommand(()=>Content = k_c);
            EmploySelector = new RelayCommand(()=>Content = E_c);
            GPTSelector = new RelayCommand(()=>Content = G_c);
        }
        private Control content;
        public Control Content
        {
            get => content;
            set => SetProperty(ref content, value);
        }
        public ICommand NavSelector { get; set; }
        public ICommand EmploySelector { get; set; }
        public ICommand GPTSelector { get; set; }

    }
}
