using fileAPI.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fileAPI.Infrastructure
{
    public class FileRepository : IFileRepository
    {
        private readonly IDbContextFactory<FileContext> _contextFactory;

        private readonly ConnectionStrings _connectionStrings;

        public FileRepository(IDbContextFactory<FileContext> contextFactory, IOptions<ConnectionStrings> connectionStrings)
        {
            _contextFactory=contextFactory;
            _connectionStrings = connectionStrings.Value;
        }

        public async Task<IEnumerable<FileEntry>> GetAllForRecipe(int recipeId)
        {
            using( var context = _contextFactory.CreateDbContext() )
            {
                var files = context.Files.ToList();

                await context.DisposeAsync();


                return files;
            }
        }

        public async Task Save(FileEntry entry)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                context.Files.Add(entry);
                await context.SaveChangesAsync();
                await context.DisposeAsync();
            }
        }
    }
}
