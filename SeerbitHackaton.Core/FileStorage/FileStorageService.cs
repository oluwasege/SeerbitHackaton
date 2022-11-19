using Microsoft.IdentityModel.Tokens;

namespace SeerbitHackaton.Core.FileStorage
{
    public class FileStorageService : IFileStorageService
    {
        private readonly IFileProvider _fileProvider;

        public FileStorageService(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }

        public void DeleteFile(string path)
        {
            FileInfo fileInfo = new FileInfo(MapStorage(path));
            if (!fileInfo.Exists)
            {
                throw new ArgumentException(string.Format("File {0} does not exist", path));
            }

            fileInfo.Delete();
        }

        public void RenameFile(string oldPath, string newPath)
        {
            FileInfo sourceFileInfo = new FileInfo(MapStorage(oldPath));
            if (!sourceFileInfo.Exists)
            {
                throw new ArgumentException(string.Format("File {0} does not exist", oldPath));
            }

            FileInfo targetFileInfo = new FileInfo(MapStorage(newPath));
            if (targetFileInfo.Exists)
            {
                throw new ArgumentException(string.Format("File {0} already exists", newPath));
            }

            File.Move(sourceFileInfo.FullName, targetFileInfo.FullName);
        }

        public string RenameDuplicateFile(string filepath)
        {
            var sourceFileInfo = new FileInfo(MapStorage(filepath));
            if (!sourceFileInfo.Exists)
            {
                throw new ArgumentException(string.Format("File {0} does not exists", filepath));
            }

            var folder = sourceFileInfo.Directory + "/";
            var oldFileInfo = sourceFileInfo.Name.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            var newFilePath = "";
            var counter = 1;

            do
            {
                counter++;
                newFilePath = folder + oldFileInfo[0] + " (" + counter.ToString() + ")" + oldFileInfo[1];
            } while (File.Exists(newFilePath));

            //join full directory
            RenameFile(filepath, newFilePath);

            FileInfo targetFileInfo = new FileInfo(MapStorage(newFilePath));

            File.Move(sourceFileInfo.FullName, targetFileInfo.FullName);

            return newFilePath;
        }

        public bool ValidateFile(string[] allowedExt, string fileName)
        {
            var ext = Path.GetExtension(fileName).ToLower();
            return allowedExt.ToList().Any(x => $".{x}".Equals(ext, StringComparison.InvariantCultureIgnoreCase));
        }

