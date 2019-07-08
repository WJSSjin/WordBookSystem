
using BookSystem.Model;
using BookSystem.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace BookSystem.Dao {
    public class BooksDao {

        private static readonly XmlHandler Xml = new XmlHandler(Config.BooksPathName);
        private const string RootNode = "Book";

        // 书籍缓存
        private static readonly List<Book> BooksTemp = new List<Book>();

        /// <summary>
        /// 创建Book文件
        /// </summary>
        /// <returns>是否🆗</returns>
        public static bool CreateBooksFile() {
            // 新建xml文件
            return Xml.CreateXML(Config.BooksRoot);
        }
        /// <summary>
        /// 添加书籍
        /// </summary>
        /// <param name="book">Book对象</param>
        /// <returns>是否创建成功</returns>
        public Book AddBook(Book book) {
            // 获取个数
            int count = Xml.GetChildsCount();
            Book bk = QueryBook(book.BookName.Trim());
            // 存在书籍
            if( count>0 && QueryBook(book.BookName.Trim()) != null ) {
                return null;
            }

            // 写入 word
            book.BookPath = CopyFile(book.BookPath);
            // 拷贝 图片
            book.BookImg = CopyFile(book.BookImg);

            // 开始添加到xml文件
            // 赋值
            List<string> key = new List<string>();
            List<string> value = new List<string>();
            string id = ( count+1 ).ToString();
            key.Add("BookId");
            value.Add(id);
            key.Add("BookName");
            value.Add(book.BookName.Trim());
            key.Add("BookPath");
            value.Add(book.BookPath.Trim());
            key.Add("BookImg");
            value.Add(book.BookImg.Trim());
            key.Add("BookCount");
            value.Add(book.BookCount.ToString().Trim());
            key.Add("BookHistory");
            value.Add(book.BookHistory.ToString().Trim());
            key.Add("BookDisplay");
            value.Add(book.BookDisplay.ToString().Trim());

            // 创建节点
            Xml.CreateNode(RootNode, " ");
            // 设置属性
            Xml.SetNodeAttr("id",id,count);
            // 创建子节点
            Xml.CreateNodes(key, value, RootNode);
            // 路径返回
            book.BookImg  = Directory.GetCurrentDirectory() + book.BookImg;
            // 开始添加缓存
            BooksTemp.Add(book);
            return book;
        }
        /// <summary>
        /// 拷贝文件
        /// </summary>
        /// <param name="fromPath">路径</param>
        /// <returns>是否OK布尔值</returns>
        private string CopyFile(string fromPath) {
            // 书籍目录
            string fileBasePath = Directory.GetCurrentDirectory() +  Config.WordPath;
            // 是否存在
            if( !Directory.Exists(fileBasePath) ) {
                Directory.CreateDirectory(fileBasePath);
            }
            //截取后缀名
            string fileName  = fromPath.Substring(fromPath.LastIndexOf('\\')).Trim();
            string filePath  = Path.Combine(fileBasePath + fileName);
            // bug 路径按string传入前面会多一个 ?
            if( fromPath .IndexOf('?') !=-1) fromPath = fromPath.Substring(1);
            //必须判断要复制的文件是否存在
            if( !File.Exists(filePath) && File.Exists(fromPath) )
            {
                //三个参数分别是源文件路径，存储路径，若存储路径有相同文件是否替换
                File.Copy(Path.Combine(fromPath), filePath, false);
                return Config.WordPath + fileName;
             }
            return Config.WordPath + fileName;
        }
        /// <summary>
        /// 获取所有书籍
        /// </summary>
        /// <returns>Book对象list</returns>
        public List<Book> GetAllBook() {
            if( BooksTemp.Count  == 0 )
                QueryBook();
            return BooksTemp;
        }
        /// <summary>
        /// 查询书籍
        /// </summary>
        /// <param name="bookName">关键值</param>
        /// <returns>值</returns>
        public Book QueryBook(string bookName="") {
            Book book = null;
            string name;
            // 无缓存 
            if( BooksTemp.Count == 0 ) {
                // 获取所有书籍节点
                XmlNodeList bookList = Xml.GetXmlDoc().DocumentElement.ChildNodes;
                // 无缓存赋值
                foreach( XmlNode item in bookList ) {
                    name =  item.SelectSingleNode("BookName").InnerText;
                    Book b = new Book {
                        BookId = Convert.ToInt32(item.SelectSingleNode("BookId").InnerText),
                        BookName = name,
                        BookPath = item.SelectSingleNode("BookPath").InnerText,
                        BookImg =  Directory.GetCurrentDirectory() + item.SelectSingleNode("BookImg").InnerText,
                        BookCount = Convert.ToInt32(item.SelectSingleNode("BookCount").InnerText),
                        BookHistory = Convert.ToInt32(item.SelectSingleNode("BookHistory").InnerText),
                        BookDisplay = item.SelectSingleNode("BookDisplay").InnerText == "open" ? Book.IsDisplay.open : Book.IsDisplay.dele,
                    };
                    // 因为要添加缓存
                    if( name.Equals(bookName.Trim()) )
                        book = b;
                    // 添加
                    BooksTemp.Add(b);
                }
            }else {
                // 有缓存
                foreach( Book item in BooksTemp ) {
                    if( item.BookName.Equals(bookName.Trim()) ) {
                        book = item;
                        break;
                    }
                }
            }
            return book;
        }
        /// <summary>
        /// 更改 book 参数
        /// </summary>
        /// <param name="nodeName">节点</param>
        /// <param name="nodeValue">值</param>
        /// <param name="id">book id</param>
        /// <returns>bool</returns>
        public bool ChangBook(string nodeName, string nodeValue, int id) {
            return Xml.UpdateNode(nodeName, nodeValue, id);
        }
    }
}
