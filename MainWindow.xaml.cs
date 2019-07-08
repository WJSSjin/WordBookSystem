using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BookSystem.Dao;
using BookSystem.Model;
using BookSystem.View;

namespace BookSystem {
    
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window {
        public static MainWindow Active;
        private BooksDao bookDao;
        // 记录最后书籍坐标
        private readonly int leftLength = 150;
        private readonly int TopLength = 40;
        public MainWindow() {
            // 判断是否有目录 无就创建
            Util.XmlHandler.CreatPath(Config.BasePath);
            // 判断是否有 config。xml文件 
            ConfigDao.CreateConfigFile();
            // 判断是否有 book.xml 文件
            BooksDao.CreateBooksFile();

            // 打开视图
            InitializeComponent();
            // 添加视图
            Active = this;
            // 刷新视图 获取书本
            bookDao = new BooksDao();
            List<Book> bs = bookDao.GetAllBook();
            foreach( Book item in bs ) {
                // 添加书籍
                ViewAddBook(item, leftLength, TopLength, BooksList);
                if( leftLength >= 450 && TopLength <220) {
                    leftLength = 150;
                    TopLength = 220;
                }else {
                    leftLength +=150;
                }
            }
            // 添加按钮书籍
            btn_addBook.Click +=  OnClick;
            // 移动添加按钮
            ViewMoveBook(btn_addBook, leftLength, TopLength);
        }
        /// <summary>
        /// 添加书籍
        /// </summary>
        private void ViewAddBook(Book item,int leftLength,int TopLength,Grid obj) {
            Button btn = new Button();
            ImageBrush brush = new ImageBrush();
            btn.ToolTip = item.BookName;
            btn.Width = 100;
            btn.Height = 140;
            btn.Margin = new Thickness(leftLength + 140, TopLength, 0, 0);
            btn.HorizontalAlignment = HorizontalAlignment.Left;
            btn.VerticalAlignment = VerticalAlignment.Top;
            // 设置背景
            brush.ImageSource = new BitmapImage(new Uri(item.BookImg, UriKind.Relative));
            btn.Background = brush;

            // 添加样式
            btn.Cursor=Cursors.Hand;
            btn.SetValue(StyleProperty, Resources["NoMouseOverButtonStyle"]);
            // 添加
            obj.Children.Add(btn);

            // 添加方法
            btn.Click += GetRoodBook;
        }
        /// <summary>
        /// 移动书籍
        /// </summary>
        private void ViewMoveBook(Button btn, int leftLength, int TopLength) {
            if( leftLength > 450 ) {
                leftLength = 150;
                TopLength = 220;
            }
            btn.Margin = new Thickness(leftLength + 140, TopLength, 0, 0);
        }

        /// <summary>
        /// 返回主窗口
        /// </summary>
        public void GetMainWindow(Book book=null) {
            if( Active == null ) {
                new MainWindow();
            } else if(book!=null) {
                ViewAddBook(book, leftLength, TopLength, BooksList);
                ViewMoveBook(btn_addBook, leftLength+150, TopLength);
            }
            Active.Show();
        }
        /// <summary>
        /// 重写关闭状态方法
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(CancelEventArgs e) {
            if( Active != null && MessageBox.Show("是否关闭程序？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.No ) {
                e.Cancel = true;
            } else {
                Environment.Exit(0);
            }

        }
        /// <summary>
        /// 按钮事件打开添加书籍
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetRoodBook(object sender, EventArgs e) {
            Button bt = (Button)sender;
            Hide();
            ReadBook read = new ReadBook(bookDao.QueryBook(bt.ToolTip.ToString().Trim()));
            read.Show();
        }
        /// <summary>
        /// 调用方法
        /// </summary>
        /// <param name="btn"></param>
        private void OnClick(object sender, EventArgs e) {
            AddBook add = new AddBook();
            add.Show();
        }
        /// <summary>
        /// 重写关闭之后状态方法
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e) {
            Active = null;
        }
    }
}
