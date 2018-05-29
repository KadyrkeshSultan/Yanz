namespace Yanz.Models.Quiz.Public
{
    /// <summary>
    /// Public choice
    /// </summary>
    public class Chc
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
        public Qst Qst { get; set; }
        public string QstId { get; set; }
        #endregion

        public Chc()
        {

        }
    }
}
