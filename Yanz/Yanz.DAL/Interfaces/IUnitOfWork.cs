using System;
using System.Threading.Tasks;
using Yanz.DAL.Entities;
using Yanz.DAL.Repositories;

namespace Yanz.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Choice> Choices { get; }
        QuestionRepository Questions { get; }
        FolderRepository Folders { get; }
        IRepository<QuestionSet> QuestionSets { get; }
        IRepository<Set> Sets { get; }
        IRepository<ModerMsg> ModerMsgs { get; }

        void Save();
        Task SaveAsync();
    }
}
