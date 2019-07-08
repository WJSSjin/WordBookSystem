using Word = Microsoft.Office.Interop.Word;
using System;
using System.Collections;
using System.IO;

namespace BookSystem.Dao {
    class WordDao {

        private static readonly Hashtable hashtable = new Hashtable();

        /// <summary>
        /// 读取Word
        /// </summary>
        public string[] ReadWordBook(string fileName) {
            string[] temps;
            if( hashtable.Contains(fileName) ) {
                return (string[]) hashtable[fileName];
            }
            string filePath = Directory.GetCurrentDirectory() + fileName;
            // bug 路径按string传入前面会多一个 ?
            if( filePath.IndexOf('?') !=-1 )
                filePath = filePath.Substring(1);
            //Word.ApplicationClass doc = new Microsoft.Office.Interop.Word.ApplicationClass();
            object file = filePath;
            Word.Application app = new Word.Application();
            Word.Document doc = null;
            try {
                object unknow = Type.Missing;
                //所打开的MSWord程序，是否是可见的
                app.Visible = false;
                doc = app.Documents.Open(ref file);

                temps = new string[doc.Paragraphs.Count];

                for( int i = 0; i < doc.Paragraphs.Count; i++ ) {
                    temps[i] = doc.Paragraphs[i+1].Range.Text.Trim();
                }
                
            } catch( Exception ex ) {
                temps = new string[1];
                temps[0] = ex.Message;
            } finally {
                doc.Close();
                app.Quit();
            }
            // 添加缓存
            hashtable.Add(fileName, temps);

            return temps;
        }
    }
}
