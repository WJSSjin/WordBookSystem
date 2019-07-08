
namespace BookSystem.Model {
    /// <summary>
    /// 书本信息类
    /// </summary>
    public class Book {

        public enum IsDisplay : int {
            dele = 0,
            open = 1
        }
        /// <summary>
        /// 书号
        /// </summary>
        public int BookId { get; set; }
        /// <summary>
        /// 书名
        /// </summary>
        public string BookName { get; set; }
        /// <summary>
        /// 书籍地址
        /// </summary>
        public string BookPath { get; set; }
        /// <summary>
        /// 封面
        /// </summary>
        public string BookImg { get; set; }
        /// <summary>
        /// 历史记录
        /// </summary>
        public int BookHistory { get; set; }
        /// <summary>
        /// 点击量
        /// </summary>
        public int BookCount{ get; set; }
        /// <summary>
        /// 是否书架显示
        /// </summary>
        public IsDisplay BookDisplay { get; set; }
    }
}
