﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

using System.Net;
using System.Net.Http;

using Ds3.Models;

namespace Ds3.Runtime
{
    class Network
    {
        public static async Task<T> Invoke<T, K>(K request, Uri endpoint, Credentials creds) where T: Ds3Response where K : Ds3Request
        {
            bool redirect = false;
            int redirectCount = 0;
            int maxRedirects = 5;

            do {
                DateTime date = DateTime.UtcNow;
                UriBuilder uriBuilder = new UriBuilder(endpoint);            
                uriBuilder.Path = request.Path;

                if (request.QueryParams.Count > 0)
                {
                    uriBuilder.Query = buildQueryParams(request.QueryParams);
                }

                HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(uriBuilder.ToString());
                httpRequest.Method = request.Verb.ToString();
                httpRequest.Date = date;
                httpRequest.Host = endpoint.Host;
                httpRequest.AllowAutoRedirect = false;
                httpRequest.Headers.Add("Authorization", S3Signer.AuthField(creds, request.Verb.ToString(), date.ToString("r"), request.Path));

                if (request.Verb == HttpVerb.PUT || request.Verb == HttpVerb.POST)
                {
                    using (Stream content = request.getContentStream()) {
                        httpRequest.ContentLength = content.Length;                    
                        using (Stream requestStream = httpRequest.GetRequestStream())
                        {
                            if (content != Stream.Null)
                            {
                                content.CopyTo(requestStream);
                                requestStream.Flush();
                            }                        
                        }
                    }
                }
            
                try
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)await httpRequest.GetResponseAsync().ConfigureAwait(false);
                    if (is307(httpResponse))
                    {
                        redirect = true;
                        redirectCount++;
                        continue;
                    }
                    return CreateResponseInstance<T>(httpResponse);
                }
                catch (WebException e)
                {                   
                    return CreateResponseInstance<T>((HttpWebResponse)e.Response);
                }   
            }while(redirect && redirectCount < maxRedirects);
            
            throw new Exception("Too many redirects.");            
        }

        private static bool is307(HttpWebResponse httpResponse)
        {
            return httpResponse.StatusCode.Equals(HttpStatusCode.TemporaryRedirect);
        }

        private static T CreateResponseInstance<T>(HttpWebResponse content)
        {
            Type type = typeof(T);            
            return (T)Activator.CreateInstance(type, content);
        }

        private static string FormatedDateString()
        {
            return DateTime.Now.ToString("ddd, dd MMM yyyy HH:mm:ss K");
        }

        private static string buildQueryParams(Dictionary<string, string> queryParams)
        {
            List<string> queryList = queryParams.Select(kvp => kvp.Key + "=" + kvp.Value).ToList();
            return String.Join("&", queryList);            
        }
    }
}
