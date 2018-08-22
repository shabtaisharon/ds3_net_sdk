﻿/*
 * ******************************************************************************
 *   Copyright 2014-2016 Spectra Logic Corporation. All Rights Reserved.
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

using Ds3;
using Ds3.Helpers;
using System;
using System.Configuration;
using System.Diagnostics;

namespace Ds3Examples
{
    /// <summary>
    /// Shows how to transfer all of the files in a directory to a bucket
    /// using the bulk job helpers as well as the file helpers.
    /// </summary>
    class BulkPutExample
    {
        private static TraceSwitch clientSwitch = new TraceSwitch("clientSwitch", "Controls tracing for example client");
        
        static void Main(string[] args)
        {
            // Configure and build the core client.
            IDs3Client client = new Ds3Builder(
                ConfigurationManager.AppSettings["Ds3Endpoint"],
                new Credentials(
                    ConfigurationManager.AppSettings["Ds3AccessKey"],
                    ConfigurationManager.AppSettings["Ds3SecretKey"]
                )
            ).Build();


            client.ModifyDataPolicySpectraS3(new Ds3.Calls.ModifyDataPolicySpectraS3Request(new Guid("4ed7eb7b-cc03-4ea9-b15d-010983862951")).WithDefaultBlobSize(1024 * 1024 * 1024));

            //// Set up the high-level abstractions.
            //IDs3ClientHelpers helpers = new Ds3ClientHelpers(client);

            //string bucket = "bucket-name";
            //string directory = "TestData";

            //// Creates a bucket if it does not already exist.
            //helpers.EnsureBucketExists(bucket);
            //if (clientSwitch.TraceVerbose) { Trace.WriteLine(string.Format("Bucket exists: {0}", bucket)); }

            //// Creates a bulk job with the server based on the files in a directory (recursively).
            //IJob job = helpers.StartWriteJob(bucket, FileHelpers.ListObjectsForDirectory(directory));

            //// Tracing example 
            //if (clientSwitch.TraceInfo) { Trace.WriteLine(string.Format("StartWriteJob({0})", bucket)); }
            //if (clientSwitch.TraceVerbose) { Trace.WriteLine(string.Format("Add files from: {0}", directory)); }

            //// Keep the job id around. This is useful for job recovery in the case of a failure.
            //Console.WriteLine("Job id {0} started.", job.JobId);

            //// Transfer all of the files.
            //job.Transfer(FileHelpers.BuildFilePutter(directory));

            // Wait for user input.
            Console.WriteLine("Press enter to continue.");
            Console.ReadLine();
        }
    }
}
