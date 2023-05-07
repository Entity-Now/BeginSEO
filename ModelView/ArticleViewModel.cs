using BeginSEO.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BeginSEO.ModelView
{
    public class ArticleViewModel : ObservableObject
    {
        public string _Title;
        public string Title
        {
            get { return _Title; }
            set
            {
                SetProperty(ref _Title, value);
            }
        }
        public string _Content;
        public string Content
        {
            get { return _Content; }
            set
            {
                SetProperty(ref _Content, value);
            }
        }
        public ObservableCollection<Article> _ArticleList;
        public ObservableCollection<Article> ArticleList
        {
            get { return _ArticleList; }
            set
            {
                SetProperty(ref _ArticleList, value);
            }
        }
        public CollectionViewSource ArticleSource { get; set; }
    }
}
