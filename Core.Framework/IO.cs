using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Framework
{
    public static class IO
    {
        /// <summary>
        /// Return full path of destination file
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="ToFolder"></param>
        /// <param name="ToFileName"></param>
        /// <returns></returns>
        public static string MoveFile(string FileName, string ToFolder, string ToFileName = null)
        {
            if (ToFileName == null)
                ToFileName = Path.GetFileName(FileName);
            else
                ToFileName = Path.GetFileName(ToFileName);

            ToFolder = ToFolderNormalize(ToFolder);

            if (!Directory.Exists(ToFolder))
                Directory.CreateDirectory(ToFolder);

            string destFileName = Path.Combine(ToFolder, ToFileName);

            File.Copy(FileName, destFileName, true);

            File.Delete(FileName);

            return destFileName;
        }

        public static string ToFolderNormalize(string ToFolder)
        {
            string monthFolder = DateTime.Today.ToString("yyyy-MM");

            ToFolder = Path.Combine(ToFolder, monthFolder);

            if (!Directory.Exists(ToFolder))
                Directory.CreateDirectory(ToFolder);

            return ToFolder;
        }
    }

}
