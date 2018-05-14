using System.Collections.Generic;

namespace Yanz.Models.Quiz
{
    public class FolderView
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string ParentId { get; set; }
        public System.Collections.Generic.List<Breadcrumb> Breadcrumbs { get; set; }
        public System.Collections.Generic.List<Item> Items { get; set; }

        public FolderView()
        {

        }
        public FolderView(Folder folder)
        {
            if (folder != null)
            {
                Id = folder.Id;
                Title = folder.Title;
                ParentId = folder.ParentId;
            }
        }

        public FolderView(Folder folder, List<Breadcrumb> breadcrumbs, List<Item> items)
            :this(folder)
        {
            Breadcrumbs = breadcrumbs;
            Items = items;
        }
    }
}
