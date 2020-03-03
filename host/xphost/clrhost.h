#pragma once

#include <nethost.h>
#include <coreclr_delegates.h>
#include <hostfxr.h>
#include <tl/expected.hpp>

#include "platform.h"

struct start_parameters
{
    const char* name;
    const char* sig;
    const char* desc;
    const void* debug;
    const void* get_plugin_path;
};

class clr_host
{
private:
    clr_host(
        fs::path plugin_path,
        load_assembly_and_get_function_pointer_fn load_assembly_and_get_function_pointer, 
        bool &success)
    {
#if IBM
        fs::path assembly = plugin_path / std::wstring(STR("xpproxy.dll"));
        const string_t assembly_path = assembly.wstring();
#else
        fs::path assembly = plugin_path / std::string(STR("xpproxy.dll"));
        const string_t assembly_path = assembly.string();
#endif
        const char_t* proxy_type = STR("XP.Proxy.PluginProxy, xpproxy");
        void* delegate = nullptr;
        auto result = load_assembly_and_get_function_pointer(
            assembly_path.c_str(),
            proxy_type,
            STR("XPluginStart"),
            STR("XP.Proxy.StartDelegate, xpproxy"),
            nullptr,
            &delegate);
    }

    
public:
    static tl::expected<clr_host, std::string> create(fs::path plugin_path);
};
