using FilesManager.API.Filters;
using FilesManager.Application.Common.Interfaces;
using FilesManager.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilesManager.API.Controllers
{
    [ServiceFilter(typeof(AuthorizationFilter))]
    [ApiController]
    [Route("api/[Controller]")]
    public class SettingsController: ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public SettingsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Setting>>> GetAll()
        {
            var result = await _unitOfWork.SettingRepository.GetAll();

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Setting>> Create([FromBody] Setting setting)
        {
            var result = _unitOfWork.SettingRepository.Create(setting);

            await _unitOfWork.CompleteAsync();

            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] Setting setting)
        {
            _unitOfWork.SettingRepository.Update(setting);

            await _unitOfWork.CompleteAsync();

            return Ok(setting);
        }
    }
}
