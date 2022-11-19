using System;
using System.Collections.Generic;
using System.Text;

namespace SeerbitHackaton.Core.FileStorage
{
    public static class ImageGetter
    {
        static public string GetBase64String(IFileStorageService fileStorageService,string path)
        {
            if (!string.IsNullOrEmpty(path) && fileStorageService.FileExists(path))
                return fileStorageService.ConvertFileToBase64(path);
            return string.Empty;
        }
    }
}
