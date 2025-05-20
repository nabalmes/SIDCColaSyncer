using Newtonsoft.Json;
using SIDCColaSyncer.Model.DTOs.SIDCAPIs;
using SIDCColaSyncer.Model.DTOs.SIDCAPIs.ColaAPI.GetColaStub;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SIDCColaSyncer.Infrastracture.Shared.Services
{

    public class SIDCAPIService
    {
        public SIDCApiSettings _sidcApiSettings { get; }
        private string webAddr = null;

        public string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public string PasswordDecode(string passwordEncoded)
        {

            var passwordDecoded = Convert.FromBase64String(passwordEncoded);
            return Encoding.UTF8.GetString(passwordDecoded);
        }

        public SIDCAPIService()
        {
            _sidcApiSettings = new SIDCApiSettings();
            _sidcApiSettings.BaseUrl = Properties.Settings.Default.BASE_URL;
            _sidcApiSettings.ColaStubUrl = Properties.Settings.Default.COLA_STUB_URL;
        }


        //public string SendAuthentication()
        //{
        //    SIDCTokenRequest sidcTokenRequest = new SIDCTokenRequest()
        //    {
        //        Username = _sidcApiSettings.AuthUser,
        //        Password = _sidcApiSettings.AuthPassword,
        //        IsAdmin = _sidcApiSettings.AuthIsAdmin,
        //    };
        //    string token = string.Empty;
        //    try
        //    {
        //        webAddr = string.Format("{0}{1}", _sidcApiSettings.BaseUrl, _sidcApiSettings.AuthTokenUrl);

        //        WebRequest request = WebRequest.Create(webAddr);

        //        request.Method = "POST";
        //        string postData = JsonConvert.SerializeObject(sidcTokenRequest);

        //        byte[] byteArray = Encoding.UTF8.GetBytes(postData);

        //        request.ContentType = "application/json";

        //        request.ContentLength = byteArray.Length;

        //        Stream dataStream = request.GetRequestStream();

        //        dataStream.Write(byteArray, 0, byteArray.Length);

        //        dataStream.Close();

        //        WebResponse response = request.GetResponse();

        //        //Console.WriteLine(((HttpWebResponse)response).StatusDescription);

        //        using (dataStream = response.GetResponseStream())
        //        {
        //            StreamReader reader = new StreamReader(dataStream);

        //            string responseFromServer = reader.ReadToEnd();

        //            var sidcTokenResponse = JsonConvert.DeserializeObject<SIDCTokenResponse>(responseFromServer);

        //            token = sidcTokenResponse.Data.JwToken.ToString();
        //        }
        //        response.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    return token;
        //}

        public ColaStubApiResponse GetColaStub(ColaStubRequest colaStubRequest)
        {
            ColaStubApiResponse colaStubResponse = null;

            webAddr = string.Format("{0}{1}", _sidcApiSettings.BaseUrl, _sidcApiSettings.ColaStubUrl);
            webAddr += String.Format("?branchCode={0}",
                colaStubRequest.BranchCode
                );
            WebRequest request = WebRequest.Create(webAddr);
            //request.Headers.Add("Authorization", "Bearer " + token);
            request.Method = "GET";

            using (WebResponse response = request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream);
                    string responseFromServer = reader.ReadToEnd();

                    colaStubResponse = JsonConvert.DeserializeObject<ColaStubApiResponse>(responseFromServer);
                }
                return colaStubResponse;
            }
        }
    }
}
