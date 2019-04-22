using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;

namespace Helpdesk.Core.Helpers
{
    public class CompressionHelper
    {
        /// <summary>
        /// This method takes zips a file and returns the byte[] of the zipped file and deltes the zipped file
        /// </summary>
        /// <param name="fileToZip"></param>
        /// <param name="zipFileName"></param>
        /// <returns></returns>
        public byte[] ZipFileToByteArray(string fileToZip, string zipFileName)
        {
            using (var s = new ZipOutputStream(File.Create(zipFileName)))
            {
                s.SetLevel(9); // 0-9, 9 being the highest compression
                var buffer = new byte[4096];

                var entry = new ZipEntry(Path.GetFileName(fileToZip))
                {
                    DateTime = DateTime.Now
                };

                s.PutNextEntry(entry);
                using (FileStream fs = File.OpenRead(fileToZip))
                {
                    int sourceBytes;
                    do
                    {
                        sourceBytes = fs.Read(buffer, 0, buffer.Length);
                        s.Write(buffer, 0, sourceBytes);
                    }
                    while (sourceBytes > 0);
                }
                s.Finish();
                s.Close();

                FileStream zippedFile = File.OpenRead(zipFileName);
                MemoryStream zip = new MemoryStream();

                zip.SetLength(zippedFile.Length);
                zippedFile.Read(zip.GetBuffer(), 0, (int)zippedFile.Length);

                zip.Flush();
                zippedFile.Close();
                zip.Dispose();

                //Delete created zip file and original file, because we have it in memory
                File.Delete(zipFileName);

                //return the byte 
                return zip.ToArray();
            }
        }

        /// <summary>
        /// This method zips multiple files into a single zip file
        /// </summary>
        /// <param name="fileNames">List of files to zip</param>
        /// <param name="outputZipFile">The name of the zip file that will be created</param>
        public void ZipFile(string[] fileNames, string outputZipFile)
        {
            using (var s = new ZipOutputStream(File.Create(outputZipFile)))
            {
                s.SetLevel(9);
                var buffer = new byte[4096];

                foreach (string file in fileNames)
                {
                    var entry = new ZipEntry(Path.GetFileName(file))
                    {
                        DateTime = DateTime.Now
                    };

                    s.PutNextEntry(entry);
                    using (FileStream fs = File.OpenRead(file))
                    {
                        int sourceBytes;
                        do
                        {
                            sourceBytes = fs.Read(buffer, 0, buffer.Length);
                            s.Write(buffer, 0, sourceBytes);
                        }
                        while (sourceBytes > 0);
                    }
                }
                s.Finish();
                s.Close();
            }
        }
    }
}
