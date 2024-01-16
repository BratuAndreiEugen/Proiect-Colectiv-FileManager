using Amazon.Runtime;
using Amazon.S3;
using fileAPI.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace fileAPI.Infrastructure
{
    public class S3StorageHandler : IS3StorageHandler
    {
        private readonly ConnectionStrings _connectionStrings;

        private readonly AppSettings _appSettings;

        public S3StorageHandler(IOptions<ConnectionStrings> connectionStrings, IOptions<AppSettings> appSettings)
        {
            _connectionStrings = connectionStrings.Value;
            _appSettings = appSettings.Value;
        }

        public async Task<string> UploadToStorage(IFormFile file)
        {
            // Create an S3 client as shown in the previous answer.
            var awsCredentials = new Amazon.Runtime.BasicAWSCredentials(_appSettings.AWSAccessKey,  _appSettings.AWSSecretKey);
            var s3Client = new Amazon.S3.AmazonS3Client(awsCredentials, Amazon.RegionEndpoint.EUCentral1);

            // Specify the S3 bucket and object key where you want to store the image.
            string bucketName = "baxbaniibucketv2";
            string objectKey = "content/" + Guid.NewGuid() + Path.GetExtension(file.FileName);

            using (var stream = file.OpenReadStream())
            {
                var request = new Amazon.S3.Model.PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = objectKey,
                    InputStream = stream,
                    ContentType = file.ContentType,
                    CannedACL = S3CannedACL.PublicRead
                };

                var response = await s3Client.PutObjectAsync(request);

                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    // Image was successfully uploaded to S3.
                    // You can return a success response or perform other actions as needed.
                    return _connectionStrings.S3Storage + "/" + objectKey;
                }
                else
                {
                    // Handle the case where the upload failed.
                    throw new Exception(response.HttpStatusCode.ToString());
                }
            }
        }
    }
}
