namespace Yanz.Models.Quiz
{
    public class QuestionView
    {
        public System.Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Kind { get; set; }
        public bool IsOnBoarding { get; set; }
        public System.DateTime Modified { get; set; }
        public System.DateTime Created { get; set; }
        public System.Guid QuestionSetId { get; set; }
        public System.Guid OnBoardingId { get; set; }
        public int Weight { get; set; }
        public bool IsPoll { get; set; }

        public QuestionView(Question qst)
        {
            Id = qst.Id;
            Title = qst.Title;
            Content = qst.Content;
            Kind = qst.Kind;
            Modified = qst.Modified;
            Created = qst.Created;
            QuestionSetId = qst.QuestionSetId;
            Weight = qst.Weight;
            IsPoll = qst.IsPoll;
        }
    }
}
