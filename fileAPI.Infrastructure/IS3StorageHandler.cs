using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fileAPI.Infrastructure
{
    public interface IS3StorageHandler
    {
        public Task<string> UploadToStorage(IFormFile file);

    }
}
