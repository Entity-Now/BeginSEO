using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BeginSEO.Data;
using BeginSEO.Model;
using BeginSEO.SQL;
using BeginSEO.Utils;
using Microsoft.EntityFrameworkCore;

namespace BeginSEO.Components
{
    /// <summary>
    /// KeyWordReplice.xaml 的交互逻辑
    /// </summary>
    public partial class KeyWordReplice : UserControl
    {
        CollectionViewSource KeyWordSource;
        ObservableCollection<KeyWord> KeyWords;
        public KeyWordReplice()
        {
            InitializeComponent();
            this.DataContext = this;
            KeyWordSource = (CollectionViewSource)FindResource(nameof(KeyWordSource));
        }

        private void KeyWordList_Loaded(object sender, RoutedEventArgs e)
        {
           DataAccess.Entity<KeyWord>().Load();
           KeyWordSource.Source = DataAccess.Entity<KeyWord>().Local.ToObservableCollection();
           KeyWords = ((ObservableCollection<KeyWord>)KeyWordSource.Source);
        }
        /// <summary>
        /// 删除元素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void List_Delete_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as Button).Tag as KeyWord;
            if (item != null)
            {
                KeyWords.Remove(item);
                DataAccess.SaveChanges();
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (KeyWord.Text.Length <= 0)
            {
                ShowToast.Open("请输入关键词");
                return;
            }
            if (Content.Text.Length <= 0)
            {
                ShowToast.Open("请输入要替换的值");
                return;
            }
            // add
            var findData = KeyWords.FirstOrDefault(I => I.Key == KeyWord.Text);
            if (findData != null)
            {
                findData.Value = Content.Text;
                findData.Key = KeyWord.Text; ;
            }
            else
            {
                KeyWords.Add(new KeyWord()
                {
                    Key = KeyWord.Text,
                    Value = Content.Text
                });
            }
            DataAccess.SaveChanges();
            // clean
            KeyWord.Text = "";
            Content.Text = "";
        }

        private void KeyWordList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selector = (sender as ListBox).SelectedItem as KeyWord;
            if (selector != null)
            {
                KeyWord.Text = selector.Key;
                Content.Text = selector.Value;
            }
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            replice();
        }
        /// <summary>
        /// 替换
        /// </summary>
        void replice()
        {
            string Text = frontText.Text;
            if (Text.Length > 0)
            {
                foreach (var item in KeyWords)
                {
                    string[] splitText = item.Value.Split(new char[] { ',' });
                    string newString = splitText[new Random().Next(splitText.Length)];
                    // 随机选择一个
                    Text = Text.Replace(item.Key, newString);

                }
                behindText.Text = Text;
            }
            else
            {
                MessageBox.Show("请输入要替换的文本");
            }
        }
        private void clean_Click(object sender, RoutedEventArgs e)
        {
            behindText.Text = "";
            frontText.Text = "";
        }

        private void CopyAndDelete_Click(object sender, RoutedEventArgs e)
        {
            var temp = Clipboard.GetText();
            if (string.IsNullOrEmpty(frontText.Text))
            {
                if (!string.IsNullOrEmpty(temp))
                {
                    frontText.Text = temp;
                }
            }
            // replice
            replice();
            // 设置到剪切板
            Clipboard.SetText(behindText.Text);
        }

    }
}
