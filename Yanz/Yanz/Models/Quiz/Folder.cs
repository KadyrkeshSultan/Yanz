namespace Yanz.Models.Quiz
{
    public class Folder
    {
        public string Id { get; set; }
        public string Title { get; set; }

        #region Связи
        public string ParentId { get; set; }

        public System.Collections.Generic.List<QuestionSet> QuestionSets { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
        #endregion

        public Folder()
        {

        }

        public Folder(string title, string userId, string parentId)
        {
            Id = System.Guid.NewGuid().ToString();
            Title = title;
            ApplicationUserId = userId;
            ParentId = parentId;
        }
    }
}
