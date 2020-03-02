// xphost.cpp : Defines the entry point for the application.
//

#include "xphost.h"

#ifndef XPLM301
	#error This is made to be compiled against the XPLM301 SDK
#endif

#include "platform.h"
#include "clrhost.h"

#include <optional>

using namespace std;

#include <XPLMDefs.h>
#include <XPLMUtilities.h>

#if IBM
BOOL APIENTRY DllMain(HANDLE hModule,
    DWORD ul_reason_for_call,
    LPVOID lpReserved)
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}
#endif

std::optional<clr_host> host;

PLUGIN_API int XPluginStart(
    char* outName,
    char* outSig,
    char* outDesc)
{
    auto root_path = get_plugin_path();
    if (root_path.empty())
    {
        XPLMDebugString("Failed to get plugin path.");
        return 0;
    }
    
    auto clrhost = clr_host::create();
    if (!clrhost)
    {
        XPLMDebugString(clrhost.error().c_str());
        return 0;
    }
    host = *clrhost;

    return 0; // TODO
}

PLUGIN_API void	XPluginStop(void)
{
}

PLUGIN_API void XPluginDisable(void) 
{

}

PLUGIN_API int  XPluginEnable(void)
{
    return 1; 
}

PLUGIN_API void XPluginReceiveMessage(XPLMPluginID inFrom, int inMsg, void* inParam)
{

}
