// ���� ifdef ���Ǵ���ʹ�� DLL �������򵥵�
// ��ı�׼�������� DLL �е������ļ��������������϶���� CENAPLUSCOREDEFENDER_EXPORTS
// ���ű���ġ���ʹ�ô� DLL ��
// �κ�������Ŀ�ϲ�Ӧ����˷��š�������Դ�ļ��а������ļ����κ�������Ŀ���Ὣ
// CENAPLUSCOREDEFENDER_API ������Ϊ�Ǵ� DLL ����ģ����� DLL ���ô˺궨���
// ������Ϊ�Ǳ������ġ�
#ifdef CENAPLUSCOREDEFENDER_EXPORTS
#define CENAPLUSCOREDEFENDER_API __declspec(dllexport)
#else
#define CENAPLUSCOREDEFENDER_API __declspec(dllimport)
#endif

// �����Ǵ� CenaPlus.Core.Defender.dll ������
class CENAPLUSCOREDEFENDER_API CCenaPlusCoreDefender {
public:
	CCenaPlusCoreDefender(void);
	// TODO:  �ڴ�������ķ�����
};

extern CENAPLUSCOREDEFENDER_API int nCenaPlusCoreDefender;

CENAPLUSCOREDEFENDER_API int fnCenaPlusCoreDefender(void);
