using System;

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

        /// <summary>
        /// Превью
        /// </summary>
        public string Image { get; set; }

        #region Связи
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public Folder Folder { get; set; }
        public string FolderId { get; set; }
        public System.Collections.Generic.List<Question> Questions { get; set; }
        #endregion

        public QuestionSet()
        {

        }

        public QuestionSet(ApplicationUser user, string title, System.DateTime created, string folderId, string image)
        {
            Id = Guid.NewGuid().ToString();
            ApplicationUser = user;
            Title = title;
            FolderId = folderId;
            Created = created;
            Image = image;
            Questions = new System.Collections.Generic.List<Question>();
        }

        public void Update(QuestionSetView nSet)
        {
            this.FolderId = nSet.FolderId;
            this.Title = nSet.Title;
            this.Image = nSet.Image;
        }
    }
}
