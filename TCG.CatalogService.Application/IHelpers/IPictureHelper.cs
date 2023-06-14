namespace TCG.CatalogService.Application.IHelpers
{
    public interface IPictureHelper
    {
        public Task<string> SavePictureToAzure(string nomFichier, byte[] imageBytes, string blobStorageContainerName);

        public byte[] GetBytes(string path);
    }
}
