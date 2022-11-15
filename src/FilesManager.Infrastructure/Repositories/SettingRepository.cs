using FilesManager.Application.Common.Interfaces;
using FilesManager.Domain.Models;
using FilesManager.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FilesManager.Infrastructure.Repositories
{
    public class SettingRepository : ISettingRepository
    {
        private readonly FilesManagerContext _filesManagerContext;
        public SettingRepository(FilesManagerContext filesManagerContext)
        {
            _filesManagerContext = filesManagerContext ?? throw new ArgumentNullException(nameof(filesManagerContext));
        }

        public Setting Create(Setting setting)
        {
            var result = _filesManagerContext.Settings.Add(setting);

            return result.Entity;
        }

        public async Task<IEnumerable<Setting>> GetAll()
        {
            return await _filesManagerContext.Settings.ToListAsync();
        }

        public void Update(Setting setting)
        {
            _filesManagerContext.Settings.Update(setting);
        }

        public async Task<IEnumerable<Setting>> SearchBy(Expression<Func<Setting, bool>> predicate)
        {
            return await _filesManagerContext.Settings.Where(predicate).ToListAsync();
        }
    }
}
