namespace Yanz.Models.Quiz
{
    public class FolderView
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public System.Collections.Generic.List<Breadcrumb> Breadcrumbs { get; set; }
        public System.Collections.Generic.List<Item> Items { get; set; }

        public FolderView(Folder folder)
        {
            Id = folder.Id;
            Title = folder.Title;
        }
    }
}
