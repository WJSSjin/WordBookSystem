
using System;

namespace BookSystem.Model {
    /// <summary>
    /// 系统配置
    /// </summary>
    public class Config {
        // 主目录
        public const string BasePath =  @"\BaseData";
        // 统计文件路径
        public const string BooksPathName  = @"\BaseData\Books.xml";
        // 配置文件路径
        public const string ConfigPathName = @"\BaseData\Config.xml";
        // word 文件夹名
        public const string WordPath  = @"\BaseData\Library";
        // config XML root名
        public const string ConfigRoot = "config";
        // 书籍管理 XML root名
        public const string BooksRoot = "library";
        /// <summary>
        /// 主题
        /// </summary>
        public ThemeS Theme { get; set; }
        public enum ThemeS : int {
            white = 0,
            black = 1
        }
        /// <summary>
        /// 视图大小
        /// </summary>
        public ViewSizeS ViewSize { get; set; }
        public enum ViewSizeS : int {
            def = 0,
            all = 1
        }
        /// <summary>
        /// 字体大小
        /// </summary>
        public FootSizeS FootSize { get; set; }
        public enum FootSizeS : int {
            def = 0,
            big = 1
        }
    }
}
