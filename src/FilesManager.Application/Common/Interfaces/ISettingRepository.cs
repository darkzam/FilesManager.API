using FilesManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FilesManager.Application.Common.Interfaces
{
    public interface ISettingRepository
    {
        Task<IEnumerable<Setting>> GetAll();
        Task<IEnumerable<Setting>> SearchBy(Expression<Func<Setting, bool>> predicate);
        Setting Create(Setting setting);
        void Update(Setting setting);
    }
}
