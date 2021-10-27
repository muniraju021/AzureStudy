using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FunctionAppDemo
{
    public class Thumbnail
    {
        public Uri ImageUrl { get; set; }
        public string BlobName
        {
            get
            {
                return ImageUrl.Segments[ImageUrl.Segments.Length - 1];
            }

        }

        public string BlobNameWithoutExtension
        {
            get
            {
                return Path.GetFileNameWithoutExtension(BlobName);
            }
        }

        public string BrandId { get; set; }
        public string ProductName { get; set; }
        
    }
}