        public bool TrySaveStream(string path, Stream inputStream)
        {
            try
            {
                if (FileExists(path))
                {
                    return false;
                }

                SaveStream(path, inputStream);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<bool> TrySaveStreamAsync(string path, Stream inputStream)
        {
            try
            {
                if (FileExists(path))
                {
                    return false;
                }

                await SaveStreamAsync(path, inputStream);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public FileInfo CreateFile(string path)
        {
            FileInfo fileInfo = new FileInfo(MapStorage(path));
            if (fileInfo.Exists)
            {
                throw new ArgumentException(string.Format("File {0} already exists", fileInfo.Name));
            }

            // ensure the directory exists
            var dirName = Path.GetDirectoryName(fileInfo.FullName);
            if (!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }
            File.WriteAllBytes(fileInfo.FullName, new byte[0]);

            return fileInfo;
        }

        public void SaveStream(string path, Stream inputStream)
        {
            // Create the file.
            // The CreateFile method will map the still relative path
            var file = CreateFile(path);

            inputStream.Seek(0, SeekOrigin.Begin);

            using (var outputStream = file.OpenWrite())
            {
                var buffer = new byte[8192];
                for (; ; )
                {
                    var length = inputStream.Read(buffer, 0, buffer.Length);
                    if (length <= 0)
                        break;
                    outputStream.Write(buffer, 0, length);
                }
            }
        }

        public async Task SaveStreamAsync(string path, Stream inputStream)
        {
            // Create the file.
            // The CreateFile method will map the still relative path
            var file = CreateFile(path);

            inputStream.Seek(0, SeekOrigin.Begin);

            using (var outputStream = file.OpenWrite())
            {
                var buffer = new byte[8192];
                for (; ; )
                {
                    var length = inputStream.Read(buffer, 0, buffer.Length);
                    if (length <= 0)
                        break;
                    await outputStream.WriteAsync(buffer, 0, length);
                }
            }
        }

        public void WriteAllText(string path, string content)
        {
            var dirName = Path.GetDirectoryName(path);
            if (!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }

            File.WriteAllText(path, content);
        }

        public string ReadAllText(string path)
        {
            return File.ReadAllText(path);
        }

        public void SaveBytes(string path, byte[] raw)
        {
            var ms = new MemoryStream();
            ms.Write(raw, 0, raw.Length);

            SaveStream(path, ms);
        }

        public bool TrySaveBytes(string path, byte[] raw)
        {
            try
            {
                if (FileExists(path))
                {
                    return false;
                }

                var ms = new MemoryStream();
                ms.Write(raw, 0, raw.Length);

                SaveStream(path, ms);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool TryCreateFolder(string path)
        {
            try
            {
                // prevent unnecessary exception
                DirectoryInfo directoryInfo = new DirectoryInfo(MapStorage(path));
                if (directoryInfo.Exists)
                {
                    return false;
                }

                CreateFolder(path);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public void CreateFolder(string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(MapStorage(path));
            if (directoryInfo.Exists)
            {
                throw new ArgumentException(string.Format("Directory {0} already exists", path));
            }

            Directory.CreateDirectory(directoryInfo.FullName);
        }

        public bool FolderExists(string path)
        {
            return new DirectoryInfo(MapStorage(path)).Exists;
        }

        public bool FileExists(string path)
        {
            return File.Exists(MapStorage(path));
            //return File.Exists(path);
        }

        public string MapStorage(string path)
        {
            return _fileProvider.GetFileInfo(path).PhysicalPath;
            //return path;
        }

        public IFileInfo GetFile(string path)
        {
            var fileInfo = _fileProvider.GetFileInfo(path);
            if (!fileInfo.Exists)
            {
                throw new ArgumentException(string.Format("File {0} does not exist", path));
            }

            return fileInfo;
        }

        /// <summary>
        /// Converts the file to base64.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>System.String.</returns>
        public string ConvertFileToBase64(string path)
        {
            var file = GetFile(path);
            string base64String;
            using (var fileStream = file.CreateReadStream())
            {
                using var ms = new MemoryStream();
                fileStream.CopyTo(ms);
                var fileBytes = ms.ToArray();
                base64String = Convert.ToBase64String(fileBytes);
            }

            return base64String;
        }

        public string ConvertFileToBase64Test(string path)
        {
            byte[] fileBytes = File.ReadAllBytes(path);
            string base64String;
            base64String = Convert.ToBase64String(fileBytes);
            return base64String;
        }

        /// <summary>
        /// Converts the file to base64.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>System.String.</returns>
        public string ConvertFileToBase64(IFormFile file)
        {
            string base64String;
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();
                base64String = Convert.ToBase64String(fileBytes);
            }

            return base64String;
        }

        /// <summary>
        /// Converts the file to byte array.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>Array of bytes</returns>
        public byte[] ConvertFileToByteArray(string path)
        {
            var file = GetFile(path);
            byte[] fileBytes;

            using (var fileStream = file.CreateReadStream())
            {
                using var ms = new MemoryStream();
                fileStream.CopyTo(ms);
                fileBytes = ms.ToArray();
            }

            return fileBytes;
        }

        /// <summary>
        /// Deletes the files.
        /// </summary>
        /// <param name="filePaths">The file paths.</param>
        public void DeleteFiles(List<string> filePaths)
        {
            foreach (var filePath in filePaths)
            {
                if (!filePath.IsNullOrEmpty()) 
                    DeleteFile(filePath);
            }

           
        }

        /// <summary>
        /// Tries the upload documents.
        /// </summary>
        /// <param name="formFiles">The form files.</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        public List<string> TryUploadDocuments(List<IFormFile> formFiles)
        {
            var filePaths = new List<string>();
            var uploaded = false;
            foreach (var file in formFiles)
            {
                var fileName = CommonHelper.GenerateTimeStampedFileName(file.FileName);
                uploaded = TrySaveStream(fileName,
                    file.OpenReadStream());
                if (uploaded)
                    filePaths.Add(fileName);
                else
                    break;
            }

            if (!uploaded && filePaths.Count > 0)
            {
                foreach (var file in filePaths)
                    DeleteFile(file);

                filePaths.Clear();
            }

            return filePaths;
        }

        public (string, bool) TryUploadDocument(IFormFile file)
        {
            string filepath = default;
            if (file != null)
            {
                var paths = TryUploadDocuments(new List<IFormFile> { file });

                if (paths.Any())
                {
                    filepath = paths.First();
                }

                else
                {
                    DeleteFiles(paths);
                    return (null, true);
                }
            }
            return (filepath, false);
        }

        private static readonly char[] InvalidPathChars = Path.GetInvalidPathChars();
    }
}
