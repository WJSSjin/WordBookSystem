using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace BookSystem.Util {
    public class XmlHandler {

        private readonly string Path ;

        private readonly XmlDocument xmlDoc = new XmlDocument();

        /// <summary>
        /// 创建目录
        /// </summary>
        public static bool CreatPath(string path) {
            //获取当前文件夹路径
            string subPath = Directory.GetCurrentDirectory() + $"/{path.Trim()}";
            //检查是否存在文件夹
            if( false == Directory.Exists(subPath) ) {
                //创建文件夹
                Directory.CreateDirectory(subPath);
                return true;
            }
            return false;
        }
        /// <summary>
        ///  xml 文件操作     
        /// </summary>
        /// <param name="path">XML文件路径</param>
        public XmlHandler(string path) {
            Path = Directory.GetCurrentDirectory() + path;
        }
        /// <summary>
        /// 获取路径
        /// </summary>
        /// <returns></returns>
        public string GetPath() => Path;
        /// <summary>
        /// 获取xml对象
        /// </summary>
        /// <returns></returns>
        public XmlDocument GetXmlDoc() {
            xmlDoc.Load(Path);
            return xmlDoc;
        }
        /// <summary>
        /// 获取 XML Root 数据个数
        /// </summary>
        /// <returns></returns>
        public int GetChildsCount() {
            xmlDoc.Load(Path);
            return xmlDoc.DocumentElement.ChildNodes.Count;
        }

        /// <summary>
        /// 创建空的xml文件
        /// </summary>
        /// <returns></returns>
        public bool CreateXML(string RootNode) {
            // 文件存在退出
            if( File.Exists(Path) )
                return false;
            // 创建头文件
            XmlNode node = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "");
            xmlDoc.AppendChild(node);
            // 创建Root节点
            XmlNode root = xmlDoc.CreateElement(RootNode);
            root.InnerText = "";
            xmlDoc.AppendChild(root);
            // 保存
            return SaveXmlFile();
        }

        /// <summary>
        /// 添加一个XML文件中的 节点
        /// </summary>
        /// <param name="parentNode">父节点</param>
        /// <param name="name">节点名字</param>
        /// <param name="value">节点值</param>
        public bool CreateNode(string name, string value,string parentNode = "") {
            XmlNode node = xmlDoc.CreateNode(XmlNodeType.Element, name, null);
            node.InnerText = value;
            // 默认父节点添加
            GetParentNode(parentNode).AppendChild(node);
            // 保存
            return SaveXmlFile();
        }

        /// <summary>
        /// 添加多个XML文件中的 节点
        /// </summary>
        /// <param name="key">List</param>
        /// <param name="value">List</param>
        public bool CreateNodes(List<string> key, List<string> value, string parentNode = "") {
            for( int i = 0; i < key.Count; i++ ) {
                CreateNode(key[i].ToString(), value[i].ToString(),parentNode);
            }
            // 保存
            return SaveXmlFile();
        }
        public bool SetNodeAttr(string key,string value,int count=0) {
            XmlElement node = (XmlElement) xmlDoc.DocumentElement.ChildNodes[count];
            node.SetAttribute(key,value);
            // 保存
            return SaveXmlFile();
        }
        /// <summary>
        /// 读取节点值
        /// </summary>
        /// <param name="nodeName">ROO下一级节点</param>
        /// <returns>节点值</returns>
        public string GetXmlReader(string nodeName) {
            xmlDoc.Load(Path);
            return xmlDoc.DocumentElement.SelectSingleNode(nodeName).InnerText;
        }
        /// <summary>
        /// 更新XML中指定节点的值
        /// </summary>
        /// <param name="nodeName">需要更改的节点</param>
        /// <param name="nodeValue">需要更新的节点值</param>
        /// <param name="id">节点id</param>
        public bool UpdateNode(string nodeName, string nodeValue,int id=0) {
            xmlDoc.Load(Path);
            XmlNode xn = GetChangNode(id).SelectSingleNode(nodeName);
            xn.InnerText = nodeValue;
            // 保存
            return SaveXmlFile();
        }
        public XmlNode GetChangNode(int id=0) {
            if( id==0 ) {
                return xmlDoc.DocumentElement;
            } else {
                return xmlDoc.DocumentElement.ChildNodes[--id];
            }
        }
        /// <summary>
        /// 保存Xml文件
        /// </summary>
        /// <returns>是否OK</returns>
        private bool SaveXmlFile() {
            try {
                xmlDoc.Save(Path);
                return true;
            } catch( Exception ) {
                return false;
            }
        }
        /// <summary>
        /// 获取父节点
        /// </summary>
        /// <returns>父节点</returns>
        private XmlNode GetParentNode(string parentNode = "") {
            int count = GetChildsCount();
            // 默认父节点直接添加
            if( parentNode==""  )
                return xmlDoc.DocumentElement;
            // 否者父节点的子节点
            else
                return xmlDoc.DocumentElement.ChildNodes[--count];
        }


    }

}