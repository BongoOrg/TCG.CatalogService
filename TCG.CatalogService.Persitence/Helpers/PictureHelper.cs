using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Net;
using TCG.CatalogService.Application.IHelpers;

namespace TCG.CatalogService.Persitence.Helpers
{
    public class PictureHelper : IPictureHelper
    {
        private string blobStorageConnectionString =
            "DefaultEndpointsProtocol=https;AccountName=cscimagestore;AccountKey=os+cJJGnsCQ5/b4e1XoJg4s4g3PF+rdFZiz+PE4U1UTpykRSIZHCK31P7QDVErs36/8Tswbexuf6+AStI0cFKQ==;EndpointSuffix=core.windows.net";
        
        public async Task<string> SavePictureToAzure(string nomFichier, byte[] imageBytes, string blobStorageContainerName)
        {
            var container = new BlobContainerClient(blobStorageConnectionString, blobStorageContainerName);

            try
            {
                // If the blob already exists, this will overwrite it.
                var blob = container.GetBlobClient(nomFichier + ".webp");
                using (var stream = new MemoryStream(imageBytes))
                {
                    var blobUploadOptions = new BlobUploadOptions()
                    {
                        HttpHeaders = new BlobHttpHeaders()
                        {
                            ContentType = "image/webp"
                        }
                    };
                    await blob.UploadAsync(stream, blobUploadOptions);
                    return blob.Uri.ToString();
                }
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
    }
}
