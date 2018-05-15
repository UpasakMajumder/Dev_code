﻿using System;
using System.IO;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Contracts
{
    public interface IFileService
    {
        Task<string> GetUrlFromS3(string key);

        string CreateMailingList(string fileName, Stream fileStream);

        Task<Uri> GetContainerFileUrl(Guid containerId);
    }
}
