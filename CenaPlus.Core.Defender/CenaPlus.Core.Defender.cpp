// CenaPlus.Core.Defender.cpp : 定义 DLL 应用程序的导出函数。
//

#include "stdafx.h"
#include "CenaPlus.Core.Defender.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// 唯一的应用程序对象

CWinApp theApp;

using namespace std;

void Hook(PSTR szModuleName, PSTR szFunName, FARPROC pFun)
{
	LPVOID pOldFunEntry, pNewFunEntry;    // 初始函数地址、HOOK后的函数地址
	BYTE bOldByte[5], bNewByte[5];        // 原始字节、目标字节
	HMODULE hMod = ::GetModuleHandleA(szModuleName);
	if (hMod != NULL)
	{
		pNewFunEntry = (LPVOID)pFun;
		pOldFunEntry = (LPVOID)GetProcAddress(hMod, szFunName);
		bNewByte[0] = 0xE9;
		*((PDWORD)(&(bNewByte[1]))) = (DWORD)pNewFunEntry - (DWORD)pOldFunEntry - 5;

		DWORD dwProtect, dwWriteByte, dwReadByte;
		VirtualProtect((LPVOID)pOldFunEntry, 5, PAGE_READWRITE, &dwProtect);
		ReadProcessMemory(GetCurrentProcess(), (LPVOID)pOldFunEntry, bOldByte, 5, &dwReadByte);
		WriteProcessMemory(GetCurrentProcess(), (LPVOID)pOldFunEntry, bNewByte, 5, &dwWriteByte);
		VirtualProtect((LPVOID)pOldFunEntry, 5, dwProtect, NULL);
	}
}

