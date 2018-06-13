using System;
using System.Threading.Tasks;
using Yanz.DAL.Repositories;

namespace Yanz.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ChoiceRepository Choices { get; }
        QuestionRepository Questions { get; }
        FolderRepository Folders { get; }
        QuestionSetRepository QuestionSets { get; }
        SetRepository Sets { get; }
        ModerMsgRepository ModerMsgs { get; }

        void Save();
        Task SaveAsync();
    }
}
