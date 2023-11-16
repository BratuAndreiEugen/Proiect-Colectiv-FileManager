using fileAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fileAPI.Infrastructure
{
    public interface IFileRepository
    {
        public Task Save(FileEntry entry);
    }
}
