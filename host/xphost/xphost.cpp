// xphost.cpp : Defines the entry point for the application.
//

#include "xphost.h"

#ifndef XPLM301
	#error This is made to be compiled against the XPLM301 SDK
#endif

#include "platform.h"
#include "proxy.h"

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

std::optional<proxy> plugin_proxy;

void get_plugin_full_name_string(char* str, int n) {
    strcpy_s(str, n, get_plugin_full_name().u8string().c_str());
}

PLUGIN_API int XPluginStart(
    char* outName,
    char* outSig,
    char* outDesc)
{
    XPLMDebugString("Loaded xphost.");

    auto full_name = get_plugin_full_name().u8string();
    auto root_path = get_plugin_path();
    if (root_path.empty())
    {
        XPLMDebugString("Failed to get plugin path.");
        return 0;
    }
    
    auto proxy_result = proxy::create(root_path);
    if (!proxy_result)
    {
        XPLMDebugString(proxy_result.error().c_str());
        return 0;
    }
    plugin_proxy = *proxy_result;

    start_parameters params {
        outName,
        outSig,
        outDesc,
        get_library_handle(&XPLMDebugString),
        nullptr,
        full_name.c_str()
    };

    
    auto result = plugin_proxy->start(&params);
    return result;
}

PLUGIN_API void	XPluginStop(void)
{
    if (plugin_proxy.has_value())
    {
        plugin_proxy->stop();
    }
}

PLUGIN_API void XPluginDisable(void) 
{
    if (plugin_proxy.has_value())
    {
        plugin_proxy->disable();
    }
}

PLUGIN_API int  XPluginEnable(void)
{
    return plugin_proxy.has_value() ? plugin_proxy->enable() : 0;
}

PLUGIN_API void XPluginReceiveMessage(XPLMPluginID inFrom, int inMsg, void* inParam)
{
    if (plugin_proxy.has_value())
    {
        plugin_proxy->receive_message(inFrom, inMsg, inParam);
    }
}
