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
    [Pages("关键词替换", IsHome:true)]
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
        string replice(string Source = null, bool IsLevel = false)
        {
            string Text = Source ?? frontText.Text;
            if (string.IsNullOrWhiteSpace(Text))
            {
                ShowToast.Show("请输入要替换的文本",ShowToast.Type.Error);
                return Text;
            }

            var list = KeyWords
                .Where(I => I.Type != true)
                .Where(I => !IsLevel || I.level == -1) // 使用逻辑非和简化条件判断
                .OrderByDescending(I => I.Key.Length)
                .ThenBy(I => I.level);

            var random = new Random(); // 创建一个随机数生成器，以便避免在循环中多次创建

            foreach (var item in list)
            {
                string[] splitText = item.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (splitText.Length == 0) continue; // 跳过没有值的关键词

                string newString = splitText[random.Next(splitText.Length)];
                Text = Text.Replace(item.Key, newString);
            }

            behindText.Text = Text;

            return Text;
        }
        private void clean_Click(object sender, RoutedEventArgs e)
        {
            behindText.Text = "";
            frontText.Text = "";
        }

        private async void CopyAndDelete_Click(object sender, RoutedEventArgs e)
        {
            ShowModal.ShowLoading();
            // 获取剪切板的数据
            var clipboardText = Clipboard.GetText();

            if (string.IsNullOrEmpty(frontText.Text))
            {
                if (!string.IsNullOrEmpty(clipboardText))
                {
                    frontText.Text = clipboardText;
                }
            }

            string tempValue = replice(frontText.Text, true) ?? string.Empty;

            // 5118智能原创

            if (Original.IsChecked == true)
            {
                var originalResult = await _5188Tools.Original(tempValue, string.IsNullOrEmpty(Strict.Text) ? "0" : Strict.Text);
                if (originalResult?.errcode != "0")
                {
                    await ShowToast.Show("智能原创失败", ShowToast.Type.Error);
                }
                else
                {
                    float schedule = float.Parse(originalResult.like) * 100;
                    OriginalValue.Value = schedule;
                    OriginalText.Text = $"{schedule}%";
                    tempValue = originalResult.data;
                    await ShowToast.Show("智能原创成功", ShowToast.Type.Success);
                }
            }
            if (ReplaceKeyWord.IsChecked == true)
            {
                var filterKeyWord = KeyWords.Where(kw => kw.Type)
                    .Select(kw => kw.Value.Replace(",", "|"))
                    .Aggregate(string.Empty, (a, b) => $"{a}|{b}");

                var reKeyword = await _5188Tools.ReplaceKeyWord(!string.IsNullOrEmpty(tempValue) ? tempValue : frontText.Text, filterKeyWord);

                if (reKeyword?.errcode != "0")
                {
                    await ShowToast.Show("一键换词失败", ShowToast.Type.Error);
                }
                else
                {
                    float schedule = float.Parse(reKeyword.like) * 100;
                    OriginalValue.Value = schedule;
                    OriginalText.Text = $"{schedule}%";
                    tempValue = reKeyword.data;
                    await ShowToast.Show("一键换词成功", ShowToast.Type.Success);
                }
            }

            replice(tempValue);

            if (IsCopy.IsChecked == true)
            {
                Clipboard.SetText(behindText.Text);
            }

            ShowModal.Closing();
        }

    }
}
