/****************************** Module Header ******************************\
* Module Name:  ServiceTriggerStart.cpp
* Project:      CppWin7TriggerStartService
* Copyright (c) Microsoft Corporation.
* 
* The file implements functions to set and get the configuration of service 
* trigger start.
* 
* * SupportServiceTriggerStart:
*   Check whether the current system supports service trigger start. Service 
*   trigger events are not supported until Windows Server 2008 R2 and Windows 
*   7. Wndows Server 2008 R2 and Windows 7 have major version 6 and minor 
*   version 1.
*   
* * GetServiceTriggerInfo
*   Get the trigger-start configuration of a service.
* 
* * SetServiceTriggerStartOnUSBArrival
*   Set the service to trigger-start when a generic USB disk is available.
* 
* * SetServiceTriggerStartOnIPAddressArrival
*   Set the service to trigger-start when the first IP address on the TCP/IP 
*   networking stack becomes available, and trigger-stop when the last IP 
*   address on the TCP/IP networking stack becomes unavailable.
* 
* Services and background processes have tremendous influence on the overall 
* performance of the system. If we could just cut down on the total number of 
* services, we would reduce the total power consumption and increase the 
* overall stability of the system. The Windows 7 Service Control Manager has 
* been extended so that a service can be automatically started and stopped 
* when a specific system event, or trigger, occurs on the system. The new 
* mechanism is called Service Trigger Event. A service can register to be 
* started or stopped when a trigger event occurs. This eliminates the need 
* for services to start when the system starts, or for services to poll or 
* actively wait for an event; a service can start when it is needed, instead 
* of starting automatically whether or not there is work to do. Examples of 
* predefined trigger events include arrival of a device of a specified device 
* interface class or availability of a particular firewall port. A service 
* can also register for a custom trigger event generated by an Event Tracing 
* for Windows (ETW) provider.
* 
* This source is subject to the Microsoft Public License.
* See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
* All other rights reserved.
* 
* THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
* EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
* WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/

#pragma region "Includes"
#include <windows.h>
#include "ServiceTriggerStart.h"
#pragma endregion


//
//  FUNCTION: SupportServiceTriggerStart
//
//  PURPOSE: Check whether the current system supports service trigger start
//
//  PARAMETERS:
//    none
//
//  RETURN VALUE:
//    TRUE if the current system supports service trigger start
//    FALSE if the current system does not support service trigger start
//
//  COMMENTS:
//    Service trigger events are not supported until Windows Server 2008 
//    R2 and Windows 7. Wndows Server 2008 R2 and Windows 7 have major 
//    version 6 and minor version 1.
//
BOOL SupportServiceTriggerStart()
{
	// osVersionInfoToCompare contains the OS version requirements to compare
	OSVERSIONINFOEX osVersionInfoToCompare = { sizeof(osVersionInfoToCompare) };
	osVersionInfoToCompare.dwMajorVersion = 6;
	osVersionInfoToCompare.dwMinorVersion = 1;		// Windows 7
	osVersionInfoToCompare.wServicePackMajor = 0;
	osVersionInfoToCompare.wServicePackMinor = 0;

	// Initialize the condition mask with ULONGLONG VER_SET_CONDITION(
	// ULONGLONG dwlConditionMask, DWORD dwTypeBitMask, BYTE dwConditionMask)
	ULONGLONG comparisonInfo = 0;
	BYTE conditionMask = VER_GREATER_EQUAL;
	VER_SET_CONDITION(comparisonInfo, VER_MAJORVERSION, conditionMask);
	VER_SET_CONDITION(comparisonInfo, VER_MINORVERSION, conditionMask);
	VER_SET_CONDITION(comparisonInfo, VER_SERVICEPACKMAJOR, conditionMask);
	VER_SET_CONDITION(comparisonInfo, VER_SERVICEPACKMINOR, conditionMask);

	// Compares a set of operating system version requirements to the 
	// corresponding values for the currently running version of the system.
	return VerifyVersionInfo(&osVersionInfoToCompare, VER_MAJORVERSION | 
		VER_MINORVERSION | VER_SERVICEPACKMAJOR | VER_SERVICEPACKMINOR,
		comparisonInfo);
}


//
//  FUNCTION: GetServiceTriggerInfo
//
//  PURPOSE: Get the trigger-start configuration of a service
//
//  PARAMETERS:
//    hService - A handle to the service. This handle is returned by the 
//      OpenService or CreateService function and must have the 
//      SERVICE_QUERY_CONFIG access right. 
//    lpfIsTriggerStart - Output a pointer to a variable that indicates 
//      whether the service is configured to trigger start
//
//  RETURN VALUE:
//    TRUE if the function succeeds
//    FALSE if the function fails. To get extended error information, call 
//      GetLastError.
//
//  COMMENTS:
//    none
//
BOOL GetServiceTriggerInfo(SC_HANDLE hService, LPBOOL lpfIsTriggerStart)
{
	// Get the service trigger info size of the current serivce
	DWORD cbBytesNeeded = -1;
	QueryServiceConfig2(hService, SERVICE_CONFIG_TRIGGER_INFO, NULL, 0, 
		&cbBytesNeeded);
	if (cbBytesNeeded == -1)
	{
		return FALSE;
	}

	// Allocate memory for the service trigger info struct
	PBYTE lpBuffer = (PBYTE)HeapAlloc(GetProcessHeap(), HEAP_ZERO_MEMORY, 
		cbBytesNeeded);
	if (lpBuffer == NULL)
	{
		return FALSE;
	}

	// Retrieve the service trigger info
	DWORD dwErr = ERROR_SUCCESS;
	if (QueryServiceConfig2(hService, SERVICE_CONFIG_TRIGGER_INFO, lpBuffer, 
		cbBytesNeeded, &cbBytesNeeded))
	{
		PSERVICE_TRIGGER_INFO psti = (PSERVICE_TRIGGER_INFO)lpBuffer;

		*lpfIsTriggerStart = (psti->cTriggers > 0);
			
		// You can retrieve more trigger information of the service
		/*for (int i = 0; i < pTriggerInfo->cTriggers; i++)
		{
			// You can access each SERVICE_TRIGGER struct through 
			// pTriggerInfo->pTriggers[i];
		}*/
	}
	else
	{
		dwErr = GetLastError();
	}

	// Free the memory of the service trigger info struct
	HeapFree(GetProcessHeap(), 0, lpBuffer);

	if (dwErr == ERROR_SUCCESS)
	{
		return TRUE;
	}
	else
	{
		SetLastError(dwErr);
		return FALSE;
	}
}