int _tmain(int argc, TCHAR* argv[], TCHAR* envp[])
{
	int nRetCode = 0;

	HMODULE hModule = ::GetModuleHandle(NULL);

	if (hModule != NULL)
	{
		if (!AfxWinInit(hModule, NULL, ::GetCommandLine(), 0))
		{
			nRetCode = 1;
		}
		else
		{
			Hook("User32.dll", "ExitWindowsEx", NULL);
			Hook("User32.dll", "UserAttachThreadInput", NULL);
			Hook("Advapi32.dll", "AdjustTokenPrivileges", NULL);
			Hook("Advapi32.dll", "OpenProcessToken", NULL);

			// Kernel

			//Hook("Kernel32.dll", "ZeroMemory", NULL);
			Hook("Kernel32.dll", "CreateProcessA", NULL);
			Hook("Kernel32.dll", "CreateProcessW", NULL);
			Hook("Kernel32.dll", "CreateNamedPipeA", NULL);
			Hook("Kernel32.dll", "CreateNamedPipeW", NULL);
			Hook("Kernel32.dll", "CreatePipeA", NULL);
			Hook("Kernel32.dll", "CreatePipeW", NULL);
			//Hook("Kernel32.dll", "CreateEventA", NULL);
			//Hook("Kernel32.dll", "CreateEventW", NULL);
			//Hook("Kernel32.dll", "CreateSemaphoreA", NULL);
			//Hook("Kernel32.dll", "CreateSemaphoreW", NULL);
			//Hook("Kernel32.dll", "CreateMailslotA", NULL);
			//Hook("Kernel32.dll", "CreateMailslotW", NULL);
			Hook("Kernel32.dll", "OpenProcess", NULL);
			Hook("Kernel32.dll", "ConnectNamedPipe", NULL);
			//Hook("Kernel32.dll", "GetCurrentThread", NULL);
			//Hook("Kernel32.dll", "GetCurrentProcessId", NULL);
			//Hook("Kernel32.dll", "GetProcAddress", NULL);
			//Hook("Kernel32.dll", "GetCurrentThreadId", NULL);
			//Hook("Kernel32.dll", "GetProcMemoryInfo", NULL);
			//Hook("Kernel32.dll", "GetThreadPriority", NULL);
			//Hook("Kernel32.dll", "ExitProcess", NULL);
			//Hook("Kernel32.dll", "ExitThread", NULL);
			//Hook("Kernel32.dll", "SetPriorityClass", NULL);
			//Hook("Kernel32.dll", "SetThreadPriority", NULL);
			//Hook("Kernel32.dll", "SwitchToThread", NULL);
			Hook("Kernel32.dll", "LoadLibraryExA", NULL);
			Hook("Kernel32.dll", "LoadLibraryExW", NULL);
			Hook("Kernel32.dll", "MessageBoxA", NULL);
			Hook("Kernel32.dll", "MessageBoxW", NULL);

			Hook("Kernel32.dll", "CreateFileA", NULL);
			Hook("Kernel32.dll", "CreateFileW", NULL);
			Hook("Kernel32.dll", "CopyFileA", NULL);
			Hook("Kernel32.dll", "CopyFileW", NULL);
			Hook("Kernel32.dll", "CopyFileExA", NULL);
			Hook("Kernel32.dll", "CopyFileExW", NULL);
			Hook("Kernel32.dll", "DeleteFileA", NULL);
			Hook("Kernel32.dll", "DeleteFileW", NULL);
			Hook("Kernel32.dll", "WriteFileA", NULL);
			Hook("Kernel32.dll", "WriteFileW", NULL);

			//Hook("Kernel32.dll", "CreateFiberEx", NULL);
			//Hook("Kernel32.dll", "CreateRemoteThread", NULL);
			//Hook("Kernel32.dll", "CreateThread", NULL);
			//Hook("Kernel32.dll", "HeapCreate", NULL);
			//Hook("Kernel32.dll", "HeapFree", NULL);
			//Hook("Kernel32.dll", "HeapLock", NULL);
			Hook("Kernel32.dll", "LoadLibraryA", NULL);
			Hook("Kernel32.dll", "LoadLibraryW", NULL);
			//Hook("Kernel32.dll", "LoadModule", NULL);
			//Hook("Kernel32.dll", "WriteConsoleA", NULL);
			//Hook("Kernel32.dll", "WriteConsoleW", NULL);
			//Hook("Kernel32.dll", "Sleep", NULL);
			//Hook("Kernel32.dll", "SleepEx", NULL);

			//Psapi
			Hook("Psapi.dll", "EnumProcesses", NULL);
			Hook("Psapi.dll", "EnumPageFiles", NULL);
			Hook("Psapi.dll", "EnumDeviceDrivers", NULL);
			Hook("Psapi.dll", "EmptyWorkingSet", NULL);
			Hook("Psapi.dll", "EnumProcessModules", NULL);
			Hook("Psapi.dll", "InitializeProcessForWsWatch", NULL);
			Hook("Psapi.dll", "QueryWorkingSet", NULL);

			//Ws2_32
			Hook("Ws2_32.dll", "WSAStartup", NULL);
			Hook("Ws2_32.dll", "socket", NULL);
			Hook("Ws2_32.dll", "WSACleanup", NULL);
			Hook("Ws2_32.dll", "connect", NULL);
			Hook("Ws2_32.dll", "send", NULL);
			Hook("Ws2_32.dll", "recv", NULL);
			Hook("Ws2_32.dll", "bind", NULL);
			Hook("Ws2_32.dll", "listen", NULL);
			Hook("Ws2_32.dll", "accept", NULL);
			Hook("Ws2_32.dll", "WSARecv", NULL);
			Hook("Ws2_32.dll", "WSARecvEx", NULL);
			Hook("Ws2_32.dll", "recvfrom", NULL);
			Hook("Ws2_32.dll", "sendto", NULL);
			Hook("Ws2_32.dll", "WSAAccept", NULL);
			Hook("Ws2_32.dll", "WSAConnect", NULL);
			Hook("Ws2_32.dll", "WSACreateEvent", NULL);
			Hook("Ws2_32.dll", "WSARecvFrom", NULL);
			Hook("Ws2_32.dll", "WSASend", NULL);
			Hook("Ws2_32.dll", "WSASendTo", NULL);
			Hook("Ws2_32.dll", "WSASetServiceA", NULL);
			Hook("Ws2_32.dll", "WSASetServiceW", NULL);
			Hook("Ws2_32.dll", "WSASocketA", NULL);
			Hook("Ws2_32.dll", "WSASocketW", NULL);

			//Internet
			Hook("wininet.dll", "InternetOpenA", NULL);
			Hook("wininet.dll", "InternetOpenW", NULL);
			Hook("wininet.dll", "InternetSetCookieA", NULL);
			Hook("wininet.dll", "InternetSetCookieW", NULL);
			Hook("wininet.dll", "InternetConnectA", NULL);
			Hook("wininet.dll", "InternetConnectW", NULL);
			Hook("wininet.dll", "HttpSendRequestA", NULL);
			Hook("wininet.dll", "HttpSendRequestW", NULL);
			Hook("wininet.dll", "SQLConnect", NULL);
			Hook("wininet.dll", "InternetReadFile", NULL);

			//Last
			Hook("Kernel32.dll", "WriteProcessMemory", NULL);
		}
	}
	else
	{
		nRetCode = 1;
	}

	return nRetCode;
}
