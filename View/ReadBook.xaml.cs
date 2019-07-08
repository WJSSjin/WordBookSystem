using BookSystem.Dao;
using BookSystem.Model;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BookSystem.View
{
    /// <summary>
    /// ReadBook.xaml 的交互逻辑
    /// </summary>
    public partial class ReadBook: Window {
        private readonly ConfigDao cd = new ConfigDao();
        private readonly string theme;
        private readonly Book book;
        public ReadBook(Book b){
            // 打开窗口
            InitializeComponent();
            // 居中打开
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            // 文件名
            book = b;
            // 标题
            Book_Name.Text = book.BookName;
            // 获取主题
            theme = cd.QueryConfig("Theme");
            ChangTheme(theme);

            // 监听
            btn_Main.Click += GetManWindow;
            btn_Theme.Click +=  ClickTheme;
        }
        
        /// <summary>
        /// 返回主窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetManWindow(object sender, EventArgs e){
            Close();
            // 打开主页面
            MainWindow.Active.GetMainWindow();
        }
        /// <summary>
        /// 重写加载完毕方法
        /// </summary>
        /// <param name="e"></param>
        protected override void OnContentRendered(EventArgs e) {
            // 读写书籍
            WordDao wd; wd = new WordDao();
            string[] path = wd.ReadWordBook(book.BookPath);
            string s = "";
            for( int i = 0; i < path.Length; i++ ) {
                s += path[i];
            }
            Book_Content.Text = s;
        }
        /// <summary>
        /// 更改主题
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickTheme(object sender, EventArgs e) {
            Button send = (Button)sender;
            if( send.Content.ToString() == "夜间模式" ) {
                ChangTheme("black");
            } else {
                ChangTheme("white");
            }
        }
        /// <summary>
        /// 更改主题
        /// </summary>
        /// <param name="theme"></param>
        private bool ChangTheme(string theme) {
            string white = Config.ThemeS.white.ToString();
            string black = Config.ThemeS.black.ToString();
            bool result;
            if( theme  == white ) {
                //白天模式
                btn_Theme.Content = "夜间模式";
                Book_Content.Foreground = Brushes.Black;//字体颜色
                //Book_Content.Background = Brushes.White;//背景颜色
                // 写入数据
                result = cd.ChangeConfig("Theme", white);
            } else {
                //夜间模式
                btn_Theme.Content = "白天模式";
                Book_Content.Foreground = Brushes.PaleGreen;//字体颜色
                //Book_Content.Background = Brushes.Gray;//背景颜色
                // 写入数据
                result = cd.ChangeConfig("Theme", black);
            }
            return result;
        }
    }
}
