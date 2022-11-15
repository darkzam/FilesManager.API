using FilesManager.Application.Common.Interfaces;
using FilesManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FilesManager.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly HttpClient _httpClient;
        public NotificationService(HttpClient httpClient,
                                   IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        private async Task<IEnumerable<Setting>> CheckSettingUpdates()
        {
            //it goes to the DB and updates settings for this particular service.
            var result = await _unitOfWork.SettingRepository.GetAll();

            if (result.Any())
            {
                return result;
            }

            return null;
        }

        public async Task Notify(FileMetadata fileMetadata)
        {
            //checks for notification channels setup
            var settings = await CheckSettingUpdates();

            //notify all configured channels;
            var calls = new List<Task>();
            foreach (var setting in settings)
            {
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(fileMetadata);
                var content = new StringContent(json);
                calls.Add(_httpClient.PostAsync(setting.Value, content));
            }

            await Task.WhenAll(calls);
        }
    }
}
