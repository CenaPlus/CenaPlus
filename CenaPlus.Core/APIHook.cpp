#include "stdafx.h"

DWORD EnablePrivilege()
{
	HANDLE hToken;
	BOOL rv;
	TOKEN_PRIVILEGES priv = { 1, { 0, 0, SE_PRIVILEGE_ENABLED } };
	OpenProcessToken(
		GetCurrentProcess(),
		TOKEN_ADJUST_PRIVILEGES,
		&hToken
		);
	AdjustTokenPrivileges(
		hToken,
		FALSE,
		&priv,
		sizeof priv,
		0,
		0
		);
	rv = GetLastError();
	CloseHandle(hToken);
	return rv;
}

BOOL GetProcessIdByName(LPWSTR szProcessName, LPDWORD lpPID)
{
	STARTUPINFO st;
	PROCESS_INFORMATION pi;
	PROCESSENTRY32 ps;
	HANDLE hSnapshot;
	ZeroMemory(&st, sizeof(STARTUPINFO));
	ZeroMemory(&pi, sizeof(PROCESS_INFORMATION));
	st.cb = sizeof(STARTUPINFO);
	ZeroMemory(&ps, sizeof(PROCESSENTRY32));
	ps.dwSize = sizeof(PROCESSENTRY32);
	hSnapshot = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);
	if (hSnapshot == INVALID_HANDLE_VALUE)
	{
		return FALSE;
	}

	if (!Process32First(hSnapshot, &ps))
	{
		return FALSE;
	}
	do
	{
		if (lstrcmpi(ps.szExeFile, szProcessName) == 0)
		{
			*lpPID = ps.th32ProcessID;
			CloseHandle(hSnapshot);
			return TRUE;
		}
	} while (Process32Next(hSnapshot, &ps));
	CloseHandle(hSnapshot);
	return FALSE;
}

BOOL LoadRemoteDLL(DWORD dwProcessId, LPWSTR lpszLibName)
{
	if (!EnablePrivilege())
		return FALSE;
	BOOL bResult = FALSE;
	HANDLE hProcess = NULL;
	HANDLE hThread = NULL;
	PSTR pszLibFileRemote = NULL;
	DWORD cch;
	LPTHREAD_START_ROUTINE pfnThreadRtn;
	hProcess = OpenProcess(
		PROCESS_ALL_ACCESS,
		FALSE,
		dwProcessId
		);
	if (hProcess == NULL)
		return FALSE;
	cch = 1 + lstrlen(lpszLibName);
	pszLibFileRemote = (PSTR)VirtualAllocEx
		(
		hProcess,
		NULL,
		cch,
		MEM_COMMIT,
		PAGE_READWRITE
		);
	if (pszLibFileRemote == NULL)
		return FALSE;
	if (!WriteProcessMemory
		(
		hProcess,
		(PVOID)pszLibFileRemote,
		(PVOID)lpszLibName,
		cch,
		NULL
		))
		return FALSE;
	pfnThreadRtn = (LPTHREAD_START_ROUTINE)GetProcAddress(
		GetModuleHandle(_T("Kernel32")),
		"LoadLibraryW"
		);
	if (pfnThreadRtn == NULL)
		return FALSE;
	hThread = CreateRemoteThread(
		hProcess,
		NULL,
		0,
		pfnThreadRtn,
		(PVOID)pszLibFileRemote,
		0,
		NULL
		);
	if (hThread == NULL)
		return FALSE;
	WaitForSingleObject(hThread, INFINITE);
	bResult = TRUE;
	if (pszLibFileRemote != NULL)
		bResult = VirtualFreeEx(hProcess, (PVOID)pszLibFileRemote, 0, MEM_RELEASE);
	if (bResult == FALSE)
		return bResult;
	if (hThread != NULL)
		bResult = CloseHandle(hThread);
	if (bResult == FALSE)
		return bResult;
	if (hProcess != NULL)
		bResult = CloseHandle(hProcess);
	if (bResult == FALSE)
		return bResult;
	return bResult;
}
