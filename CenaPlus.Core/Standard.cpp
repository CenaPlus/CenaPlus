#include <windows.h>
#include <cstdio>
#include <iostream>
#define max(a,b) ((a) > (b) ? (a) : (b))
using namespace std;

struct RetInfo{
    bool flag;
    char *ptr;
    RetInfo(){};
    RetInfo(bool f,char *p):flag(f),ptr(p)
    {
    }
};
//struct

const int AC_CODE = 0,PE_CODE = 1,WA_CODE = 2;//return code
const int MaxSize = 50 * 1024 * 1024;//50MB
char OutData[MaxSize],StdData[MaxSize],InData[100];//Data
char *OutPtr,*StdPtr;//Data pointer
int CountLine = 1;
FILE *InFp;
//Var and arr

int AC();
int WA();
int PE();
//Return information
inline bool IsBlank1(const char t);
inline bool IsBlank2(const char t);
RetInfo EndOfLine(char* p);
bool EndOfFile(char* p);
char* SkipBlank(char* p);
//Functions for Judge()
int Judge();

int main(int argc,char *argv[])
{
    if(argc != 4)
        return -1;
	SECURITY_ATTRIBUTES sa;
    ZeroMemory(&sa, sizeof(sa));
    sa.nLength = sizeof(SECURITY_ATTRIBUTES);
    sa.bInheritHandle = TRUE;
    sa.lpSecurityDescriptor = NULL;
    HANDLE hStd, hOut;
    DWORD ReadCount;
    hStd = CreateFile(argv[1], GENERIC_READ, FILE_SHARE_WRITE | FILE_SHARE_READ, &sa, OPEN_EXISTING, FILE_ATTRIBUTE_READONLY, NULL);
    hOut = CreateFile(argv[2], GENERIC_READ, FILE_SHARE_WRITE | FILE_SHARE_READ, &sa, OPEN_EXISTING, FILE_ATTRIBUTE_READONLY, NULL);
    ReadFile(hStd, StdData, MaxSize - 1, &ReadCount, NULL);
    ReadFile(hOut, OutData, MaxSize - 1, &ReadCount, NULL);
    InFp = fopen(argv[3],"r");
	return Judge();
}

int AC()
{
    fclose(InFp);
    return AC_CODE;
}

int WA()
{
    char *StdPtrT = max(StdData,StdPtr - 10),*OutPtrT = max(OutData,OutPtr - 10);
    int i;
    bool flag = false;
    cout << "Cena+ Local Judge Result" << endl;
    cout << "========================" << endl;
    cout << "Line: " << CountLine << "" << endl;
    cout << "Answer:" << endl;
    if(StdPtrT != StdData)
        cout<<"...";
    for(;StdPtrT != StdPtr;++StdPtrT)
        cout << *StdPtrT;
    for(i = 0;i < 10 && *StdPtrT != '\0';++i,++StdPtrT)
        cout << *StdPtrT;
    if(*StdPtrT != '\0')
        cout<<"...";
    cout<<endl;
    cout << "Yours: " << endl;
    if(OutPtrT != OutData)
        cout<<"...";
    for(;OutPtrT != OutPtr;++OutPtrT)
        cout << *OutPtrT;
    for(i = 0;i < 10 && *OutPtrT != '\0';++i,++OutPtrT)
        cout << *OutPtrT;
    if(*OutPtrT != '\0')
        cout<<"...";
    cout<<endl;
    for(i = 0;i < 20;++i)
        if((InData[i] = fgetc(InFp)) == EOF)
        {
            flag = true;
            break;
        }
    cout << "Input: " << endl;
    if(flag)
        i >>= 1;
    for(int j = 0;j < i;++j)
            cout << InData[j];
    if(i != 0)
        cout << "...";
    else
        cout << InData[0];
    fclose(InFp);
    return WA_CODE;
}

int PE()
{
    fclose(InFp);
    return PE_CODE;
}

inline bool IsBlank1(const char t)
{
    return t == ' ' || t == '\r' || t == '\t';
}

inline bool IsBlank2(const char t)
{
    return t == ' ' || t == '\r' || t == '\t' || t == '\n';
}

char* SkipBlank(char* p)
{
    while(IsBlank2(*p))
    {
        if(*p == '\n')
            ++CountLine;
        ++p;
    }
    return p;
}

RetInfo EndOfLine(char* p)
{
    while(IsBlank1(*p))
        ++p;
    return RetInfo(*p == '\n' || *p == '\0',p);
}

bool EndOfFile(char* p)
{
    while(IsBlank2(*p))
        ++p;
    return *p == '\0';
}

int Judge()
{
    RetInfo ret1,ret2;
    bool Skip = false;
    StdPtr = StdData;
    OutPtr = OutData;
    while(*StdPtr != '\0' && *OutPtr != '\0')
    {
        if(*StdPtr == '\n')
            ++CountLine;
        if(*StdPtr != *OutPtr)
        {
            ret1 = EndOfLine(StdPtr);
            ret2 = EndOfLine(OutPtr);
            if(ret1.flag && ret2.flag)
            {
                StdPtr = ret1.ptr;
                OutPtr = ret2.ptr;
            }
            else
            {
                StdPtr = SkipBlank(ret1.ptr);
                OutPtr = SkipBlank(ret2.ptr);
                Skip = true;
            }
            if(*StdPtr != *OutPtr && *StdPtr != '\0' && *OutPtr != '\0')
                return WA();
        }
        ++StdPtr;
        ++OutPtr;
    }
    if(EndOfFile(StdPtr) == false || EndOfFile(OutPtr) == false)
        return WA();
    return Skip ? PE() : AC();
}
