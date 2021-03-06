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

using System.Net;
using Ds3.Models;
using System.Collections.Generic;

namespace Ds3.Runtime
{
    public class Ds3BadStatusCodeException : Ds3RequestException
    {
        private readonly HttpStatusCode _statusCode;
        private readonly Error _error;
        private readonly string _responseBody;

        public HttpStatusCode StatusCode
        {
            get { return _statusCode; }
        }

        public Error Error
        {
            get { return _error; }
        }

        public string ResponseBody
        {
            get { return _responseBody; }
        }

        public Ds3BadStatusCodeException(IEnumerable<HttpStatusCode> expectedStatusCodes, HttpStatusCode receivedStatusCode, Error error, string responseBody)
            : base(StatusCodeMessage(expectedStatusCodes, receivedStatusCode, error))
        {
            this._statusCode = receivedStatusCode;
            this._error = error;
            this._responseBody = responseBody;
        }

        private static string StatusCodeMessage(IEnumerable<HttpStatusCode> expectedStatusCodes, HttpStatusCode receivedStatusCode, Error error)
        {
            var expectedCodesString = string.Join(", ", expectedStatusCodes);
            if (error == null)
            {
                return string.Format(Resources.BadStatusCodeInvalidErrorResponseException, receivedStatusCode, expectedCodesString);
            }
            else
            {
                return string.Format(Resources.BadStatusCodeException, receivedStatusCode, expectedCodesString, error.Message);
            }
        }
    }
}
