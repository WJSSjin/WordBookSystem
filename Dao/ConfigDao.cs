
using BookSystem.Model;
using BookSystem.Util;
using System.Collections.Generic;
using System.IO;

namespace BookSystem.Dao {
    public class ConfigDao {

        private static readonly XmlHandler Xml = new XmlHandler(Config.ConfigPathName);
        /// <summary>
        /// 创建Config文件
        /// </summary>
        /// <returns>是否🆗</returns>
        public static bool CreateConfigFile() {
            // 文件存在退出
            if( File.Exists(Directory.GetCurrentDirectory() + Config.ConfigPathName) )
                return false;
            // 新建文件
            Config config = new Config {
                Theme = Config.ThemeS.white,
                ViewSize = Config.ViewSizeS.def,
                FootSize = Config.FootSizeS.def
            };
            List<string> key = new List<string>();
            List<string> value = new List<string>();
            key.Add("BasePath");
            value.Add(Config.BasePath);
            key.Add("WordPath");
            value.Add(Config.WordPath);
            key.Add("Theme");
            value.Add(config.Theme.ToString());
            key.Add("ViewSize");
            value.Add(config.ViewSize.ToString());
            key.Add("FootSize");
            value.Add(config.FootSize.ToString());
            // 新建xml对象 并且添加属性
            return Xml.CreateXML(Config.ConfigRoot) && Xml.CreateNodes(key, value);
        }
        /// <summary>
        /// 更新值
        /// </summary>
        /// <param name="key">名字</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public bool ChangeConfig(string key, string value) {
            return Xml.UpdateNode( key, value);
        }
        /// <summary>
        /// 查询key的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns>值</returns>
        public string QueryConfig(string key) {
            return Xml.GetXmlReader(key);
        }
        
    }
}
