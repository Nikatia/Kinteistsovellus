using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Azure.Storage.Blobs.Models;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;
using Azure.Storage.Blobs;

namespace Kiinteistosovellus
{
    public class ImageService
    {
        public async Task<string> UploadImageAsync(HttpPostedFileBase imageToUpload)
        {
            string imageFullPath = null;
            if (imageToUpload == null || imageToUpload.ContentLength == 0)
            {
                return null;
            }
            try
            {


                CloudStorageAccount cloudStorageAccount = ConnectionString.GetConnectionString();
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                ServiceProperties blobServiceProperties = cloudBlobClient.GetServiceProperties();//Lisätty jälkeenpäin
                blobServiceProperties.Cors = new CorsProperties(); //Lisätty jälkeenpäin
                blobServiceProperties.Cors.CorsRules.Add(new CorsRule()//Lisätty jälkeenpäin
                {
                    AllowedHeaders = new List<string>() { "*" },
                    AllowedMethods = CorsHttpMethods.Put | CorsHttpMethods.Get | CorsHttpMethods.Head | CorsHttpMethods.Post,
                    AllowedOrigins = new List<string>() { "http://corsazurestorage.azurewebsites.net" },
                    ExposedHeaders = new List<string>() { "*" },
                    MaxAgeInSeconds = 1800 // 30 minutes
                });
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("pics");

                if (await cloudBlobContainer.CreateIfNotExistsAsync())
                {
                    await cloudBlobContainer.SetPermissionsAsync(
                    new BlobContainerPermissions
                    {
                        PublicAccess = BlobContainerPublicAccessType.Blob
                    }
                    );
                }
                // string imageName = Guid.NewGuid().ToString() + "-" + Path.GetExtension(imageToUpload.FileName);
                string imageName = Path.GetFileName(imageToUpload.FileName);
                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(imageName);
                cloudBlockBlob.Properties.ContentType = imageToUpload.ContentType;
                await cloudBlockBlob.UploadFromStreamAsync(imageToUpload.InputStream);

                imageFullPath = cloudBlockBlob.Uri.ToString();
            }
            catch (Exception ex)
            {

            }
            return imageFullPath;
        }
    }
}
