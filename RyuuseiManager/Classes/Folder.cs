namespace RyuuseiManager.Classes
{
    public class Folder
    {
        public Folder()
        {
            FolderName = "(null)";
            Cards = [];
            RegularCardIndex = -1;
            TagCards = [-1, -1];
        }

        public string FolderName { get; set; }
        public List<int> Cards { get; set; }
        public int RegularCardIndex { get; set; }
        public int[] TagCards { get; set; }
    }
}
