using System;

namespace Yanz.Models.Quiz.Public
{
    /// <summary>
    /// Public QuestionSet
    /// </summary>
    public class Set
    {
        public string Id { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public System.DateTime Created { get; set; }

        public int CopyCount { get; set; }
        public string Desc { get; set; }
        public string Title { get; set; }

        /// <summary>
        /// Превью
        /// </summary>
        public string Image { get; set; }

        #region Связи
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public System.Collections.Generic.List<Question> Questions { get; set; }
        #endregion

        public Set()
        {

        }

        public Set(QuestionSet set)
        {
            Id = Guid.NewGuid().ToString();
            Created = DateTime.Now;
            Desc = set.Desc;
            Image = set.Image;
            ApplicationUserId = set.ApplicationUserId;
        }
    }
}
