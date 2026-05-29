using System;
using System.Collections.Generic;
using System.Text;

namespace RyuuseiManager.Classes
{
    public class Folder
    {
        public Folder()
        {
            FolderName = "(null)";
            Cards = [];
            RegularCardIndex = -1;
        }

        public string FolderName { get; set; }
        public List<int> Cards { get; set; }
        public int RegularCardIndex { get; set; }
    }
}
