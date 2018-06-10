using System;
using Yanz.DAL.Entities;

namespace Yanz.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Choice> Choices { get; }
        IRepository<Question> Questions { get; }
        IRepository<Folder> Folders { get; }
        IRepository<QuestionSet> QuestionSets { get; }
        IRepository<Set> Sets { get; }
        IRepository<ModerMsg> ModerMsgs { get; }

        void Save();
    }
}
