global using Microsoft.AspNetCore.Http;
global using Microsoft.Extensions.FileProviders;

namespace SeerbitHackaton.Core.FileStorage
{
    public interface IFileStorageService
    {
        bool FileExists(string path);

        bool FolderExists(string path);

        bool TryCreateFolder(string path);

        void CreateFolder(string path);

        void DeleteFile(string path);

        void RenameFile(string oldPath, string newPath);

        FileInfo CreateFile(string path);

        IFileInfo GetFile(string path);

        bool TrySaveStream(string path, Stream inputStream);

        Task<bool> TrySaveStreamAsync(string path, Stream inputStream);

        void SaveStream(string path, Stream inputStream);

        Task SaveStreamAsync(string path, Stream inputStream);

        void SaveBytes(string path, byte[] raw);

        void WriteAllText(string path, string content);

        string ReadAllText(string path);

        bool TrySaveBytes(string path, byte[] raw);

        string MapStorage(string path);

        string RenameDuplicateFile(string filepath);

        bool ValidateFile(string[] allowedExt, string fileName);
        /// <summary>
        /// Converts the file to base64.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>System.String.</returns>
        string ConvertFileToBase64(string path);
        /// <summary>
        /// Converts the file to base64.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>System.String.</returns>
        string ConvertFileToBase64(IFormFile file);
        /// <summary>
        /// Converts the file to byte array.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>Array of bytes</returns>
        byte[] ConvertFileToByteArray(string path);
        /// <summary>
        /// Deletes the files.
        /// </summary>
        /// <param name="filePaths">The file paths.</param>
        void DeleteFiles(List<string> filePaths);
        /// <summary>
        /// Tries the upload documents.
        /// </summary>
        /// <param name="formFiles">The form files.</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        List<string> TryUploadDocuments(List<IFormFile> formFiles);
        /// <summary>
        /// Tries to upload a document
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        (string, bool) TryUploadDocument(IFormFile file);
        string ConvertFileToBase64Test(string path);
    }
}
