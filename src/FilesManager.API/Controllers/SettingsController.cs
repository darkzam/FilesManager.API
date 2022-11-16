using FilesManager.API.Filters;
using FilesManager.API.Models;
using FilesManager.Application.Common.Interfaces;
using FilesManager.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilesManager.API.Controllers
{
    [ServiceFilter(typeof(AuthorizationFilter))]
    [ApiController]
    [Route("api/[Controller]")]
    public class SettingsController : ControllerBase
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

            return Ok(result.Select(x => new SettingDto() { Name = x.Name, Value = x.Value }));
        }

        [HttpPost]
        public async Task<ActionResult<Setting>> Create([FromBody] SettingDto settingDto)
        {
            if (settingDto is null)
            {
                return BadRequest();
            }

            if (string.IsNullOrWhiteSpace(settingDto.Name) || string.IsNullOrWhiteSpace(settingDto.Value))
            {
                return BadRequest();
            }

            var existingSetting = await _unitOfWork.SettingRepository.SearchBy(x => x.Name == settingDto.Name);

            if (existingSetting.Any())
            {
                return BadRequest("Name already exists in the system.");
            }

            var setting = new Setting()
            {
                Name = settingDto.Name,
                Value = settingDto.Value,
                Enabled = true,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };

            var result = _unitOfWork.SettingRepository.Create(setting);

            await _unitOfWork.CompleteAsync();

            return Ok(new SettingDto() { Name = result.Name, Value = result.Value });
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] SettingDto settingDto)
        {
            if (settingDto is null)
            {
                return BadRequest();
            }

            if (string.IsNullOrWhiteSpace(settingDto.Name) || string.IsNullOrWhiteSpace(settingDto.Value))
            {
                return BadRequest();
            }

            var result = await _unitOfWork.SettingRepository.SearchBy(x => x.Name == settingDto.Name);

            var existingSetting = result.FirstOrDefault();

            if (existingSetting is null)
            {
                return NotFound("Parameter not found in the system.");
            }

            existingSetting.UpdatedDate = DateTime.UtcNow;
            existingSetting.Value = settingDto.Value;

            _unitOfWork.SettingRepository.Update(existingSetting);

            await _unitOfWork.CompleteAsync();

            return Ok(new SettingDto() { Name = existingSetting.Name, Value = existingSetting.Value });
        }
    }
}
