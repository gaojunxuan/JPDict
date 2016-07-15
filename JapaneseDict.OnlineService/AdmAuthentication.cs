/* Copyright 2012 Marco Minerva, marco.minerva@gmail.com

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Net;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace JapaneseDict.OnlineService
{
    [DataContract]
    internal class AdmAccessToken
    {
        [DataMember]
        public string access_token { get; set; }
        [DataMember]
        public string token_type { get; set; }
        [DataMember]
        public string expires_in { get; set; }
        [DataMember]
        public string scope { get; set; }
    }

    internal class AdmAuthentication
    {
        private const string DATAMARKET_ACCESS_URI = "https://datamarket.accesscontrol.windows.net/v2/OAuth2-13";
        private string clientID;
        private string cientSecret;
        private string request;

        public AdmAuthentication(string clientID, string clientSecret)
        {
            this.clientID = clientID;
            this.cientSecret = clientSecret;
            //If clientid or client secret has special characters, encode before sending request
            this.request = string.Format("grant_type=client_credentials&client_id={0}&client_secret={1}&scope=http://api.microsofttranslator.com", Uri.EscapeDataString(clientID), Uri.EscapeDataString(clientSecret));
        }

        public async Task<AdmAccessToken> GetAccessToken()
        {
            //Prepare OAuth request 
            WebRequest webRequest = WebRequest.Create(DATAMARKET_ACCESS_URI);
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "POST";
            byte[] bytes = Encoding.UTF8.GetBytes(this.request);            
            using (Stream outputStream = await webRequest.GetRequestStreamAsync())
                outputStream.Write(bytes, 0, bytes.Length);

            using (WebResponse webResponse = await webRequest.GetResponseAsync())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(AdmAccessToken));
                //Get deserialized object from JSON stream
                AdmAccessToken token = await Task.Run(() => (AdmAccessToken)serializer.ReadObject(webResponse.GetResponseStream()));
                return token;
            }       
        }
    }
}
