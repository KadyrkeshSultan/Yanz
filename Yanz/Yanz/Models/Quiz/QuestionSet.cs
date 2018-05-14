namespace Yanz.Models.Quiz
{
    public class QuestionSet
    {
        public string Id { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public System.DateTime Created { get; set; }

        public string Title { get; set; }


        #region Связи
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public Folder Folder { get; set; }
        public string FolderId { get; set; }
        public System.Collections.Generic.List<Question> Questions { get; set; }
        #endregion
    }
}
