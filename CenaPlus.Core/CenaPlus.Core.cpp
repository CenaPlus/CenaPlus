// CenaPlus.Core.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"
#include "CenaPlus.Core.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

CWinApp theApp;

using namespace std;

typedef struct _THREAD_PARAM
{
	HANDLE ProcessHandle;
	int TimeLimit;
} THREAD_PARAM, *LPTHREAD_PARAM;

int TimeLimit;
int MemoryLimit;
int HighPriorityTime;
DWORD ExitCode = 0;
int TimeUsed = -1;
int PagedUsed = -1;
int WorkingSetUsed = -1;
/*
	argv[1]: filename and arguments.
	argv[2]: input file path.
	argv[3]: output file path.
	argv[4]: errput file path.
	argv[5]: time limit(ms).
	argv[6]: memory limit(KB).
	argv[7]: high priority run time(ms).
	argv[8]: APIHook dll path.
	argv[9]: XML create path.
*/
int _tmain(int argc, TCHAR* argv[], TCHAR* envp[])
{
	int nRetCode = 0;
	const int nSuccess = 0;
	const int nRuntimeError = 1;
	const int nTimeLimitExceeded = 2;
	const int nMemoryLimitExceeded = 3;
	const int nSystemError = 4;
	const int nArgcError = 5;

	HMODULE hModule = ::GetModuleHandle(NULL);
	HANDLE TimeLimitValidator;
	DWORD TimeLimitValidatorThreadID;

	DWORD WINAPI TimeLimitValidatorThreadProc(LPVOID lpParam);

	if (hModule != NULL)
	{
		if (!AfxWinInit(hModule, NULL, ::GetCommandLine(), 0))
		{
			_tprintf(_T("ERROR:  MFC init failed\n"));
			nRetCode = nSystemError;
		}
		else
		{
			if (argc != 9)
			{
				_tprintf(_T("ERROR:  GetModuleHandle failed\n"));
				nRetCode = nSystemError;
			}
			else
			{
				TimeLimit = _ttoi(CString(argv[5]));
				MemoryLimit = _ttoi(CString(argv[6]));
				HighPriorityTime = _ttoi(CString(argv[7]));
				SECURITY_ATTRIBUTES saAttr;
				HANDLE ChildIn_Read, ChildIn_Write, ChildOut_Read, ChildOut_Write;
				HANDLE TimeLimitValidator;
				STARTUPINFOA StartupInfo;
				PROCESS_INFORMATION ProcessInfo;
				PROCESS_MEMORY_COUNTERS ProcessMemoryCounters;

				ZeroMemory(&saAttr, sizeof(saAttr));
				saAttr.nLength = sizeof(SECURITY_ATTRIBUTES);
				saAttr.bInheritHandle = TRUE;
				saAttr.lpSecurityDescriptor = NULL;
				ZeroMemory(&StartupInfo, sizeof(StartupInfo));
				StartupInfo.cb = sizeof(STARTUPINFO);
				ZeroMemory(&ProcessInfo, sizeof(ProcessInfo));
				if (CString(argv[2]) != CString(L"NULL"))
				{
					HANDLE InputFile = CreateFile(argv[2], GENERIC_READ, FILE_SHARE_READ | FILE_SHARE_WRITE, &saAttr, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL);
					StartupInfo.hStdInput = InputFile;
				}
				if (CString(argv[3]) != CString(L"NULL"))
				{
					HANDLE OutputFile = CreateFile(argv[3], GENERIC_WRITE, FILE_SHARE_READ | FILE_SHARE_WRITE, &saAttr, CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL, NULL);
					StartupInfo.hStdOutput = OutputFile;
				}
				if (CString(argv[4]) != CString(L"NULL"))
				{
					HANDLE ErrputFile = CreateFile(argv[4], GENERIC_WRITE, FILE_SHARE_READ | FILE_SHARE_WRITE, &saAttr, CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL, NULL);
					StartupInfo.hStdError = ErrputFile;
				}
				StartupInfo.dwFlags |= STARTF_USESTDHANDLES;
				CreateProcess(NULL, argv[1], NULL, NULL, TRUE, CREATE_SUSPENDED, NULL, NULL, (LPSTARTUPINFOW)(&StartupInfo), &ProcessInfo);
				if (CString(argv[8]) != CString(L"NULL"))
					LoadRemoteDLL(ProcessInfo.dwProcessId, argv[8]);
				LPTHREAD_PARAM pData;
				pData = (LPTHREAD_PARAM)HeapAlloc(GetProcessHeap(), HEAP_ZERO_MEMORY, sizeof(THREAD_PARAM));
				if (pData != NULL)
				{
					pData->ProcessHandle = ProcessInfo.hProcess;
					pData->TimeLimit = TimeLimit;
					TimeLimitValidator = CreateThread(
						NULL,
						0,
						TimeLimitValidatorThreadProc,
						pData,
						0,
						&TimeLimitValidatorThreadID
						);
				}
				ResumeThread(ProcessInfo.hThread);
				WaitForSingleObject(ProcessInfo.hProcess, INFINITE);
				GetProcessMemoryInfo(ProcessInfo.hProcess, &ProcessMemoryCounters, sizeof(ProcessMemoryCounters));
				PagedUsed = ProcessMemoryCounters.PeakPagefileUsage / 1024;
				WorkingSetUsed = ProcessMemoryCounters.PeakWorkingSetSize / 1024;
				DWORD ThreadExitCode;
				GetExitCodeThread(TimeLimitValidator, &ThreadExitCode);
				if (ThreadExitCode == 2)
					TimeUsed = TimeLimit + 1;
				CloseHandle(TimeLimitValidator);
				GetExitCodeProcess(pData->ProcessHandle, &ExitCode);
				CString Convert;
				CString Result = L"<?xml version=\"1.0\" ?>";
				Result += L"<Result>";
				Convert = L"";
				Convert.Format(L"%d", ExitCode);
				Result += L"    <ExitCode>" + Convert + L"</ExitCode>";
				Convert = L"";
				Convert.Format(L"%d", TimeUsed);
				Result += L"    <TimeUsed>" + Convert + L"</TimeUsed>";
				Convert = L"";
				Convert.Format(L"%d", PagedUsed);
				Result += L"    <PagedSize>" + Convert + L"</PagedSize>";
				Convert = L"";
				Convert.Format(L"%d", WorkingSetUsed);
				Result += L"    <WorkingSet>" + Convert + L"</WorkingSet>";
				Result += L"</Result>";
				wcout << Result << endl;
				if (argv[9] != CString("NULL"))
				{
					CFile XmlFile(argv[9], CFile::modeWrite | CFile::modeCreate);
					XmlFile.Write(Result, Result.GetLength());
					XmlFile.Close();
				}
				nRetCode = nSuccess;
			}
		}
	}
	else
	{
		_tprintf(_T("ERROR:  GetModuleHandle failed\n"));
		nRetCode = nSystemError;
	}
	return nRetCode;
}

