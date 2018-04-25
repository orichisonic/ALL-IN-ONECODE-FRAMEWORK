=============================================================================
                  CSASPNETIPtoLocation Project Overview
=============================================================================

Use:

 This project illustrates how to get the geographical location from an IP
 address via a free webservice http://freegeoip.appspot.com/. 

/////////////////////////////////////////////////////////////////////////////
Demo the Sample.

Step1: Browse the Default.aspx from the sample project and you can find your 
IP address displayed on the page. If you are viewing the page at local, you 
may get the 127.0.0.1 as your client and the server is the same machine. When 
you deploy this demo to a host server, you will get your real IP address.

Step2: Enter an IP address in the TextBox and click the Submit button.

Step3: You will get the basic geographical location information, including
cooutry name and city name after the page posts back.

/////////////////////////////////////////////////////////////////////////////
Code Logical:

Step1: Create a C# ASP.NET Empty Web Application in Visual Studio 2010.

Step2: Add a Default ASP.NET page into the application.

Step3: Add a Label, a TextBox and a Button control to the page. The Label
is used to show the client IP address. TextBox is for IP address inputting,
and then user can click the Button to get the location info based on that
input.

Step4: Write code to get the client IP address.

    string ipAddress;
    ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
    if (string.IsNullOrEmpty(ipAddress))
    {
        ipAddress = Request.ServerVariables["REMOTE_ADDR"];
    }

Step5: Write code to get the location info from the free webservice 
http://freegeoip.appspot.com/. 

    using (WebClient wc = new WebClient())
    {
		string url = "http://freegeoip.appspot.com/json/" + ipAddress;
        locationJson = wc.DownloadString(url);
    }

    System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
    using (MemoryStream jsonStream = new MemoryStream(encoding.GetBytes(locationJson)))
    {
        jsonStream.Position = 0;
        try
        {
            DataContractJsonSerializer ser;
			ser = new DataContractJsonSerializer(typeof(LocationInfo));
            locationInfo = (LocationInfo)ser.ReadObject(jsonStream);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

NOTE: The returned Json data will like this:

	{
		"status":true,
		 "ip":"***.***.***.***",
		 "countrycode":"US",
		 "countryname":
		 "United States",
		 "regioncode":"",
		 "regionname":"",
		 "city":"",
		 "zipcode":"",
		 "latitude":38.0,
		 "longitude":-97.0
	 }

And we can also get the location info in XML format. Please refer to the link
http://freegeoip.appspot.com/xml/ipaddress to view the XML format data.

Step6: Write code to display the info on the page.

/////////////////////////////////////////////////////////////////////////////
References:

# free IP geolocation webservice
http://freegeoip.appspot.com/

# MSDN: How to: Serialize and Deserialize JSON Data
http://msdn.microsoft.com/en-us/library/bb412179.aspx

/////////////////////////////////////////////////////////////////////////////