//
//  FUNCTION: SetServiceTriggerStartOnUSBArrival
//
//  PURPOSE: Set the service to trigger-start when a generic USB disk becomes 
//    available. 
//
//  PARAMETERS:
//    hService - A handle to the service. This handle is returned by the 
//      OpenService or CreateService function and must have the 
//      SERVICE_CHANGE_CONFIG access right.
//
//  RETURN VALUE:
//    TRUE if the function succeeds
//    FALSE if the function fails. To get extended error information, call 
//      GetLastError.
//
//  COMMENTS:
//    none
//
BOOL SetServiceTriggerStartOnUSBArrival(SC_HANDLE hService)
{
	// Hardware ID generated by the USB storage port driver
	wchar_t szDeviceData[] = L"USBSTOR\\GenDisk";

	// Allocate and set the SERVICE_TRIGGER_SPECIFIC_DATA_ITEM structure
	SERVICE_TRIGGER_SPECIFIC_DATA_ITEM deviceData = {0};
	deviceData.dwDataType = SERVICE_TRIGGER_DATA_TYPE_STRING;
	deviceData.cbData = sizeof(szDeviceData);
	deviceData.pData = (PBYTE)szDeviceData;

	// Allocate and set the SERVICE_TRIGGER structure
	SERVICE_TRIGGER serviceTrigger = {0};
	serviceTrigger.dwTriggerType = SERVICE_TRIGGER_TYPE_DEVICE_INTERFACE_ARRIVAL;
	serviceTrigger.dwAction = SERVICE_TRIGGER_ACTION_SERVICE_START;
	serviceTrigger.pTriggerSubtype = const_cast<GUID*>(&GUID_DEVINTERFACE_DISK);
	serviceTrigger.cDataItems = 1;
	serviceTrigger.pDataItems = &deviceData;

	// Allocate and set the SERVICE_TRIGGER_INFO structure
	SERVICE_TRIGGER_INFO serviceTriggerInfo = {0};
	serviceTriggerInfo.cTriggers = 1;
	serviceTriggerInfo.pTriggers = &serviceTrigger;

	// Call ChangeServiceConfig2 with the SERVICE_CONFIG_TRIGGER_INFO level 
	// and pass to it the address of the SERVICE_TRIGGER_INFO structure
	return ChangeServiceConfig2(hService, SERVICE_CONFIG_TRIGGER_INFO, &serviceTriggerInfo);
}


