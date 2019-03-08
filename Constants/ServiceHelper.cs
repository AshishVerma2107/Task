using System;
using System.Collections.Generic;
using System.IO;
using System.Json;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using TaskAppWithLogin.Models;

namespace TaskAppWithLogin.Constants
{

    class ServiceHelper
    {
        HttpClient client;
        HttpWebRequest request;
        Cryptography cryptography;

        string licenceId, UserId, AppDateTime;
        public ServiceHelper()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            cryptography = new Cryptography();
            //AppDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        }

        ISharedPreferences prefs;

        public void init(Context context)
        {
            prefs = PreferenceManager.GetDefaultSharedPreferences(context);
            licenceId = prefs.GetString("LicenceId", "");
            UserId = prefs.GetString("DesignationId", "");
            AppDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public async Task<string> GetLicenceId(Context context, string geoLocation, string version, string jsonData)
        {
            string licenceId = "";
            init(context);

            string url = "http://mobileapi.work121.com/api/AppLogin/GetMethod?associateID=8B280FFF-BFDD-4F62-8D46-08BA937DB981&associatepwd=mA121&licenceId=" + "0" + "&licenceFor=TaskApp&AppVersion=" + version + "&geolocation=" + "0" + "&methodname=GetLicenceID&AppDateTime=" + AppDateTime + "&UserId=" + "0" + " &jsonData=" + jsonData;

            request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";
            request.Headers["userName"] = "uYfVkoP5BDouLkCBZ971sNZhzocdFLhmAvULyvsDnBo=";
            request.Headers["password"] = "/I/tmrWuA6AxGV6CiFgD/1AaOcV+2zzhS6OabhGQXVs=";

            try
            {
                // Send the request to the server and wait for the response:
                using (WebResponse response = await request.GetResponseAsync().ConfigureAwait(false))
                {

                    // Get a stream representation of the HTTP web response:
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        string text = reader.ReadToEnd();
                        //  string decrypted_Text = await OnCryptoAsync(text, "Decryption");
                        ResponseModel responseModel = JsonConvert.DeserializeObject<ResponseModel>(text);
                        if (responseModel.ResponseCode.Equals("Ok"))
                        {
                            string decrypted_Text = await OnCryptoAsync(responseModel.ResponseValue, "Decryption");
                            reader.Close();
                            stream.Close();
                            return decrypted_Text;
                        }
                        else
                        {
                            return responseModel.ResponseCode;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }


        }


        public async Task<JsonValue> LoginUser2(Context context,string version,string userid, string jsonData, string geolocation)
        {
            //Items = new LoginModel();
            JsonValue jsonDoc = null;
            init(context);
            //string url = "http://mobileapi.work121.com/api/Login/Getloginuser?associateID=8B280FFF-BFDD-4F62-8D46-08BA937DB981&associatepwd=mA121&licenceId=" + licenceid + "&UserId=" + userid + "&Password=" + password + "&geolocation=" + geolocation + "&licenceFor=TaskApp";
            string url = "http://mobileapi.work121.com/api/AppLogin/GetMethod?associateID=8B280FFF-BFDD-4F62-8D46-08BA937DB981&associatepwd=mA121&licenceId=" + licenceId + "&licenceFor=TaskApp&AppVersion=" + version + "&geolocation=" + geolocation + "&methodname=Getloginuser&AppDateTime=" + AppDateTime + "&UserId=" + userid + " &jsonData=" + jsonData;
            request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";
            request.Headers["userName"] = "uYfVkoP5BDouLkCBZ971sNZhzocdFLhmAvULyvsDnBo=";
            request.Headers["password"] = "/I/tmrWuA6AxGV6CiFgD/1AaOcV+2zzhS6OabhGQXVs=";

            try
            {
                // Send the request to the server and wait for the response:
                using (WebResponse response = await request.GetResponseAsync().ConfigureAwait(false))
                {

                    // Get a stream representation of the HTTP web response:
                    using (Stream stream = response.GetResponseStream())
                    {

                        StreamReader reader = new StreamReader(stream);
                        string text = reader.ReadToEnd();
                        //  string decrypted_Text = await OnCryptoAsync(text, "Decryption");
                        ResponseModel responseModel = JsonConvert.DeserializeObject<ResponseModel>(text);
                        if (responseModel.ResponseCode.Equals("Ok"))
                        {
                            string decrypted_Text = await OnCryptoAsync(responseModel.ResponseValue, "Decryption");
                            reader.Close();
                            stream.Close();
                            return decrypted_Text;
                        }
                        else
                        {
                            return null;
                        }

                        //StreamReader reader = new StreamReader(stream);
                        //string text = reader.ReadToEnd();
                        //string decrypted_Text = await OnCryptoAsync2(text, "Decryption");
                        //// Use this stream to build a JSON document object:
                        //jsonDoc = await Task.Run(() => JsonObject.Parse(decrypted_Text));
                        //Log.Error("Response: {0}", jsonDoc.ToString());

                        // Return the JSON document:
                        // Items = JsonConvert.SerializeObject(jsonDoc);

                    }
                }
            }
            catch (Exception e)
            {
                return null ;
            }
        }

        public async Task<string> RegisterUser(Context context,string licenceid, string geoLocation, string version, string jsonData)
        {
            string licenceId = "";
            init(context);

            string url = "http://mobileapi.work121.com/api/AppLogin/GetMethod?associateID=8B280FFF-BFDD-4F62-8D46-08BA937DB981&associatepwd=mA121&licenceId=" + licenceid + "&licenceFor=TaskApp&AppVersion=" + version + "&geolocation=" + geoLocation + "&methodname=VerifylicenceId&AppDateTime=" + AppDateTime + "&UserId=" + UserId + " &jsonData=" + jsonData;

            request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";
            request.Headers["userName"] = "uYfVkoP5BDouLkCBZ971sNZhzocdFLhmAvULyvsDnBo=";
            request.Headers["password"] = "/I/tmrWuA6AxGV6CiFgD/1AaOcV+2zzhS6OabhGQXVs=";

            try
            {
                // Send the request to the server and wait for the response:
                using (WebResponse response = await request.GetResponseAsync().ConfigureAwait(false))
                {

                    // Get a stream representation of the HTTP web response:
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        string text = reader.ReadToEnd();
                        //  string decrypted_Text = await OnCryptoAsync(text, "Decryption");
                        ResponseModel responseModel = JsonConvert.DeserializeObject<ResponseModel>(text);
                        if (responseModel.ResponseCode.Equals("Ok"))
                        {
                            //string decrypted_Text = await OnCryptoAsync2(responseModel.ResponseValue, "Decryption");
                            string decrypted_Text = responseModel.ResponseValue;
                            reader.Close();
                            stream.Close();
                            return decrypted_Text;
                        }
                        else
                        {
                            return responseModel.ResponseCode;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }


        }


        public async Task<string> LogoutUser(Context context, string geoLocation, string version, string jsonData)
        {
            init(context);
            string url = "http://mobileapi.work121.com/api/AppLogin/GetMethod?associateID=8B280FFF-BFDD-4F62-8D46-08BA937DB981&associatepwd=mA121&licenceId=" + licenceId + "&licenceFor=TaskApp&AppVersion=" + version + "&geolocation=" + geoLocation + "&methodname=LogoutViaIdentity&AppDateTime=" + AppDateTime + "&UserId=" + UserId + " &jsonData=" + jsonData;

            request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";
            request.Headers["userName"] = "uYfVkoP5BDouLkCBZ971sNZhzocdFLhmAvULyvsDnBo=";
            request.Headers["password"] = "/I/tmrWuA6AxGV6CiFgD/1AaOcV+2zzhS6OabhGQXVs=";
            try
            {
                using (WebResponse response = await request.GetResponseAsync().ConfigureAwait(false))
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        string text = reader.ReadToEnd();
                        ResponseModel responseModel = JsonConvert.DeserializeObject<ResponseModel>(text);
                        if (responseModel.ResponseCode.Equals("Ok"))
                        {
                            //string decrypted_Text = await OnCryptoAsync2(responseModel.ResponseValue, "Decryption");
                            string decrypted_Text = responseModel.ResponseCode;
                            reader.Close();
                            stream.Close();
                            return decrypted_Text;
                        }
                        else
                        {
                            return responseModel.ResponseCode;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }

        }
        public async Task<string> FileTypelist(Context context, string _jsonData, string geolocation)
        {


            init(context);


            string url = "http://mobileapi.work121.com/api/Task/GetMethod?associateID=8B280FFF-BFDD-4F62-8D46-08BA937DB981&" +
                "associatepwd=mA121&licenceId=" + licenceId + "&UserId=" + UserId + "&methodname=GetFileType&jsonData=" + _jsonData + "&AppDateTime=" + AppDateTime+ "&geoLocation=" + geolocation;
            request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";
            request.Headers["userName"] = "uYfVkoP5BDouLkCBZ971sNZhzocdFLhmAvULyvsDnBo=";
            request.Headers["password"] = "/I/tmrWuA6AxGV6CiFgD/1AaOcV+2zzhS6OabhGQXVs=";
            try
            {
                using (WebResponse response = await request.GetResponseAsync().ConfigureAwait(false))
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        string text = reader.ReadToEnd();
                        ResponseModel responseModel = JsonConvert.DeserializeObject<ResponseModel>(text);
                        if (responseModel.ResponseCode.Equals("Ok"))
                        {
                            string decrypted_Text = await OnCryptoAsync(responseModel.ResponseValue, "Decryption");

                            reader.Close();
                            stream.Close();
                            return decrypted_Text;

                        }
                        else
                        {
                            return responseModel.ResponseCode;
                        }

                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }

        }
        public async Task<string> FileExtension(Context context, string _jsonData, string geolocation)
        {


            init(context);


            string url = "http://mobileapi.work121.com/api/Task/GetMethod?associateID=8B280FFF-BFDD-4F62-8D46-08BA937DB981&" +
                "associatepwd=mA121&licenceId=" + licenceId + "&UserId=" + UserId + "&methodname=GetFileExtension&jsonData=" + _jsonData + "&AppDateTime=" + AppDateTime + "&geoLocation=" + geolocation;
            request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";
            request.Headers["userName"] = "uYfVkoP5BDouLkCBZ971sNZhzocdFLhmAvULyvsDnBo=";
            request.Headers["password"] = "/I/tmrWuA6AxGV6CiFgD/1AaOcV+2zzhS6OabhGQXVs=";
            try
            {
                using (WebResponse response = await request.GetResponseAsync().ConfigureAwait(false))
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        string text = reader.ReadToEnd();
                        ResponseModel responseModel = JsonConvert.DeserializeObject<ResponseModel>(text);
                        if (responseModel.ResponseCode.Equals("Ok"))
                        {
                            string decrypted_Text = await OnCryptoAsync(responseModel.ResponseValue, "Decryption");

                            reader.Close();
                            stream.Close();
                            return decrypted_Text;

                        }
                        else
                        {
                            return responseModel.ResponseCode;
                        }

                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }

        }
        public async Task<string> FrequentList(Context context, string _jsonData, string geolocation)
        {

            
            init(context);


            string url = "http://mobileapi.work121.com/api/ContactCentral/GetMethod?associateID=8B280FFF-BFDD-4F62-8D46-08BA937DB981&" +
                "associatepwd=mA121&licenceId=" + licenceId + "&UserId=" + UserId + "&methodname=GetFrequentContact&jsonData=" + _jsonData + "&AppDateTime=" + AppDateTime;
            request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";
            request.Headers["userName"] = "uYfVkoP5BDouLkCBZ971sNZhzocdFLhmAvULyvsDnBo=";
            request.Headers["password"] = "/I/tmrWuA6AxGV6CiFgD/1AaOcV+2zzhS6OabhGQXVs=";
            try
            {
                using (WebResponse response = await request.GetResponseAsync().ConfigureAwait(false))
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        string text = reader.ReadToEnd();
                        ResponseModel responseModel = JsonConvert.DeserializeObject<ResponseModel>(text);
                        if (responseModel.ResponseCode.Equals("Ok"))
                        {
                            string decrypted_Text = await OnCryptoAsync(responseModel.ResponseValue, "Decryption");

                            reader.Close();
                            stream.Close();
                            return decrypted_Text;

                        }
                        else
                        {
                            return responseModel.ResponseCode;
                        }

                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }

        }


        public async Task<string> MarkingList(Context context, string json, string geolocation)
        {
            init(context);

            string url = "http://mobileapi.work121.com/api/Task/GetMethod?associateID=8B280FFF-BFDD-4F62-8D46-08BA937DB981&associatepwd=mA121&licenceId=" + licenceId + "&UserId=" + UserId + "&methodname=GetDesignationListByOrg&jsonData=" + json + "&AppDateTime=" + AppDateTime + "&geoLocation=" + geolocation;
            request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";
            request.Headers["userName"] = "uYfVkoP5BDouLkCBZ971sNZhzocdFLhmAvULyvsDnBo=";
            request.Headers["password"] = "/I/tmrWuA6AxGV6CiFgD/1AaOcV+2zzhS6OabhGQXVs=";
            try
            {
                using (WebResponse response = await request.GetResponseAsync().ConfigureAwait(false))
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        string text = reader.ReadToEnd();
                        //  string decrypted_Text = await OnCryptoAsync(text, "Decryption");
                        ResponseModel responseModel = JsonConvert.DeserializeObject<ResponseModel>(text);
                        if (responseModel.ResponseCode.Equals("Ok"))
                        {
                            string decrypted_Text = responseModel.ResponseValue;
                            reader.Close();
                            stream.Close();
                            return decrypted_Text;
                        }
                        else
                        {
                            return responseModel.ResponseCode;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public async Task<string> CreateTaskMethod(Context context , string jsonData,  string location)
        {

            
            init(context);

            if (jsonData.Equals(""))
            {
                jsonData = "w3ZUSfhMYXujjSK28/4kfw==";
            }
           
            string url = "http://mobileapi.work121.com/api/Task/PostMethod?associateID=8B280FFF-BFDD-4F62-8D46-08BA937DB981&associatepwd=mA121" + "&licenceId=" + licenceId + "&UserId=" + UserId + "&methodname=CreateTask&jsonData=" + jsonData + "&AppDateTime=" + AppDateTime + "&GeoLocation=" + location;
            request = (HttpWebRequest)WebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "POST";
            request.Headers["userName"] = "uYfVkoP5BDouLkCBZ971sNZhzocdFLhmAvULyvsDnBo=";
            request.Headers["password"] = "/I/tmrWuA6AxGV6CiFgD/1AaOcV+2zzhS6OabhGQXVs=";
            request.ContentLength = 0;
            try
            {
                using (WebResponse response = await request.GetResponseAsync().ConfigureAwait(false))
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        string text = reader.ReadToEnd();
                        ResponseModel responseModel = JsonConvert.DeserializeObject<ResponseModel>(text);
                        if (responseModel.ResponseCode.Contains("Ok"))
                        {
                            string decrypted_Text = await OnCryptoAsync(responseModel.ResponseValue, "Decryption");
                            reader.Close();
                            stream.Close();
                            return decrypted_Text;
                        }
                        else
                        {
                            return responseModel.ResponseCode;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }

        }

        //public async Task<JsonValue> DesignationList(string licenceid, string orgId)
        //{
        //    JsonValue jsonDoc = null;
        //    string url = "http://mobileapi.work121.com/api/Task/GetDesignationListByOrg?associateID=8B280FFF-BFDD-4F62-8D46-08BA937DB981&" +
        //        "associatepwd=mA121&licenceId=" + licenceid + "&OrgId=" + orgId;

        //    request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
        //    request.ContentType = "application/json";
        //    request.Method = "GET";
        //    request.Headers["userName"] = "uYfVkoP5BDouLkCBZ971sNZhzocdFLhmAvULyvsDnBo=";
        //    request.Headers["password"] = "/I/tmrWuA6AxGV6CiFgD/1AaOcV+2zzhS6OabhGQXVs=";

        //    try
        //    {
        //        // Send the request to the server and wait for the response:
        //        using (WebResponse response = await request.GetResponseAsync().ConfigureAwait(false))
        //        {
        //            // Get a stream representation of the HTTP web response:
        //            using (Stream stream = response.GetResponseStream())
        //            {
        //                StreamReader reader = new StreamReader(stream);
        //                string text = reader.ReadToEnd();
        //                string decrypted_Text = await OnCryptoAsync(text, "Decryption");
        //                // Use this stream to build a JSON document object:
        //                jsonDoc = await Task.Run(() => JsonObject.Parse(decrypted_Text));
        //                Log.Error("Response: {0}", jsonDoc.ToString());
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Log.Error("Error:", e.Message);
        //    }
        //    return jsonDoc;
        //}

        public async Task<JsonValue> Saveforlater(Context context, string json, string geolocation)
        {
            JsonValue jsonDoc = null;
            init(context);
            //string url = "http://mobileapi.work121.com/api/Task/GetAssignTaskMe?associateID=8B280FFF-BFDD-4F62-8D46-08BA937DB981&associatepwd=mA121&licenceId=" + licenceid + "&userId=" + userId + "&PageNo=" + pageNo + "&PageSize=" + pageSize;
            string url = "http://mobileapi.work121.com/api/Task/GetMethod?associateID=8B280FFF-BFDD-4F62-8D46-08BA937DB981&associatepwd=mA121&licenceId=" + licenceId + "&UserId=" + UserId + "&methodname=GetCreatedTaskForApp&jsonData=" + json + "&AppDateTime=" + AppDateTime + "&geoLocation=" + geolocation;

            request = (HttpWebRequest)HttpWebRequest.
                Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";
            request.Headers["userName"] = "uYfVkoP5BDouLkCBZ971sNZhzocdFLhmAvULyvsDnBo=";
            request.Headers["password"] = "/I/tmrWuA6AxGV6CiFgD/1AaOcV+2zzhS6OabhGQXVs=";

            try
            {
                // Send the request to the server and wait for the response:
                using (WebResponse response = await request.GetResponseAsync().ConfigureAwait(false))
                {

                    // Get a stream representation of the HTTP web response:
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        string text = reader.ReadToEnd();
                        //  string decrypted_Text = await OnCryptoAsync(text, "Decryption");
                        ResponseModel responseModel = JsonConvert.DeserializeObject<ResponseModel>(text);
                        if (responseModel.ResponseCode.Equals("Ok"))
                        {
                            string decrypted_Text = responseModel.ResponseValue;
                            reader.Close();
                            stream.Close();
                            return decrypted_Text;
                        }
                        else
                        {
                            return responseModel.ResponseCode;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return null;
                //Debug.WriteLine("Error:", e.Message);
            }

        }


        public async Task<JsonValue> TaskInbox(Context context, string json, string geolocation)
        {
            JsonValue jsonDoc = null;
            init(context);
            //string url = "http://mobileapi.work121.com/api/Task/GetAssignTaskMe?associateID=8B280FFF-BFDD-4F62-8D46-08BA937DB981&associatepwd=mA121&licenceId=" + licenceid + "&userId=" + userId + "&PageNo=" + pageNo + "&PageSize=" + pageSize;
            string url = "http://mobileapi.work121.com/api/Task/GetMethod?associateID=8B280FFF-BFDD-4F62-8D46-08BA937DB981&associatepwd=mA121&licenceId=" + licenceId + "&UserId=" + UserId + "&methodname=GetAssignTaskMe&jsonData=" + json + "&AppDateTime=" + AppDateTime + "&geoLocation=" + geolocation;

            request = (HttpWebRequest)HttpWebRequest.
                Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";
            request.Headers["userName"] = "uYfVkoP5BDouLkCBZ971sNZhzocdFLhmAvULyvsDnBo=";
            request.Headers["password"] = "/I/tmrWuA6AxGV6CiFgD/1AaOcV+2zzhS6OabhGQXVs=";

            try
            {
                // Send the request to the server and wait for the response:
                using (WebResponse response = await request.GetResponseAsync().ConfigureAwait(false))
                {

                    // Get a stream representation of the HTTP web response:
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        string text = reader.ReadToEnd();
                        //  string decrypted_Text = await OnCryptoAsync(text, "Decryption");
                        ResponseModel responseModel = JsonConvert.DeserializeObject<ResponseModel>(text);
                        if (responseModel.ResponseCode.Equals("Ok"))
                        {
                            string decrypted_Text = responseModel.ResponseValue;
                            reader.Close();
                            stream.Close();
                            return decrypted_Text;
                        }
                        else
                        {
                            return responseModel.ResponseCode;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return null;
                //Debug.WriteLine("Error:", e.Message);
            }

        }
        public async Task<JsonValue> TaskOutbox(Context context, string json, string geolocation)
        {
            JsonValue jsonDoc = null;
            init(context);
            //string url = "http://mobileapi.work121.com/api/Task/GetAssignTaskMe?associateID=8B280FFF-BFDD-4F62-8D46-08BA937DB981&associatepwd=mA121&licenceId=" + licenceid + "&userId=" + userId + "&PageNo=" + pageNo + "&PageSize=" + pageSize;
            string url = "http://mobileapi.work121.com/api/Task/GetMethod?associateID=8B280FFF-BFDD-4F62-8D46-08BA937DB981&associatepwd=mA121&licenceId=" + licenceId + "&UserId=" + UserId + "&methodname=GetTaskMarkByMe&jsonData=" + json + "&AppDateTime=" + AppDateTime + "&geoLocation=" + geolocation;

            request = (HttpWebRequest)HttpWebRequest.
                Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";
            request.Headers["userName"] = "uYfVkoP5BDouLkCBZ971sNZhzocdFLhmAvULyvsDnBo=";
            request.Headers["password"] = "/I/tmrWuA6AxGV6CiFgD/1AaOcV+2zzhS6OabhGQXVs=";

            try
            {
                // Send the request to the server and wait for the response:
                using (WebResponse response = await request.GetResponseAsync().ConfigureAwait(false))
                {

                    // Get a stream representation of the HTTP web response:
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        string text = reader.ReadToEnd();
                        //  string decrypted_Text = await OnCryptoAsync(text, "Decryption");
                        ResponseModel responseModel = JsonConvert.DeserializeObject<ResponseModel>(text);
                        if (responseModel.ResponseCode.Equals("Ok"))
                        {
                            string decrypted_Text = responseModel.ResponseValue;
                            reader.Close();
                            stream.Close();
                            return decrypted_Text;
                        }
                        else
                        {
                            return responseModel.ResponseCode;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return null;
                //Debug.WriteLine("Error:", e.Message);
            }

        }


        public async Task<string> GetOtp(Context context, string licenceid, string geoLocation, string version, string jsonData)
        {
            init(context);
            string url = "http://mobileapi.work121.com/api/AppLogin/GetMethod?associateID=8B280FFF-BFDD-4F62-8D46-08BA937DB981&associatepwd=mA121&licenceId=" + licenceid + "&licenceFor=TaskApp&AppVersion=" + version + "&geolocation=" + geoLocation + "&methodname=SendSMS&AppDateTime=" + AppDateTime + "&UserId=" + "5" + " &jsonData=" + jsonData;

            request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";
            request.Headers["userName"] = "uYfVkoP5BDouLkCBZ971sNZhzocdFLhmAvULyvsDnBo=";
            request.Headers["password"] = "/I/tmrWuA6AxGV6CiFgD/1AaOcV+2zzhS6OabhGQXVs=";


            try
            {
                using (WebResponse response = await request.GetResponseAsync().ConfigureAwait(false))
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        string text = reader.ReadToEnd();
                        //  string decrypted_Text = await OnCryptoAsync(text, "Decryption");
                        ResponseModel responseModel = JsonConvert.DeserializeObject<ResponseModel>(text);
                        if (responseModel.ResponseCode.Equals("Ok"))
                        {
                            string decrypted_Text = await OnCryptoAsync(responseModel.ResponseValue, "Decryption");
                            reader.Close();
                            stream.Close();
                            return decrypted_Text;
                        }
                        else
                        {
                            string decrypted_Text = await OnCryptoAsync(responseModel.ResponseValue, "Decryption");
                            reader.Close();
                            stream.Close();
                            return decrypted_Text;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }

        }

        
        

        



        public async Task<string> GetServiceMethod(Context context, string methodName, string jsonData)
        {

            prefs = PreferenceManager.GetDefaultSharedPreferences(context);
            init(context);

            if (jsonData.Equals(""))
            {
                jsonData = "w3ZUSfhMYXujjSK28/4kfw==";
            }

            string url = "http://mobileapi.work121.com/api/Comtax/GetMethod?associateID=8B280FFF-BFDD-4F62-8D46-08BA937DB981&associatepwd=mA121&licenceId=" +
                licenceId + "&UserId=" + UserId + "&methodname=" + methodName + "&jsonData=" + jsonData + "&AppDateTime=" + AppDateTime;

            request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";
            request.Headers["userName"] = "uYfVkoP5BDouLkCBZ971sNZhzocdFLhmAvULyvsDnBo=";
            request.Headers["password"] = "/I/tmrWuA6AxGV6CiFgD/1AaOcV+2zzhS6OabhGQXVs=";
            try
            {
                using (WebResponse response = await request.GetResponseAsync().ConfigureAwait(false))
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        string text = reader.ReadToEnd();
                        //  string decrypted_Text = await OnCryptoAsync(text, "Decryption");
                        ResponseModel responseModel = JsonConvert.DeserializeObject<ResponseModel>(text);
                        if (responseModel.ResponseCode.Equals("Ok"))
                        {
                            string decrypted_Text = await OnCryptoAsync(responseModel.ResponseValue, "Decryption");
                            reader.Close();
                            stream.Close();
                            return decrypted_Text;
                        }
                        else
                        {
                            return responseModel.ResponseCode;
                        }

                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }

        }
        

        public async Task<string> GetComplianceTask(Context context,  string _jsonData, string geolocation)
        {
            prefs = PreferenceManager.GetDefaultSharedPreferences(context);
            init(context);


            string url = "http://mobileapi.work121.com/api/Task/GetMethod?associateID=8B280FFF-BFDD-4F62-8D46-08BA937DB981&" +
             "associatepwd=mA121&licenceId=" + licenceId + "&UserId=" + UserId + "&methodname=GetTaskDetailByTaskId&jsonData=" + _jsonData + "&AppDateTime=" + AppDateTime + "&geoLocation=" + geolocation;

            request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";
            request.Headers["userName"] = "uYfVkoP5BDouLkCBZ971sNZhzocdFLhmAvULyvsDnBo=";
            request.Headers["password"] = "/I/tmrWuA6AxGV6CiFgD/1AaOcV+2zzhS6OabhGQXVs=";
            try
            {
                // Send the request to the server and wait for the response:
                using (WebResponse response = await request.GetResponseAsync().ConfigureAwait(false))
                {

                    // Get a stream representation of the HTTP web response:
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        string text = reader.ReadToEnd();
                        //  string decrypted_Text = await OnCryptoAsync(text, "Decryption");
                        ResponseModel responseModel = JsonConvert.DeserializeObject<ResponseModel>(text);
                        if (responseModel.ResponseCode.Equals("Ok"))
                        {
                            string decrypted_Text = responseModel.ResponseValue;
                            reader.Close();
                            stream.Close();
                            return decrypted_Text;
                        }
                        else
                        {
                            return responseModel.ResponseCode;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return null;
                //Debug.WriteLine("Error:", e.Message);
            }
        }

        public async Task<string> CompliancePostServiceMethod(Context context,string methodName, string jsonData, string comments)
        {

            //prefs = PreferenceManager.GetDefaultSharedPreferences(context);
            init(context);

            if (jsonData.Equals(""))
            {
                jsonData = "w3ZUSfhMYXujjSK28/4kfw==";
            }
         
            string url = "http://mobileapi.work121.com/api/Task/PostMethod?associateID=8B280FFF-BFDD-4F62-8D46-08BA937DB981&associatepwd=mA121" + "&licenceId=" + licenceId + "&UserId=" + UserId + "&methodname=" + methodName
                + "&jsonData=" + jsonData + "&AppDateTime=" + AppDateTime + "&GeoLocation=" + comments;
            request = (HttpWebRequest)WebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "POST";
            request.Headers["userName"] = "uYfVkoP5BDouLkCBZ971sNZhzocdFLhmAvULyvsDnBo=";
            request.Headers["password"] = "/I/tmrWuA6AxGV6CiFgD/1AaOcV+2zzhS6OabhGQXVs=";
            request.ContentLength = 0;
            try
            {
                using (WebResponse response = await request.GetResponseAsync().ConfigureAwait(false))
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        string text = reader.ReadToEnd();
                        //  string decrypted_Text = await OnCryptoAsync(text, "Decryption");
                        ResponseModel responseModel = JsonConvert.DeserializeObject<ResponseModel>(text);
                        if (responseModel.ResponseCode.Equals("Ok"))
                        {
                           // string decrypted_Text = await OnCryptoAsync(responseModel.ResponseValue, "Decryption");
                            reader.Close();
                            stream.Close();
                            return responseModel.ResponseValue;
                        }
                        else
                        {
                            return responseModel.ResponseCode;
                        }

                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }

        }
        public async Task<string> ComplianceTaskMarkCompleted(Context context, string methodName, string jsonData, string comments)
        {

            //prefs = PreferenceManager.GetDefaultSharedPreferences(context);
            init(context);

            if (jsonData.Equals(""))
            {
                jsonData = "w3ZUSfhMYXujjSK28/4kfw==";
            }

            string url = "http://mobileapi.work121.com/api/Task/PostMethod?associateID=8B280FFF-BFDD-4F62-8D46-08BA937DB981&associatepwd=mA121" + "&licenceId=" + licenceId + "&UserId=" + UserId + "&methodname=" + methodName
                + "&jsonData=" + jsonData + "&AppDateTime=" + AppDateTime + "&GeoLocation=" + comments;
            request = (HttpWebRequest)WebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "POST";
            request.Headers["userName"] = "uYfVkoP5BDouLkCBZ971sNZhzocdFLhmAvULyvsDnBo=";
            request.Headers["password"] = "/I/tmrWuA6AxGV6CiFgD/1AaOcV+2zzhS6OabhGQXVs=";
            request.ContentLength = 0;
            try
            {
                using (WebResponse response = await request.GetResponseAsync().ConfigureAwait(false))
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        string text = reader.ReadToEnd();
                        //  string decrypted_Text = await OnCryptoAsync(text, "Decryption");
                        ResponseModel responseModel = JsonConvert.DeserializeObject<ResponseModel>(text);
                        if (responseModel.ResponseCode.Equals("Ok"))
                        {
                            //string decrypted_Text = await OnCryptoAsync(responseModel.ResponseValue, "Decryption");
                            reader.Close();
                            stream.Close();
                            return responseModel.ResponseValue;
                        }
                        else
                        {
                            return responseModel.ResponseCode;
                        }

                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public async Task<string> ComplianceTask(Context context, string _licenceid, string _jsonData, string geolocation)
        {
            prefs = PreferenceManager.GetDefaultSharedPreferences(context);
            init(context);


            string url = "http://mobileapi.work121.com/api/Task/GetMethod?associateID=8B280FFF-BFDD-4F62-8D46-08BA937DB981&" +
             "associatepwd=mA121&licenceId=" + _licenceid + "&UserId=5" + UserId + "&methodname=GetTaskDetailByTaskId&jsonData=" + _jsonData + "&AppDateTime=" + AppDateTime + "&geoLocation=" + "26,80";

            request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";
            request.Headers["userName"] = "uYfVkoP5BDouLkCBZ971sNZhzocdFLhmAvULyvsDnBo=";
            request.Headers["password"] = "/I/tmrWuA6AxGV6CiFgD/1AaOcV+2zzhS6OabhGQXVs=";
            try
            {
                using (WebResponse response = await request.GetResponseAsync().ConfigureAwait(false))
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        string text = reader.ReadToEnd();
                        ResponseModel responseModel = JsonConvert.DeserializeObject<ResponseModel>(text);
                        if (responseModel.ResponseCode.Equals("Ok"))
                        {
                            string decrypted_Text = await OnCryptoAsync(responseModel.ResponseValue, "Decryption");

                            reader.Close();
                            stream.Close();
                            return decrypted_Text;

                        }
                        else
                        {
                            return responseModel.ResponseCode;
                        }

                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        //public async Task<string> PostServiceMethod(Context context, string methodName, string jsonData, string latLng, string datetime, string versionname, string username, string number)
        //{

        //    //prefs = PreferenceManager.GetDefaultSharedPreferences(context);
        //    init(context);

        //    if (jsonData.Equals(""))
        //    {
        //        jsonData = "w3ZUSfhMYXujjSK28/4kfw==";
        //    }

        //    string url = "https://abvplfunction.azurewebsites.net/api/TrackingApp?code=SDkarJzkiQJgs2NJa5PSLS2K0beVfKd7loTO2pLWMK4e0oZtzypYBA==&associateID=8B280FFF-BFDD-4F62-8D46-08BA937DB981&associatepwd=mA121&licenceId=" +
        //        licenceId + "&UserId=" + number + "&methodname=" + methodName + "&jsonData=" + jsonData + "&AppDateTime=" + datetime + "&GeoLocation=" + latLng + "&AppVersion=" + versionname + "&UserName=" + username;

        //    request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
        //    request.ContentType = "application/json";
        //    request.Method = "GET";
        //    request.Headers["userName"] = "uYfVkoP5BDouLkCBZ971sNZhzocdFLhmAvULyvsDnBo=";
        //    request.Headers["password"] = "/I/tmrWuA6AxGV6CiFgD/1AaOcV+2zzhS6OabhGQXVs=";
        //    try
        //    {
        //        using (WebResponse response = await request.GetResponseAsync().ConfigureAwait(false))
        //        {
        //            using (Stream stream = response.GetResponseStream())
        //            {
        //                StreamReader reader = new StreamReader(stream);
        //                string text = reader.ReadToEnd();
        //                return text;
        //                //  string decrypted_Text = await OnCryptoAsync(text, "Decryption");
        //                //ResponseModel responseModel = JsonConvert.DeserializeObject<ResponseModel>(text);
        //                //if (responseModel.ResponseCode.Equals("Ok"))
        //                //{
        //                //    //string decrypted_Text = await OnCryptoAsync(responseModel.ResponseValue, "Decryption");
        //                //    //reader.Close();
        //                //    //stream.Close();
        //                //    return responseModel.ResponseValue;
        //                //}
        //                //else
        //                //{
        //                //    return responseModel.ResponseValue;
        //                //}

        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return null;
        //    }

        //}


        public async Task<string> PostServiceLogin(Context context, string mobilenum)
        {
            init(context);
            string url = "http://mobileapi.work121.com/api/Login/SendSMS?associateID=8B280FFF-BFDD-4F62-8D46-08BA937DB981&associatepwd=mA121&licenceId=" + licenceId + "&UserId=5&MobileNo=" + mobilenum + "&SMS=OTP";

            request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";
            request.Headers["userName"] = "uYfVkoP5BDouLkCBZ971sNZhzocdFLhmAvULyvsDnBo=";
            request.Headers["password"] = "/I/tmrWuA6AxGV6CiFgD/1AaOcV+2zzhS6OabhGQXVs=";
            try
            {
                using (WebResponse response = await request.GetResponseAsync().ConfigureAwait(false))
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        string text = reader.ReadToEnd();
                        string otp = text;
                        return otp;
                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public async Task<string> OrgnizationList(Context context, string json, string geolocation)
        {
            init(context);
            //  "http://mobileapi.work121.com/api/Task/GetMethod?associateID=8B280FFF-BFDD-4F62-8D46-08BA937DB981&associatepwd=mA121&licenceId=" + licenceId + "&UserId=" + UserId + "&methodname=GetDesignationListByOrg&jsonData=" + json + "&AppDateTime=" + AppDateTime + "&geoLocation=" + geolocation;
            // api / Snayu / GetMethod ? associateID = 8B280FFF - BFDD - 4F62 - 8D46 - 08BA937DB981 & associatepwd = mA121 & licenceId = 12345 & UserId = 6 & methodname = GetOrganisationList & jsonData = &AppDateTime = 2019 - 01 - 21
            string url = "http://mobileapi.work121.com/api/Snayu/GetMethod?associateID=8B280FFF-BFDD-4F62-8D46-08BA937DB981&associatepwd=mA121&licenceId=" + licenceId + "&UserId=" + UserId + "&methodname=GetOrganisationList&jsonData=" + json + "&AppDateTime=" + AppDateTime + "&geoLocation=" + geolocation;
            request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";
            request.Headers["userName"] = "uYfVkoP5BDouLkCBZ971sNZhzocdFLhmAvULyvsDnBo=";
            request.Headers["password"] = "/I/tmrWuA6AxGV6CiFgD/1AaOcV+2zzhS6OabhGQXVs=";
            try
            {
                using (WebResponse response = await request.GetResponseAsync().ConfigureAwait(false))
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        string text = reader.ReadToEnd();
                        //  string decrypted_Text = await OnCryptoAsync(text, "Decryption");
                        ResponseModel responseModel = JsonConvert.DeserializeObject<ResponseModel>(text);
                        if (responseModel.ResponseCode.Equals("Ok"))
                        {
                            string decrypted_Text = await OnCryptoAsync(responseModel.ResponseValue, "Decryption");
                            reader.Close();
                            stream.Close();
                            return decrypted_Text;
                        }
                        else
                        {
                            return responseModel.ResponseCode;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }

        }


        //public async Task<JsonValue> GetTaskAssignByMe(string licenceid, string userId)
        //{
        //    JsonValue jsonDoc = null;
        //    string url = "http://mobileapi.work121.com/api/Task/GetTaskMarkByMe?associateID=8B280FFF-BFDD-4F62-8D46-08BA937DB981&associatepwd=mA121&licenceId=" + licenceid + "&userId=" + userId;

        //    request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
        //    request.ContentType = "application/json";
        //    request.Method = "GET";
        //    request.Headers["userName"] = "uYfVkoP5BDouLkCBZ971sNZhzocdFLhmAvULyvsDnBo=";
        //    request.Headers["password"] = "/I/tmrWuA6AxGV6CiFgD/1AaOcV+2zzhS6OabhGQXVs=";

        //    try
        //    {
        //        // Send the request to the server and wait for the response:
        //        using (WebResponse response = await request.GetResponseAsync().ConfigureAwait(false))
        //        {

        //            // Get a stream representation of the HTTP web response:
        //            using (Stream stream = response.GetResponseStream())
        //            {
        //                StreamReader reader = new StreamReader(stream);
        //                string text = reader.ReadToEnd();
        //                string decrypted_Text = await OnCryptoAsync2(text, "Decryption");
        //                //string decrypted_Text = await OnDecryptAsync(text);
        //                // Use this stream to build a JSON document object:
        //                jsonDoc = await Task.Run(() => JsonObject.Parse(decrypted_Text));
        //                //Debug.WriteLine("Response: {0}", jsonDoc.ToString());


        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        string decrypted_Text = await OnCryptoAsync2("JJqtv5Qv3tbsMRCFXZVjGg==", "Decryption");
        //    }
        //    return jsonDoc;

        //}

        //public async Task<JsonValue> TaskInbox(string licenceid, string userId, string pageNo, string pageSize)
        //{
        //    JsonValue jsonDoc = null;
        //    string url = "http://mobileapi.work121.com/api/Task/GetAssignTaskMe?associateID=8B280FFF-BFDD-4F62-8D46-08BA937DB981&associatepwd=mA121&licenceId=" + licenceid + "&userId=" + userId + "&PageNo=" + pageNo + "&PageSize=" + pageSize;

        //    request = (HttpWebRequest)HttpWebRequest.
        //        Create(new Uri(url));
        //    request.ContentType = "application/json";
        //    request.Method = "GET";
        //    request.Headers["userName"] = "uYfVkoP5BDouLkCBZ971sNZhzocdFLhmAvULyvsDnBo=";
        //    request.Headers["password"] = "/I/tmrWuA6AxGV6CiFgD/1AaOcV+2zzhS6OabhGQXVs=";

        //    try
        //    {
        //        // Send the request to the server and wait for the response:
        //        using (WebResponse response = await request.GetResponseAsync().ConfigureAwait(false))
        //        {

        //            // Get a stream representation of the HTTP web response:
        //            using (Stream stream = response.GetResponseStream())
        //            {
        //                StreamReader reader = new StreamReader(stream);
        //                string text = reader.ReadToEnd();
        //                string decrypted_Text = await OnCryptoAsync(text, "Decryption");
        //                //string decrypted_Text = await OnDecryptAsync(text);
        //                // Use this stream to build a JSON document object:
        //                jsonDoc = await Task.Run(() => JsonObject.Parse(decrypted_Text));
        //                //Debug.WriteLine("Response: {0}", jsonDoc.ToString());


        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        //Debug.WriteLine("Error:", e.Message);
        //    }
        //    return jsonDoc;
        //}

        //public async Task<string> OnDecryptAsync(string textValue)
        //{
        //    string text = textValue.Substring(1, textValue.Length - 2);
        //    string decryptedString = "";

        //    var decryption = DependencyService.Get<ICryptography>();
        //    if (decryption != null)
        //    {
        //        try
        //        {
        //            decryptedString = await decryption.FunctionAsync(text, "Decryption");
        //        }
        //        catch (Exception e)
        //        {
        //            Debug.WriteLine("error", e.Message);
        //        }
        //    }

        //    return decryptedString;
        //}

        public async Task<string> OnCryptoAsync(string textValue, string encDesc)
        {
            string text = "";
            if (encDesc.Equals("Encryption"))
            {
                text = textValue;
            }
            else
            {
                text = textValue.Substring(1, textValue.Length - 2);
            }
            string decryptedString = "";

            try
            {
                decryptedString = await cryptography.FunctionAsync(textValue, encDesc);
            }
            catch (Exception e)
            {
                // Debug.WriteLine("error", e.Message);
            }

            return decryptedString;
        }


        public async Task<string> OnCryptoAsync2(string textValue, string encDesc)
        {
            string text = "";
            if (encDesc.Equals("Encryption"))
            {
                text = textValue;
            }
            else
            {
                text = textValue.Substring(1, textValue.Length - 2);
            }
            string decryptedString = "";

            try
            {
                decryptedString = await cryptography.FunctionAsync(text, encDesc);
            }
            catch (Exception e)
            {
                // Debug.WriteLine("error", e.Message);
            }

            return decryptedString;
        }

    }
}


