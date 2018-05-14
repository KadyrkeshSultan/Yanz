namespace Yanz.Models.Quiz
{
    public class Folder
    {
        public string Id { get; set; }
        public string Title { get; set; }

        #region Связи
        public string ParentId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
        #endregion
    }
}
