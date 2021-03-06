﻿/*
 * ******************************************************************************
 *   Copyright 2014-2017 Spectra Logic Corporation. All Rights Reserved.
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

using System.Collections.Generic;
using System.IO;
using Ds3;
using Ds3.Helpers.TransferStrategies;
using Ds3.Runtime;
using Moq;
using NUnit.Framework;
using Range = Ds3.Models.Range;

namespace TestDs3.Helpers.TransferStrategies
{
    [TestFixture]
    internal class TestPartialDataTransferStrategyDecorator
    {
        [Test]
        public void Test1Retries()
        {
            var client = new Mock<IDs3Client>(MockBehavior.Strict);
            MockHelpers.SetupGetObjectWithContentLengthMismatchException(client, "bar", 0L, "ABCDEFGHIJ", 20L, 10L);
            MockHelpers.SetupGetObjectWithContentLengthMismatchException(client, "bar", 0L, "ABCDEFGHIJ", 20L, 10L,
                Range.ByPosition(9, 19));
            try
            {
                var stream = new MemoryStream(200);
                var exceptionTransferStrategy = new ReadTransferStrategy();
                var retries = 1;
                var decorator = new PartialDataTransferStrategyDecorator(exceptionTransferStrategy, retries);
                decorator.Transfer(new TransferStrategyOptions
                {
                    Client = client.Object,
                    BucketName = JobResponseStubs.BucketName,
                    ObjectName = "bar",
                    BlobOffset = 0,
                    JobId = JobResponseStubs.JobId,
                    Ranges = new List<Range>(),
                    Stream = stream,
                    ObjectTransferAttempts = retries
                });
                Assert.Fail();
            }
            catch (Ds3NoMoreRetransmitException ex)
            {
                var expectedMessage = string.Format(Resources.NoMoreRetransmitException, "1", "bar", "0");
                Assert.AreEqual(expectedMessage, ex.Message);

                Assert.AreEqual(1, ex.Retries);
            }
        }

        [Test]
        public void TestZeroRetries()
        {
            var client = new Mock<IDs3Client>(MockBehavior.Strict);
            MockHelpers.SetupGetObjectWithContentLengthMismatchException(client, "bar", 0L, "ABCDEFGHIJ", 20L, 10L);
                // The initial request is for all 20 bytes, but only the first 10 will be sent

            try
            {
                var stream = new MemoryStream(200);
                var exceptionTransferStrategy = new ReadTransferStrategy();
                var decorator = new PartialDataTransferStrategyDecorator(exceptionTransferStrategy, 0);
                decorator.Transfer(new TransferStrategyOptions
                {
                    Client = client.Object,
                    BucketName = JobResponseStubs.BucketName,
                    ObjectName = "bar",
                    BlobOffset = 0,
                    JobId = JobResponseStubs.JobId,
                    Ranges = new List<Range>(),
                    Stream = stream,
                    ObjectTransferAttempts = 0
                });
                Assert.Fail();
            }
            catch (Ds3NoMoreRetransmitException ex)
            {
                var expectedMessage = string.Format(Resources.NoMoreRetransmitException, "0", "bar", "0");
                Assert.AreEqual(expectedMessage, ex.Message);

                Assert.AreEqual(0, ex.Retries);
            }
        }
    }
}