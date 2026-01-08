using System;
using System.IO;
using System.Web.Hosting;

namespace ServerModel.ServerModel.Helper
{
    public class CommonHelper
    {
        /// <summary>
        /// Uploads a file from a temporary path to a destination folder.
        /// Supports physical or virtual path and dynamic subfolders.
        /// </summary>
        /// <param name="tempFilePath">Physical temporary file path from upload provider</param>
        /// <param name="baseDestinationPath">Base destination path (virtual or physical)</param>
        /// <param name="subFolders">Optional dynamic folders (like CompId, DocName)</param>
        /// <param name="overwrite">Set to true to overwrite existing file</param>
        /// <returns>Final file path or error message</returns>
        public static string UploadFile(string tempFilePath, string baseDestinationPath, bool overwrite = false, params string[] subFolders)
        {
            try
            {
                // Validate source file
                if (!File.Exists(tempFilePath))
                    return "Error: Source file not found.";

                // Check if destination is virtual path
                string physicalDestinationPath = baseDestinationPath.StartsWith("~")
                    ? HostingEnvironment.MapPath(baseDestinationPath)
                    : baseDestinationPath;

                // Add dynamic subfolders
                if (subFolders != null && subFolders.Length > 0)
                    physicalDestinationPath = Path.Combine(physicalDestinationPath, Path.Combine(subFolders));

                // Create destination folder if not exists
                Directory.CreateDirectory(physicalDestinationPath);

                string fileName = Path.GetFileName(tempFilePath);
                string finalFilePath = Path.Combine(physicalDestinationPath, fileName);

                // Handle overwrite or unique name
                if (File.Exists(finalFilePath))
                {
                    if (overwrite)
                        File.Delete(finalFilePath);
                    else
                    {
                        string fileNameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
                        string ext = Path.GetExtension(fileName);
                        fileName = $"{fileNameWithoutExt}_{DateTime.Now.Ticks}{ext}";
                        finalFilePath = Path.Combine(physicalDestinationPath, fileName);
                    }
                }

                // Move the file
                File.Move(tempFilePath, finalFilePath);

                return $"Success: File uploaded to {finalFilePath}";
            }
            catch (Exception ex)
            {
                return $"Error uploading file: {ex.Message}";
            }
        }
    }
}
