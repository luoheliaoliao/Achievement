using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Domain.ViewModel
{
    public class TreeMolde
    {
        public TreeMolde()
        {
            children = new List<TreeMolde>();
            @checked = false;
        }

        public long id { get; set; }

        public string text { get; set; }

        /// <summary>
        /// check是关键字，需要加@来转换
        /// </summary>
        public bool @checked { get; set; }

        public List<TreeMolde> children { get; set; }

    }
}
