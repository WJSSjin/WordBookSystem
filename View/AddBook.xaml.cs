using BookSystem.Dao;
using BookSystem.Model;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;

namespace BookSystem.View {
    /// <summary>
    /// AddBook.xaml 的交互逻辑
    /// </summary>
    public partial class AddBook : Window {

        public AddBook() {
            InitializeComponent();
            // 居中桌面
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            // 监听
            btn_openBookPath.Click += OnClick;
            btn_openBookImg.Click += OnClick;
            btn_submit.Click += Submit;
            btn_reset.Click += Reset;
        }
        /// <summary>
        /// 打开文件夹
        /// </summary>
        /// <param name="btn"></param>
        private void OnClick(object sender, EventArgs e) {
            Button btn = (Button)sender;
            //打开文件窗口
            OpenFileDialog ofd = new OpenFileDialog();                
            if( btn.Name == "btn_openBookPath" ) {
                ofd.Filter = "Word文件(*.doc,*.docx)|*.doc;*.docx";
                 
                 //打开的文件名路径
                if( ofd.ShowDialog() == true ) {
                    bookPath.Text = ofd.FileName;
                }
            } else {
                ofd.Filter = "图像文件(*.bmp, *.jpg,*.png)|*.bmp;*.jpg;*.png"; 
                //打开的文件名路径
                if( ofd.ShowDialog() == true ) {
                    bookImg.Text = ofd.FileName;
                }
            }
            //如果没有打开
            if( ofd.FileName == "" )
                MessageBox.Show("打开失败");
        }
        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="btn"></param>
        private void Submit(object sender, EventArgs e) {
            // 获取值
            string name = bookName.Text;
            string img = bookImg.Text;
            string path = bookPath.Text;
            if( "".Equals(name) || "".Equals(img) || "".Equals(path) ) {
                MessageBox.Show("不可以为空！");
                return;
            }
            // 添加书籍
            Book book = new Book {
                BookName = name,
                BookPath = path,
                BookImg = img,
                BookCount = 0,
                BookHistory = 0,
                BookDisplay = Book.IsDisplay.open
            };
            BooksDao bookDao = new BooksDao();
            Book result = bookDao.AddBook(book);
            if( result !=null ) {
                MessageBox.Show("导入成功");
                // 本身关闭
                Close();
                // 打开主页面
                MainWindow.Active.GetMainWindow(result);
            } else {
                MessageBox.Show("导入失败");
            }
        }
        /// <summary>
        /// 重置
        /// </summary>
        /// <param name="btn"></param>
        private void Reset(object sender, EventArgs e) {
            bookName.Text = bookPath.Text = bookImg.Text = "";
            MessageBox.Show("清除成功");
        }
    }
}
