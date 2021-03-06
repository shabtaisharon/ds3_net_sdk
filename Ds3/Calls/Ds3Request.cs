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

using Ds3.Models;
using Ds3.Runtime;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ds3.Calls
{
    public abstract class Ds3Request
    {
        internal abstract HttpVerb Verb
        {
            get;
        }
        
        internal abstract string Path
        {
            get;   
        }

        internal virtual long GetContentLength()
        {
            return 0;
        }

        internal virtual Stream GetContentStream()
        {
            return Stream.Null;
        }

        internal virtual ChecksumType ChecksumValue
        {
            get { return ChecksumType.None; }
        }

        internal virtual ChecksumType.Type CType
        {
            get { return ChecksumType.Type.NONE; }
        }

        internal virtual IEnumerable<Range> GetByteRanges()
        {
            return Enumerable.Empty<Range>();
        }

        private Dictionary<string, string> _queryParams = new Dictionary<string, string>();
        internal virtual Dictionary<string,string> QueryParams
        {
            get { return _queryParams; }
        }

        private IRequestHeaders _headers = InitRequestHeaders();

        internal static IRequestHeaders InitRequestHeaders()
        {
            IRequestHeaders rh = new RequestHeaders();
            rh.Add("Naming-Convention", "s3");
            return rh;
        }

        internal virtual IRequestHeaders Headers
        {
            get { return _headers; }
        }

        public void AddHeaders(IDictionary<string, string> headers)
        {
            foreach (var pair in headers)
            {
                AddHeader(pair.Key, pair.Value);
            }
        }

        public void AddHeader(string key, string value)
        {
            _headers.Add(key, value);
        }

        public string getDescription(string paramstring)
        {
            return string.Format(" | {0} {1}{2}{3}", this.Verb.ToString(), this.Path, string.IsNullOrEmpty(paramstring) ? "" : "?", string.IsNullOrEmpty(paramstring) ? "" : paramstring);
        }
    }

    internal enum HttpVerb {GET, PUT, POST, DELETE, HEAD, PATCH};
}