//
//  FUNCTION: SetServiceTriggerStartOnIPAddressArrival
//
//  PURPOSE: Set the service to trigger-start when the first IP address on 
//    the TCP/IP networking stack becomes available, and trigger-stop when 
//    the last IP address on the TCP/IP networking stack becomes unavailable.
//
//  PARAMETERS:
//    hService - A handle to the service. This handle is returned by the 
//      OpenService or CreateService function and must have the 
//      SERVICE_CHANGE_CONFIG access right.
//
//  RETURN VALUE:
//    TRUE if the function succeeds
//    FALSE if the function fails. To get extended error information, call 
//      GetLastError.
//
//  COMMENTS:
//    none
//
BOOL SetServiceTriggerStartOnIPAddressArrival(SC_HANDLE hService)
{
	SERVICE_TRIGGER serviceTriggers[2] = {0};

	// Allocate and set the SERVICE_TRIGGER structure for 
	// NETWORK_MANAGER_FIRST_IP_ADDRESS_ARRIVAL_GUID to start the service
	serviceTriggers[0].dwTriggerType = SERVICE_TRIGGER_TYPE_IP_ADDRESS_AVAILABILITY;
	serviceTriggers[0].dwAction = SERVICE_TRIGGER_ACTION_SERVICE_START;
	serviceTriggers[0].pTriggerSubtype = const_cast<GUID*>(&NETWORK_MANAGER_FIRST_IP_ADDRESS_ARRIVAL_GUID);

	// Allocate and set the SERVICE_TRIGGER structure for 
	// NETWORK_MANAGER_LAST_IP_ADDRESS_REMOVAL_GUID to stop the service
	serviceTriggers[1].dwTriggerType = SERVICE_TRIGGER_TYPE_IP_ADDRESS_AVAILABILITY;
	serviceTriggers[1].dwAction = SERVICE_TRIGGER_ACTION_SERVICE_STOP;
	serviceTriggers[1].pTriggerSubtype = const_cast<GUID*>(&NETWORK_MANAGER_LAST_IP_ADDRESS_REMOVAL_GUID);

	// Allocate and set the SERVICE_TRIGGER_INFO structure
	SERVICE_TRIGGER_INFO serviceTriggerInfo = {0};
	serviceTriggerInfo.cTriggers = 2;
	serviceTriggerInfo.pTriggers = (PSERVICE_TRIGGER)&serviceTriggers;

	// Call ChangeServiceConfig2 with the SERVICE_CONFIG_TRIGGER_INFO level 
	// and pass to it the address of the SERVICE_TRIGGER_INFO structure
	return ChangeServiceConfig2(hService, SERVICE_CONFIG_TRIGGER_INFO, &serviceTriggerInfo);
}


//
//  FUNCTION: SetServiceTriggerStartOnFirewallPortEvent
//
//  PURPOSE: Set the service to trigger-start when a firewall port (UDP 5001 
//    in this example) is opened, and trigger-stop when the a firewall port 
//    (UDP 5001 in this example) is closed. 
//
//  PARAMETERS:
//    hService - A handle to the service. This handle is returned by the 
//      OpenService or CreateService function and must have the 
//      SERVICE_CHANGE_CONFIG access right.
//
//  RETURN VALUE:
//    TRUE if the function succeeds
//    FALSE if the function fails. To get extended error information, call 
//      GetLastError.
//
//  COMMENTS:
//    none
//
BOOL SetServiceTriggerStartOnFirewallPortEvent(SC_HANDLE hService)
{
	SERVICE_TRIGGER serviceTriggers[2] = {0};

	// Specify the port(5001), the protocol(UDP), and optionally the 
	// executable path and name of the service listening on the event.
    wchar_t szPortData[] = L"5001\0UDP\0";

	// Allocate and set the SERVICE_TRIGGER_SPECIFIC_DATA_ITEM structure
	SERVICE_TRIGGER_SPECIFIC_DATA_ITEM portData = {0};
	portData.dwDataType = SERVICE_TRIGGER_DATA_TYPE_STRING;
	portData.cbData = sizeof(szPortData);
	portData.pData = (PBYTE)szPortData;

	// Allocate and set the SERVICE_TRIGGER structure for 
	// FIREWALL_PORT_OPEN_GUID to start the service
	serviceTriggers[0].dwTriggerType = SERVICE_TRIGGER_TYPE_FIREWALL_PORT_EVENT;
	serviceTriggers[0].dwAction = SERVICE_TRIGGER_ACTION_SERVICE_START;
	serviceTriggers[0].pTriggerSubtype = const_cast<GUID*>(&FIREWALL_PORT_OPEN_GUID);
	serviceTriggers[0].cDataItems = 1;
	serviceTriggers[0].pDataItems = &portData;

	// Allocate and set the SERVICE_TRIGGER structure for 
	// FIREWALL_PORT_CLOSE_GUID to stop the service
	serviceTriggers[1].dwTriggerType = SERVICE_TRIGGER_TYPE_FIREWALL_PORT_EVENT;
	serviceTriggers[1].dwAction = SERVICE_TRIGGER_ACTION_SERVICE_STOP;
	serviceTriggers[1].pTriggerSubtype = const_cast<GUID*>(&FIREWALL_PORT_CLOSE_GUID);
	serviceTriggers[1].cDataItems = 1;
	serviceTriggers[1].pDataItems = &portData;

	// Allocate and set the SERVICE_TRIGGER_INFO structure
	SERVICE_TRIGGER_INFO serviceTriggerInfo = {0};
	serviceTriggerInfo.cTriggers = 2;
	serviceTriggerInfo.pTriggers = (PSERVICE_TRIGGER)&serviceTriggers;

	// Call ChangeServiceConfig2 with the SERVICE_CONFIG_TRIGGER_INFO level 
	// and pass to it the address of the SERVICE_TRIGGER_INFO structure
	return ChangeServiceConfig2(hService, SERVICE_CONFIG_TRIGGER_INFO, &serviceTriggerInfo);
}