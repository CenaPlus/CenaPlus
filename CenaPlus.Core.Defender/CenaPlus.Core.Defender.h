// 下列 ifdef 块是创建使从 DLL 导出更简单的
// 宏的标准方法。此 DLL 中的所有文件都是用命令行上定义的 CENAPLUSCOREDEFENDER_EXPORTS
// 符号编译的。在使用此 DLL 的
// 任何其他项目上不应定义此符号。这样，源文件中包含此文件的任何其他项目都会将
// CENAPLUSCOREDEFENDER_API 函数视为是从 DLL 导入的，而此 DLL 则将用此宏定义的
// 符号视为是被导出的。
#ifdef CENAPLUSCOREDEFENDER_EXPORTS
#define CENAPLUSCOREDEFENDER_API __declspec(dllexport)
#else
#define CENAPLUSCOREDEFENDER_API __declspec(dllimport)
#endif

// 此类是从 CenaPlus.Core.Defender.dll 导出的
class CENAPLUSCOREDEFENDER_API CCenaPlusCoreDefender {
public:
	CCenaPlusCoreDefender(void);
	// TODO:  在此添加您的方法。
};

extern CENAPLUSCOREDEFENDER_API int nCenaPlusCoreDefender;

CENAPLUSCOREDEFENDER_API int fnCenaPlusCoreDefender(void);
