﻿namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoPOSProvider
    {
        void TogglePOSStatus(int posID);
        bool DeletePOS(int posID);
    }
}
