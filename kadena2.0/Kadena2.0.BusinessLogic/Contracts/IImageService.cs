using System;

namespace Kadena.BusinessLogic.Contracts
{
    public interface IImageService
    {
        string GetThumbnailLink(string originalImageLink);
    }
}
