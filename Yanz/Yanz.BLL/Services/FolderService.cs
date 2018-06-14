using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Yanz.BLL.DTO;
using Yanz.BLL.Infrastructure;
using Yanz.BLL.Interfaces;
using Yanz.DAL.Entities;
using Yanz.DAL.Interfaces;

namespace Yanz.BLL.Services
{
    public class FolderService : IFolderService
    {
        IUnitOfWork Unit { get; set; }

        public FolderService(IUnitOfWork unitOfWork)
        {
            Unit = unitOfWork;
        }

        public void AddFolder(FolderDTO folderDTO)
        {
            Folder folder = new Folder()
            {
                Id = Guid.NewGuid().ToString(),
                AppUserId = folderDTO.AppUserId,
                Title = folderDTO.Title,
                ParentId = folderDTO.ParentId
            };
            Unit.Folders.Add(folder);
            Unit.Save();
        }

        public async Task AddFolderAsync(FolderDTO folderDTO)
        {
            string parentId = (await Unit.Folders.GetAsync(folderDTO.ParentId))?.Id;
            if (folderDTO.ParentId != "root" && parentId == null)
                throw new ValidationException($"Not found folder {folderDTO.ParentId}", "");

            Folder folder = new Folder()
            {
                Id = Guid.NewGuid().ToString(),
                AppUserId = folderDTO.AppUserId,
                Title = folderDTO.Title,
                ParentId = folderDTO.ParentId
            };

            await Unit.Folders.AddAsync(folder);
            await Unit.SaveAsync();
        }

        public void Dispose()
        {
            Unit.Dispose();
        }

        public FolderDTO GetFolder(string id)
        {
            if (id == null)
                throw new ValidationException("ID is null", "");
            var folder = Unit.Folders.Get(id);
            if (folder == null)
                throw new ValidationException($"Not found folder with ID = {id}", "");

            return new FolderDTO()
            {
                Id = folder.Id,
                AppUserId = folder.AppUserId,
                ParentId = folder.ParentId,
                Title = folder.Title
            };
        }

        public async Task<FolderDTO> GetFolderAsync(string id)
        {
            if (id == null)
                throw new ValidationException("ID is null", "");
            var folder = await Unit.Folders.GetAsync(id);
            if (folder == null)
                throw new ValidationException($"Not found folder with ID = {id}", "");

            return new FolderDTO()
            {
                Id = folder.Id,
                AppUserId = folder.AppUserId,
                ParentId = folder.ParentId,
                Title = folder.Title
            };
        }

        public IEnumerable<FolderDTO> GetUserFolders(string userId)
        {
            if (userId == null)
                throw new ValidationException("UserId is null", "");
            var folders = Unit.Folders.GetAllByUser(userId);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Folder, FolderDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<Folder>, List<FolderDTO>>(folders);
        }

        public async Task<IEnumerable<FolderDTO>> GetUserFoldersAsync(string userId)
        {
            if (userId == null)
                throw new ValidationException("UserId is null", "");
            var folders = await Unit.Folders.GetAllByUserAsync(userId);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Folder, FolderDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<Folder>, List<FolderDTO>>(folders);
        }
    }
}
