using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Yanz.BLL.DTO;
using Yanz.BLL.Infrastructure;
using Yanz.BLL.Interfaces;
using Yanz.DAL.Entities;
using YanzWeb.Models;

namespace YanzWeb.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FoldersController : ControllerBase
    {
        IFolderService service;
        UserManager<AppUser> userManager;

        public FoldersController(IFolderService folderService, UserManager<AppUser> userManager)
        {
            service = folderService;
            this.userManager = userManager;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(string Id)
        {
            try
            {
                var folder = await service.GetFolderAsync(Id);
                if (folder == null)
                    return NotFound(Id);

                FolderView view = new FolderView()
                {
                    Id = folder.Id,
                    ParentId = folder.ParentId,
                    Title = folder.Title
                };
                return Ok(view);
            }
            catch (ValidationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody]FolderView folder)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                string userId = userManager.GetUserId(User);
                FolderDTO folderDTO = new FolderDTO() { Title = folder.Title, AppUserId = userId, ParentId = folder.ParentId };
                await service.AddFolderAsync(folderDTO);
                return Ok();
            }
            catch(ValidationException ex)
            {
                ModelState.AddModelError(ex.Property, ex.Message);
            }
            return BadRequest(folder);
        }
    }
}