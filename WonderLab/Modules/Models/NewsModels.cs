using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WonderLab.Modules.Models
{

    public class PlayPageImage
    {
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string url { get; set; }
    }

    public class Dimensions
    {
        /// <summary>
        /// 
        /// </summary>
        public int width { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int height { get; set; }
    }

    public class NewsPageImage
    {
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Dimensions dimensions { get; set; }
    }

    public class EntriesItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string tag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string category { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string date { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string text { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public PlayPageImage playPageImage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public NewsPageImage newsPageImage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string readMoreLink { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string cardBorder { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> newsType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string id { get; set; }
    }

    public class NewsModels
    {
        /// <summary>
        /// 
        /// </summary>
        public int version { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<EntriesItem> entries { get; set; }
    }

}
