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
using BeginSEO.Attributes;
using BeginSEO.Data;
using BeginSEO.Data.DataEnum;
using BeginSEO.Model;
using BeginSEO.SQL;
using BeginSEO.Utils;
using Microsoft.EntityFrameworkCore;

namespace BeginSEO.Components
{
    /// <summary>
    /// KeyWordReplice.xaml 的交互逻辑
    /// </summary>
    [Pages("关键词替换")]
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
                findData.Key = KeyWord.Text;
                findData.Type = (bool)LockKeyWord.IsChecked;
                findData.level = (int)Level.Value;
            }
            else
            {
                KeyWords.Add(new KeyWord()
                {
                    Key = KeyWord.Text,
                    Value = Content.Text,
                    Type = (bool)LockKeyWord.IsChecked,
                    level = (int)Level.Value
            });
            }
            DataAccess.SaveChanges();
            // clean
            KeyWord.Text = "";
            Content.Text = "";
            Level.Value = 0;
            LockKeyWord.IsChecked = false;
        }
        /// <summary>
        /// 关键词列表选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyWordList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selector = (sender as ListBox).SelectedItem as KeyWord;
            if (selector != null)
            {
                KeyWord.Text = selector.Key;
                LockKeyWord.IsChecked = selector.Type;
                Content.Text = selector.Value;
                Level.Value = selector.level;
            }
        }
        /// <summary>
        /// 替换
        /// </summary>
        void replice(string Source = null)
        {
            string Text = Source ?? frontText.Text;
            if (Text.Length > 0)
            {
                var list = KeyWords.Where(I => I.Type != true).OrderBy(I=>I.level);
                foreach (var item in list)
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

        private async void CopyAndDelete_Click(object sender, RoutedEventArgs e)
        {
            ShowModal.ShowLoading();
            var temp = Clipboard.GetText();
            if (string.IsNullOrEmpty(frontText.Text))
            {
                if (!string.IsNullOrEmpty(temp))
                {
                    frontText.Text = temp;
                }
            }
            string TempValue = "";
            // 5118智能原创
            if (Original.IsChecked == true)
            {
                var OriginalResult = await _5188Tools.Original(frontText.Text, string.IsNullOrEmpty(Strict.Text) ? "0" : Strict.Text);
                if (OriginalResult == null || OriginalResult.errcode != "0")
                {
                    await ShowToast.Show("智能原创失败", ShowToast.Type.Error);
                }
                else
                {
                    float Schedule = float.Parse(OriginalResult.like) * 100;
                    OriginalValue.Value = Schedule;
                    OriginalText.Text = $"{Schedule}%";
                    TempValue = OriginalResult.data;
                    await ShowToast.Show("智能原创成功", ShowToast.Type.Success);
                }
            }
            // 5118一键换词
            if (ReplaceKeyWord.IsChecked == true)
            {
                var FilterKeyWord = KeyWords.Where(I => I.Type == true)
                    .Select(I => I.Value.Replace(",","|"))
                    // 第一个关键词无需添加‘|’
                    .Aggregate(string.Empty,(A, B)=> A + (A != string.Empty ? "|" : string.Empty) + B);

                var ReKeyword = await _5188Tools.ReplaceKeyWord(!string.IsNullOrEmpty(TempValue) ? TempValue : frontText.Text, FilterKeyWord);

                if (ReKeyword == null || ReKeyword.errcode != "0")
                {
                    await ShowToast.Show("一键换词失败", ShowToast.Type.Error);
                }
                else
                {
                    float Schedule = float.Parse(ReKeyword.like) * 100;
                    OriginalValue.Value = Schedule;
                    OriginalText.Text = $"{Schedule}%";
                    TempValue = ReKeyword.data;
                    await ShowToast.Show("一键换词成功", ShowToast.Type.Success);

                }
            }
            // replice
            replice(!string.IsNullOrEmpty(TempValue) ? TempValue : frontText.Text);
            // 设置到剪切板
            if (IsCopy.IsChecked == true)
            {
                Clipboard.SetText(behindText.Text);
            }
            ShowModal.Closing();
        }

    }
}
