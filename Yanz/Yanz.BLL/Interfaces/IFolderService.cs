using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Yanz.BLL.DTO;

namespace Yanz.BLL.Interfaces
{
    public interface IFolderService
    {
        FolderDTO GetFolder(string id);
        Task<FolderDTO> GetFolderAsync(string id);
        IEnumerable<FolderDTO> GetUserFolders(string userId);
        Task<IEnumerable<FolderDTO>> GetUserFoldersAsync(string userId);
        void AddFolder(FolderDTO folderDTO);
        Task AddFolderAsync(FolderDTO folderDTO);
        void Dispose();
    }
}
