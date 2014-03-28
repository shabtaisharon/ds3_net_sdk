﻿using System.Net;

namespace Ds3.Models
{
    public class DeleteBucketResponse : Ds3Response
    {
        public DeleteBucketResponse(HttpWebResponse response)
            : base(response)
        {
            HandleStatusCode(HttpStatusCode.NoContent);
        }
    }
}
