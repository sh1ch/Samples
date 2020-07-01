#include "pch.h"
#include "memory.h"
#include <psapi.h>

/// <summary>
/// メモリー上に確保されている物理的なメモリー量を取得します。
/// </summary>
/// <returns>物理的なメモリー量 (byte)。正しく取得できなったとき -1 を返却します。</returns>
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
/// メモリー上に確保されている物理的なメモリー量＋ストレージ上に退避させたメモリー量の合計値を取得します。
/// </summary>
/// <returns>物理的なメモリー量 (byte)。正しく取得できなったとき -1 を返却します。</returns>
long GetWorkingSetAndSwap()
{
	PROCESS_MEMORY_COUNTERS_EX info;
	
	if (!GetProcessMemoryInfo(GetCurrentProcess(), (PROCESS_MEMORY_COUNTERS*)&info, sizeof(info)))
	{
		return -1;
	}

	return (SIZE_T)info.PrivateUsage;
}

