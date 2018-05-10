namespace Yanz.Models.Quiz
{
    public class QuestionSet
    {
        public System.Guid Id { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public System.DateTime Created { get; set; }

        //TODO: Возможно надо использовать User от Identity, а owner в ViewModel
        /// <summary>
        /// Владелец
        /// </summary>
        public string Owner { get; set; }
        public string Title { get; set; }


        #region Связи
        public Folder Folder { get; set; }
        public System.Guid? FolderId { get; set; }
        public System.Collections.Generic.List<Question> Questions { get; set; }
        #endregion
    }
}
