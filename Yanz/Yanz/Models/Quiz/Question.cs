using System;
using System.Collections.Generic;
using Yanz.Models.Quiz.Public;

namespace Yanz.Models.Quiz
{
    public class Question
    {
        public string Id { get; set; }

        #region Свойства
        /// <summary>
        /// Текст вопроса
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Ссылка на изображение
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Для голосования
        /// </summary>
        public bool IsPoll { get; set; }

        /// <summary>
        /// Правда является правильной? Для True/False
        /// </summary>
        public bool? IsTrueCorrect { get; set; }

        /// <summary>
        /// Балл за правильный ответ
        /// </summary>
        public int Weight { get; set; }

        public System.DateTime Created { get; set; }
        public System.DateTime Modified { get; set; }

        /// <summary>
        /// Тип вопроса, select or checkbox
        /// </summary>
        public string Kind { get; set; }


        /// <summary>
        /// Опциональный текст вопроса, можно HTML
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Пояснение, можно HTML
        /// </summary>
        public string Explanation { get; set; }

        public int Order { get; set; }
        #endregion

        #region Связи
        public QuestionSet QuestionSet { get; set; }
        public string QuestionSetId { get; set; }

        public Set Set { get; set; }
        public string SetId { get; set; }
        /// <summary>
        /// Варианты ответа
        /// </summary>
        public System.Collections.Generic.List<Choice> Choices { get; set; }
        #endregion

        public Question GetForPublicSetCopy(string setId)
        {
            Question qst = new Question()
            {
                Id = Guid.NewGuid().ToString(),
                Content = this.Content,
                Order = this.Order,
                Created = DateTime.Now,
                Explanation = this.Explanation,
                Image = this.Image,
                IsPoll = this.IsPoll,
                IsTrueCorrect = this.IsTrueCorrect,
                Kind = this.Kind,
                Modified = DateTime.Now,
                Title = this.Title,
                Weight = this.Weight,
                SetId = setId
            };
            return qst;
        }

        public Question()
        {
                
        }
        public Question(QuestionView view)
        {
            Id = Guid.NewGuid().ToString();
            Title = view.Title;
            Image = view.Image;
            IsPoll = false;
            IsTrueCorrect = view.IsTrueCorrect;
            Weight = view.Weight;
            Created = DateTime.Now;
            Modified = DateTime.Now;
            Kind = view.Kind;
            Content = view.Content;
            Explanation = view.Explanation;
            QuestionSetId = view.QuestionSetId;
            Order = view.Order;
        }
        public Question(QuestionView view, List<Choice> choices)
            :this(view)
        {
            Choices = choices;
        }
    }
}
