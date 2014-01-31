#include "stdafx.h"

#ifndef _SMARTPROTECTER_H
#define _SMARTPROTECTER_H

DWORD EnablePrivilege();
BOOL LoadRemoteDLL(DWORD dwProcessId, LPWSTR lpszLibName);

#endif
