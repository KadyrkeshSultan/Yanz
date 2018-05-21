using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Yanz.Models.Quiz
{
    public class QuestionSetView
    {
        public string Id { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public System.DateTime Created { get; set; }

        /// <summary>
        /// Владелец
        /// </summary>
        public string Owner { get; set; }

        [StringLength(100,MinimumLength =1, ErrorMessage ="Min length 1")]
        public string Title { get; set; }

        [Required]
        public string FolderId { get; set; }
        public System.Collections.Generic.List<Breadcrumb> Breadcrumbs { get; set; }

        #region Связи
        public List<QuestionView> Questions { get; set; }
        #endregion

        public QuestionSetView()
        {

        }
        public QuestionSetView(QuestionSet set)
        {
            if (set != null)
            {
                Id = set.Id;
                Owner = set.ApplicationUser.Email;
                Title = set.Title;
                Created = set.Created;
                Questions = GetQuestionViews(set.Questions);
                FolderId = set.FolderId;
            }
        }

        public QuestionSetView(QuestionSet set, List<Breadcrumb> breadcrumbs)
            :this(set)
        {
            Breadcrumbs = breadcrumbs;
        }

        private List<QuestionView> GetQuestionViews(List<Question> questions)
        {
            var listQuestionView = new List<QuestionView>();
            foreach (var qst in questions)
                listQuestionView.Add(new QuestionView(qst));
            return listQuestionView;
        }
    }
}
