========================================================================
    AZURESTORAGE : CSAzureStorageRESTAPI Solution Overview
========================================================================

/////////////////////////////////////////////////////////////////////////////
Use:

Sometimes we need to use raw REST API instead of the StorageClient class
provided by SDK. i.e. inserting an entity to table storage without schema, writing
a "StorageClient" library in other programming languages, etc.This sample shows
how to generate an HTTP message that uses the List Blobs API. You may reuse the
code to add authentication header to call other REST APIs.


/////////////////////////////////////////////////////////////////////////////
Prerequisites:

Windows Azure Tools for Microsoft Visual Studio 1.1 (February 2010)
http://www.microsoft.com/downloads/details.aspx?familyid=5664019E-6860-4C33-9843-4EB40B297AB6&displaylang=en


/////////////////////////////////////////////////////////////////////////////
Demo:

A. Make sure development storage is running.

B. Start debugging.

C. Input the name of a container in dev storage and press <ENTER>.

D. Observe the output that lists all blobs information in that container.



/////////////////////////////////////////////////////////////////////////////
References:

Differences Between Development Storage and Windows Azure Storage Services
http://msdn.microsoft.com/en-us/library/dd320275.aspx

List Blobs
http://msdn.microsoft.com/en-us/library/dd135734.aspx

Authentication Schemes
http://msdn.microsoft.com/en-us/library/dd179428.aspx


/////////////////////////////////////////////////////////////////////////////