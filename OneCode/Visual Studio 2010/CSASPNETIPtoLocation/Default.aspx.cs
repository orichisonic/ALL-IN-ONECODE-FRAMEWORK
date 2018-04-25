/**************************** Module Header ********************************\
* Module Name:    Default.aspx.cs
* Project:        CSASPNETIPtoLocation
* Copyright (c) Microsoft Corporation
*
* This project illustrates how to get the geographical location from an IP
* address via a free webservice http://freegeoip.appspot.com/. 
* 
* This source is subject to the Microsoft Public License.
* See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
* All other rights reserved.
\***************************************************************************/

using System;
using System.Net;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace CSASPNETIPtoLocation
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string ipAddress;

            // Get the client's IP address.
            ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = Request.ServerVariables["REMOTE_ADDR"];
            }

            lbIPAddress.Text = "You IP Address is: [" + ipAddress + "].";
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string ipAddress = tbIPAddress.Text;
            string locationJson;
            LocationInfo locationInfo = null;

            // New a WebClient instance.
            using (WebClient wc = new WebClient())
            {
                // Visit http://reegeoip.appspot.com to download the location json data
                locationJson = wc.DownloadString("http://freegeoip.appspot.com/json/" + ipAddress);
            }

            // Convert the data string to stream.
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

            using (MemoryStream jsonStream = new MemoryStream(encoding.GetBytes(locationJson)))
            {
                jsonStream.Position = 0;
                try
                {
                    // Deserialize the json data to get the LactionInfo object.
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(LocationInfo));
                    locationInfo = (LocationInfo)ser.ReadObject(jsonStream);
                }
                catch
                {
                    throw;
                }
            }

            if (locationInfo == null)
            {
                Response.Write("<strong>Cannot find the location based on the IP address [" + ipAddress + "].</strong> ");
            }
            else
            {
                if (locationInfo.status == true)
                {
                    // Output.
                    Response.Write("<strong>IP Address:</strong> ");
                    Response.Write(locationInfo.ip + "<br />");

                    Response.Write("<strong>Country Name:</strong> ");
                    Response.Write(locationInfo.countryname + "<br />");

                    Response.Write("<strong>City Name:</strong> ");
                    Response.Write(locationInfo.city + "<br />");
                }
            }

            lbIPAddress.Visible = false;
        }

        [Serializable]
        private class LocationInfo
        {
            public bool status = false;
            public string ip = null;
            public string countrycode = null;
            public string countryname = null;
            public string regioncode = null;
            public string regionname = null;
            public string city = null;
            public string zipcode = null;
            public string latitude = null;
            public string longitude = null;
        }
    }
}