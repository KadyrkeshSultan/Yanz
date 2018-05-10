namespace Yanz.Models.Quiz
{
    public class Folder
    {
        public System.Guid Id { get; set; }
        public string Title { get; set; }

        #region Связи
        public System.Guid? ParentId { get; set; }

        //TODO: Нужен по типу User Identity
        #endregion
    }
}
