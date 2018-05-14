namespace Yanz.Models.Quiz
{
    public class SessionQstView
    {
        public string Id { get; set; }

        /// <summary>
        /// Опциональный текст
        /// </summary>
        public string Content { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }
        public bool IsPoll { get; set; }
        public bool? IsTrueCorrect { get; set; }

        /// <summary>
        /// Тип вопроса
        /// </summary>
        public string Kind { get; set; }
        public string OnBoardingId { get; set; }

        /// <summary>
        /// Порядок вопроса
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Текст вопроса
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Балл за ответ
        /// </summary>
        public int Weight { get; set; }

        public System.Collections.Generic.List<Item> Items { get; set; }
    }
}
