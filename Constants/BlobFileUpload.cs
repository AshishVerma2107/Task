using System;
using System.Threading.Tasks;
using Android.Util;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace TaskAppWithLogin.Constants
{
    class BlobFileUpload
    {

        public BlobFileUpload()
        {

        }

        public async Task<string> UploadPhotoAsync(byte[] photobytes, string photoName)
        {
            try
            {
                string connectionString = string.Format("DefaultEndpointsProtocol=https;AccountName=abvplnewvmrdiag487;AccountKey=UwRnwC3EFSBSOWlfma+vFGGTAP0XIf0zoAgujt/8qHzepM1hFAzAiNZbsm363E+ppgozldjRqD1SGiW+0TI7Cw==;EndpointSuffix=core.windows.net");
                CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
                CloudBlobClient client = account.CreateCloudBlobClient();
                CloudBlobContainer cloudBlobContainer = client.GetContainerReference("work121");
                if (await cloudBlobContainer.CreateIfNotExistsAsync())
                {
                    await cloudBlobContainer.SetPermissionsAsync(
                        new BlobContainerPermissions
                        {
                            PublicAccess = BlobContainerPublicAccessType.Blob
                        }
                        );
                }

                CloudBlockBlob photo = cloudBlobContainer.GetBlockBlobReference("TaskApp/" + photoName);
                await photo.UploadFromByteArrayAsync(photobytes, 0, photobytes.Length);
                return photo.Uri.ToString();
            }
            catch (Exception e)
            {
                Log.Error("Error", e.Message);
                return null;
            }


        }
    }
}