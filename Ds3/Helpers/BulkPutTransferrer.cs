﻿/*
 * ******************************************************************************
 *   Copyright 2014 Spectra Logic Corporation. All Rights Reserved.
 *   Licensed under the Apache License, Version 2.0 (the "License"). You may not use
 *   this file except in compliance with the License. A copy of the License is located at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 *   or in the "license" file accompanying this file.
 *   This file is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
 *   CONDITIONS OF ANY KIND, either express or implied. See the License for the
 *   specific language governing permissions and limitations under the License.
 * ****************************************************************************
 */

using System;
using System.Collections.Generic;
using System.Linq;

using Ds3.Calls;
using Ds3.Models;
using Transferrer = Ds3.Helpers.BulkTransferExecutor.Transferrer;
using ObjectPutter = Ds3.Helpers.Ds3ClientHelpers.ObjectPutter;

namespace Ds3.Helpers
{
    class BulkPutTransferrer : Transferrer
    {
        private readonly IDs3Client client;
        private readonly ObjectPutter putter;

        public BulkPutTransferrer(IDs3Client client, ObjectPutter putter)
        {
            this.client = client;
            this.putter = putter;
        }

        public BulkResponse Prime(string bucket, IEnumerable<Ds3Object> ds3Objects)
        {
            return this.client.BulkPut(new BulkPutRequest(bucket, ds3Objects.ToList()));
        }

        public void Transfer(Guid jobId, string bucket, Ds3Object ds3Object)
        {
            using (this.client.PutObject(new PutObjectRequest(bucket, ds3Object.Name, putter(ds3Object))))
            {
            }
        }
    }
}
