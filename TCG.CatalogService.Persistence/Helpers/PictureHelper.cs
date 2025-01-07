using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Net;
using TCG.CatalogService.Application.IHelpers;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using TCG.Common.Settings;

namespace TCG.CatalogService.Persistence.Helpers
{
    public class PictureHelper : IPictureHelper
    {
        private readonly AWSSettings _awsSettings;

        public PictureHelper(IOptions<AWSSettings> awsSettings)
        {
            _awsSettings = awsSettings.Value;
        }

        public async Task<string> SavePictureToAWSS3(string nomFichier, byte[] imageBytes, string bucketName)
        {
            string accessKey = _awsSettings.AccessKey;
            string secretKey = _awsSettings.SecretKey;
            string serviceUrl = _awsSettings.ServiceUrl;
            
            var config = new AmazonS3Config
            {
                ServiceURL = serviceUrl,
                ForcePathStyle = true,
                AuthenticationRegion = "eu-north-1",
            };
            using var client = new AmazonS3Client(accessKey, secretKey, config);
            try
            {
                using var stream = new MemoryStream(imageBytes);
                var putRequest = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = nomFichier + ".webp",
                    InputStream = stream,
                    ContentType = "image/webp"
                };

                // Exécutez l'opération d'upload
                await client.PutObjectAsync(putRequest);

                // Construisez l'URL de l'image. Cette URL dépendra de votre configuration de bucket S3
                return $"https://{bucketName}.s3.eu-north-1.amazonaws.com/{nomFichier}.webp";
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public byte[] GetBytes(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                using (WebClient client = new WebClient())
                {
                    byte[] imageData = client.DownloadData(new Uri(path));
                    return imageData;
                }
            }
            else
            {
                return null;
            }
        }

        #region OVH - Azure
        // private string blobStorageConnectionString =
        //     "DefaultEndpointsProtocol=https;AccountName=cscimagestore;AccountKey=os+cJJGnsCQ5/b4e1XoJg4s4g3PF+rdFZiz+PE4U1UTpykRSIZHCK31P7QDVErs36/8Tswbexuf6+AStI0cFKQ==;EndpointSuffix=core.windows.net";
        //
        // public async Task<string> SavePictureToAzure(string nomFichier, byte[] imageBytes, string blobStorageContainerName)
        // {
        //     var container = new BlobContainerClient(blobStorageConnectionString, blobStorageContainerName);
        //
        //     try
        //     {
        //         // If the blob already exists, this will overwrite it.
        //         var blob = container.GetBlobClient(nomFichier + ".webp");
        //         using (var stream = new MemoryStream(imageBytes))
        //         {
        //             var blobUploadOptions = new BlobUploadOptions()
        //             {
        //                 HttpHeaders = new BlobHttpHeaders()
        //                 {
        //                     ContentType = "image/webp"
        //                 }
        //             };
        //             await blob.UploadAsync(stream, blobUploadOptions);
        //             return blob.Uri.ToString();
        //         }
        //     }
        //     catch (Exception)
        //     {
        //         throw;
        //     }
        // }
        
        // public async Task<string> SavePictureToOVHS3(string nomFichier, byte[] imageBytes, string bucketName)
        // {
        //     // Ces valeurs doivent être définies en fonction de votre configuration OVH.
        //     string accessKey = "241a816ca43349f69ecb9da5c1e5d733";
        //     string secretKey = "6b8fc455dfb3477e9891d28db02a1d3b";
        //     string serviceUrl = "https://s3.gra.io.cloud.ovh.net/"; // Remplacez par l'URL appropriée si nécessaire.
        //
        //     var config = new AmazonS3Config
        //     {
        //         ServiceURL = serviceUrl,
        //         ForcePathStyle = true,
        //         AuthenticationRegion = "gra",
        //     };
        //     using var client = new AmazonS3Client(accessKey, secretKey, config);
        //
        //     try
        //     {
        //         using var stream = new MemoryStream(imageBytes);
        //         var putRequest = new PutObjectRequest
        //         {
        //             BucketName = bucketName,
        //             Key = nomFichier + ".webp",
        //             InputStream = stream,
        //             ContentType = "image/webp"
        //         };
        //
        //         //await client.PutObjectAsync(putRequest);
        //
        //         return $"https://tcgplaceblob.s3.gra.io.cloud.ovh.net/{nomFichier}.webp";
        //     }
        //     catch (Exception)
        //     {
        //         throw;
        //     }
        // }
        #endregion
    }
}
