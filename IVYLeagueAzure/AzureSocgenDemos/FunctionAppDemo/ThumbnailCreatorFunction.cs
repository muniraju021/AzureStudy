using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text;

namespace FunctionAppDemo
{
    public static class ThumbnailCreatorFunction
    {
        private static readonly string SqlDatabaseConnectionString = "Server=demosqlinstance.database.windows.net;Database=productsdb;User Id=dssadmin;Password=Innovatus$1;";

        [FunctionName("ThumbnailCreatorFunction")]
        public static void Run([QueueTrigger("thumbnailstoragequeuename")] Thumbnail thumbnail,
            [Blob("{BrandId}/{BlobName}", System.IO.FileAccess.Read)] Stream input,
            [Blob("{BrandId}/{BlobNameWithoutExtension}_thumbnail.png", FileAccess.ReadWrite)] CloudBlockBlob outputBlob,
            ILogger log)
        {
            log.LogInformation($"Queue trigger function invoked");
            using (var output = outputBlob.OpenWriteAsync())
            {
                ConvertImageToThumbnailJPG(input, output.Result, outputBlob);
                // outputBlob.Properties.ContentType = "image/jpeg";
                log.LogInformation($"Output Blob Uri: {outputBlob.Uri.ToString()}");
                UpdateThumbnailBlobUri(thumbnail, outputBlob.Uri.ToString(), log);
            }

            log.LogInformation($"C# Queue trigger function processed: {thumbnail}");
        }

        private static void UpdateThumbnailBlobUri(Thumbnail thumbnail, string outputUri, ILogger log)
        {
            log.LogInformation($"strarting to update thumbnail uriBlob Uri");
            var query = $"UPDATE ProductItem SET thumbnailPath='{outputUri}' where Name='{thumbnail.ProductName}' and ProductBrandId='{thumbnail.BrandId}'";
            log.LogInformation($"strarting to update thumbnail uriBlob Uri with query {query}");

            var sqlConnection = new SqlConnection(SqlDatabaseConnectionString);
            var sqlCommand = new SqlCommand(query, sqlConnection);
            sqlConnection.Open();
            _ = sqlCommand.ExecuteNonQueryAsync();
        }

        private static Stream ConvertImageToThumbnailJPG(Stream input, Stream output, CloudBlockBlob outputBlob)
        {
            int thumbnailsize = 80;
            int width;
            int height;
            var originalImage = new Bitmap(input);

            if (originalImage.Width > originalImage.Height)
            {
                width = thumbnailsize;
                height = thumbnailsize * originalImage.Height / originalImage.Width;
            }
            else
            {
                height = thumbnailsize;
                width = thumbnailsize * originalImage.Width / originalImage.Height;

            }
            Bitmap thumbnailImage = null;
            try
            {
                thumbnailImage = new Bitmap(width, height);

                using (Graphics graphics = Graphics.FromImage(thumbnailImage))
                {
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    graphics.DrawImage(originalImage, 0, 0, width, height);
                }

                using (MemoryStream ms = new MemoryStream())
                {
                    thumbnailImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    ms.Seek(0, 0);
                    outputBlob.UploadFromStreamAsync(ms).Wait();
                }
            }
            finally
            {
                if (thumbnailImage != null)
                {
                    thumbnailImage.Dispose();
                }
            }

            return output;

        }

    }
}
