#pragma once


extern "C"
{
	__declspec(dllexport) long GetWorkingSet();
	__declspec(dllexport) long GetWorkingSetAndSwap();
}