DWORD static WINAPI TimeLimitValidatorThreadProc(LPVOID lpParam)
{
	LPTHREAD_PARAM pData;
	pData = (LPTHREAD_PARAM)lpParam;
	DWORD SleepTime = pData->TimeLimit - 100 > 0 ? pData->TimeLimit - 100 : pData->TimeLimit;
	while (true)
	{
		Sleep(SleepTime);
		DWORD ExitCode;
		GetExitCodeProcess(pData->ProcessHandle, &ExitCode);
		if (ExitCode != STILL_ACTIVE)
		{
			return 0;
		}
		FILETIME CreateTime, ExitTime, KernelTime, UserTime, CurrentTime;
		GetSystemTimeAsFileTime(&CurrentTime);

		GetProcessTimes(pData->ProcessHandle, &CreateTime, &ExitTime, &KernelTime, &UserTime);
		DWORD PhysicalTime = (CurrentTime.dwLowDateTime - CreateTime.dwLowDateTime) / 10000;
		TimeUsed = UserTime.dwLowDateTime / 10000;
		if (TimeUsed > HighPriorityTime)
		{
			SetPriorityClass(pData->ProcessHandle, IDLE_PRIORITY_CLASS);
			SetPriorityClass(GetCurrentProcess(), IDLE_PRIORITY_CLASS);
		}
		if (TimeUsed > pData->TimeLimit)
		{
			TerminateProcess(pData->ProcessHandle, NULL);
			return 1;
		}
		if (PhysicalTime > pData->TimeLimit * 3)
		{
			TerminateProcess(pData->ProcessHandle, NULL);
			return 2;
		}

	}
	return 0;
}