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
            DefaultCardIndex = -1;
        }

        public string FolderName { get; set; }
        public List<int> Cards { get; set; }
        public int DefaultCardIndex { get; set; }
    }
}
