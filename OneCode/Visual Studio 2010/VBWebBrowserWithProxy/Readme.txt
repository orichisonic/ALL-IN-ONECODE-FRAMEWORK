================================================================================
       Windows APPLICATION: VBWebBrowserWithProxy Overview                        
===============================================================================
/////////////////////////////////////////////////////////////////////////////
Summary:
The sample demonstrates how to make WebBrowser use a proxy server.

The WebBrowser control is a managed wrapper for the ActiveX WebBrowser control, 
and uses whichever version of the control is installed on the user's computer. This
means that WebBrowser is just an IE instance, so that it uses the proxy settings of 
IE. 

In Internet Explorer 5 and later, Internet options can be set for on a specific 
connection, for example, LAN connection or ADSL connection. Wininet.dll contains 2 
extern methods (InternetSetOption and InternetQueryOption) to set and retrieve 
internet settings.



////////////////////////////////////////////////////////////////////////////////
Demo:

Step1. Build this project in VS2010. 

Step2. Set the proxy servers in ProxyList.xml.
       The schema is like  
	   <ProxyList>
			<Proxy>
				<ProxyName>Proxy Name</ProxyName>
				<Address>Proxy url</Address>
				<UserName></UserName>
				<Password></Password>
			</Proxy> 
		</ProxyList> 
		If the proxy server needs credential, UserName and Password should be specified.

Step3. Run VBVBWebBrowserWithProxy.exe.

Step4. Type http://www.whatsmyip.us/ in the top text box, or any web page that could display
       your IP.

Step5. Check "No Proxy" and click Go. The browser shows your real IP.

Step6. Check "Proxy Server", choose a Proxy Server in the combo box and click Go. The browser 
       shows your IP through the Proxy.


/////////////////////////////////////////////////////////////////////////////
Code Logic:

1. Wrap 2 extern methods (InternetSetOption and InternetQueryOption) of wininet.dll, design
   the structure and initialize the constants used by them.

2. Use class WinINet to set proxy, disable proxy, backup internet options and restore internet
   options.
       
3. The class WebBrowserControl inherits WebBrowser class and has a feature to set proxy server.  
   The orginal internet options will be backup and the specified proxy will be used in 
   Navigating event, and the original internet options will be restored in Navigated event.

4. Initializes the proxy servers in ProxyList.xml when this application starts. 

5. Set the proxy of the web browser when the "Go" button was clicked and navigate to the URL.


/////////////////////////////////////////////////////////////////////////////
References:
http://msdn.microsoft.com/en-us/library/aa385114(VS.85).aspx
http://msdn.microsoft.com/en-us/library/aa385101(VS.85).aspx
http://msdn.microsoft.com/en-us/library/aa385384(VS.85).aspx
http://msdn.microsoft.com/en-us/library/aa385145(VS.85).aspx

/////////////////////////////////////////////////////////////////////////////
