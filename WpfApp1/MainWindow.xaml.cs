using MahApps.Metro.Controls;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Windows.Resources;
using System.IO;
using Microsoft.Win32;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private List<RuleItemModel> RuleList;

        public MainWindow()
        {
            InitializeComponent();

            RuleList = Util.LoadRule();
            ruleListBox.ItemsSource = RuleList;
            ruleListBox.SelectedIndex = 0;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (ruleListBox.SelectedIndex < 0)
            {
                return;
            }
            loadingBar.IsIndeterminate = true;
            RuleItemModel ruleItem = RuleList[ruleListBox.SelectedIndex];
            string baseUrl = ruleItem.source.Replace("XXX", keyword.Text);
            string nameTag = ruleItem.group;
            List<MovieItemModel> movieList = new List<MovieItemModel>();
            HttpUtil.Instance.RequestGetAsync(baseUrl, (HTTPClientResponse r) => {
                loadingBar.IsIndeterminate = false;
                var nodes = r.node.SelectNodes(nameTag);
                if(nodes == null)
                {
                    return;
                }
                foreach (var node in nodes)
                {
                    MovieItemModel movieItem = MovieItemModel.ModelWithDocument(node, ruleItem);
                    movieList.Add(movieItem);
                }
                resultListView.ItemsSource = movieList;
            });

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RuleList = Util.LoadRule();
            ruleListBox.ItemsSource = RuleList;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            string magnet = btn.Tag as string;
            Clipboard.SetText(magnet);
            MessageBox.Show("链接已复制到剪切板，快去下载吧!","提示",MessageBoxButton.OK,MessageBoxImage.Information);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            string magnet = btn.Tag as string;
            RegistryKey hklm = Registry.CurrentUser;
            RegistryKey hkSoftWare = hklm.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Uninstall\WebTorrent", true);
            if(hkSoftWare == null)
            {
                MessageBox.Show("没有找到WebTorrent Desktop安装信息，请确定电脑上已安装。", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string installDir = hkSoftWare.GetValue("InstallLocation").ToString();

            hklm.Close();
            hkSoftWare.Close();
            System.Diagnostics.Process.Start(installDir + "\\WebTorrent.exe", magnet);
        }
    }
}
