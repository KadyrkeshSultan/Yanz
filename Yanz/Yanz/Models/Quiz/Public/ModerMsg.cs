namespace Yanz.Models.Quiz.Public
{
    /// <summary>
    /// Moder message
    /// </summary>
    public class ModerMsg
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public System.DateTime Create { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
        public QuestionSet QuestionSet { get; set; }
        public string QuestionSetId { get; set; }
        public Status Status { get; set; }
    }
    public enum Status
    {
        Public,
        Moderation,
        Disapproved
    }
}
