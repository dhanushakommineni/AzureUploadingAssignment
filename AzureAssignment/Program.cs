using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Neudesic.AzureAssignment
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["AzureStorageAccount"].ConnectionString;
            string localFolder = ConfigurationManager.AppSettings["Source"];
            string destinationContainer = ConfigurationManager.AppSettings["Destination"];
            Console.WriteLine(@"Connection to storage account");
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            Console.WriteLine(@"Getting reference to container");
            CloudBlobContainer container = blobClient.GetContainerReference(destinationContainer);
            string[] fileEntries = Directory.GetFiles(localFolder);
            foreach(string filepath in fileEntries)
            {
                string key = Path.GetFileName(filepath);
                UploadBlob(container, key, filepath, true);
                Console.WriteLine(@"Upload process complete.Press any key to exit");
                Console.ReadKey();
            }
        }
        static void UploadBlob(CloudBlobContainer container, string key, string fileName, bool deleteAfter)
        {
            Console.WriteLine(@"Uploading file to container : key=" + key + "source file" + fileName);
            CloudBlockBlob cloudBlob = container.GetBlockBlobReference(key);
            using (var file = System.IO.File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                cloudBlob.UploadFromStream(file);
            }
        }
    }
}
