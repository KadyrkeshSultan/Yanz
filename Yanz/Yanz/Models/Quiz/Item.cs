namespace Yanz.Models.Quiz
{
    public class Item
    {
        public string Id { get; set; }

        /// <summary>
        /// Например Set или Folder
        /// </summary>
        public string Kind { get; set; }

        public int? QuestionsCount { get; set; }

        public string Title { get; set; }
    }
}
