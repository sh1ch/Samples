#include "pch.h"
#include "memory.h"
#include <psapi.h>

/// <summary>
/// �������[��Ɋm�ۂ���Ă��镨���I�ȃ������[�ʂ��擾���܂��B
/// </summary>
/// <returns>�����I�ȃ������[�� (byte)�B�������擾�ł��Ȃ����Ƃ� -1 ��ԋp���܂��B</returns>
long GetWorkingSet()
{
	PROCESS_MEMORY_COUNTERS info;

	if (!GetProcessMemoryInfo(GetCurrentProcess(), &info, sizeof(info)))
	{
		return -1;
	}

	return (SIZE_T)info.WorkingSetSize;
}

/// <summary>
/// �������[��Ɋm�ۂ���Ă��镨���I�ȃ������[�ʁ{�X�g���[�W��ɑޔ��������������[�ʂ̍��v�l���擾���܂��B
/// </summary>
/// <returns>�����I�ȃ������[�� (byte)�B�������擾�ł��Ȃ����Ƃ� -1 ��ԋp���܂��B</returns>
long GetWorkingSetAndSwap()
{
	PROCESS_MEMORY_COUNTERS_EX info;
	
	if (!GetProcessMemoryInfo(GetCurrentProcess(), (PROCESS_MEMORY_COUNTERS*)&info, sizeof(info)))
	{
		return -1;
	}

	return (SIZE_T)info.PrivateUsage;
}

