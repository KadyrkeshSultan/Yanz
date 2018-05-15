﻿using System.Collections.Generic;

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
        public string Title { get; set; }


        #region Связи
        public List<QuestionView> Questions { get; set; }
        #endregion

        public QuestionSetView(QuestionSet set)
        {
            Id = set.Id;
            Owner = set.ApplicationUser.Email;
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