﻿using Kadena.Models.Site;
using System;
using System.Threading.Tasks;

namespace Kadena.ScheduledTasks.Infrastructure
{
    public interface IDeleteExpiredMailingListsService
    {
        Task<string> Delete(KenticoSite site);
    }
}
