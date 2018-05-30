using System;

namespace Yanz.Models.Quiz
{
    public class Choice
    {
        public string Id { get; set; }

        /// <summary>
        /// Ссылка на изображение
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Порядок расположения выбора
        /// </summary>
        public int Order { get; set; }

        public bool IsCorrect { get; set; }

        /// <summary>
        /// Тескс ответа, можно HTML
        /// </summary>
        public string Content { get; set; }

        #region Связи
        public Question Question { get; set; }
        public string QuestionId { get; set; }
        #endregion

        public Choice GetCopy(string questionId)
        {
            Choice c = new Choice()
            {
                Content = this.Content,
                Id = Guid.NewGuid().ToString(),
                Image = this.Image,
                Order = this.Order,
                IsCorrect = this.IsCorrect,
                QuestionId = questionId
            };
            return c;
        }

        public Choice()
        {

        }

        public Choice(ChoiceView view, string questionId)
        {
            Id = Guid.NewGuid().ToString();
            Image = view.Image;
            Order = view.Order;
            IsCorrect = view.IsCorrect;
            Content = view.Content;
            QuestionId = questionId;
        }
    }
}
