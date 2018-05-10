using System.Collections.Generic;

namespace Yanz.Models.Quiz
{
    public class QuestionSetView
    {
        public System.Guid Id { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public System.DateTime Created { get; set; }

        /// <summary>
        /// Владелец
        /// </summary>
        public string Owner { get; set; }
        public string Title { get; set; }


        #region Связи
        public List<QuestionView> Questions { get; set; }
        #endregion

        public QuestionSetView(QuestionSet set)
        {
            Id = set.Id;
            //TODO : Здесь должно быть типа User.Email
            Owner = set.Owner;
            Title = set.Title;
            Created = set.Created;
            Questions = GetQuestionViews(set.Questions);
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
