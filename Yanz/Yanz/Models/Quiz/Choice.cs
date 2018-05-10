namespace Yanz.Models.Quiz
{
    public class Choice
    {
        public System.Guid Id { get; set; }

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
        public System.Guid QuestionId { get; set; }
        #endregion
    }
}
