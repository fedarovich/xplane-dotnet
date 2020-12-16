#pragma once

#include <nethost.h>
#include <coreclr_delegates.h>
#include <hostfxr.h>
#include <tl/expected.hpp>
#include <XPLMDefs.h>

#include "platform.h"

typedef void (*DebugStringFunc)(const char* str);
typedef int (*StartFunc)(char* outName, char* outSig, char* outDesc);
typedef int (*EnableFunc)(void);
typedef void (*ReceiveMessageFunc)(XPLMPluginID inFrom, int inMsg, void* inParam);
typedef void (*DisableFunc)(void);
typedef void (*StopFunc)(void);

struct proxy_init_parameters
{
    DebugStringFunc in_debug;
    const char* in_startup_path;

    StartFunc out_start;
    EnableFunc out_enable;
    ReceiveMessageFunc out_receive_message;
    DisableFunc out_disable;
    StopFunc out_stop;
};

typedef int (*InitDelegate)(proxy_init_parameters* params);

class proxy
{
private:
    StartFunc plugin_start;
    EnableFunc plugin_enable;
    ReceiveMessageFunc plugin_receive_message;
    DisableFunc plugin_disable;
    StopFunc plugin_stop;

    proxy(const proxy_init_parameters *const params)
        :
        plugin_start(params->out_start),
        plugin_enable(params->out_enable),
        plugin_receive_message(params->out_receive_message),
        plugin_disable(params->out_disable),
        plugin_stop(params->out_stop)
    {
    }

    
public:
    static tl::expected<proxy, std::string> create(fs::path plugin_path, proxy_init_parameters *params);

    int start(char* outName, char* outSig, char* outDesc)
    {
        return plugin_start(outName, outSig, outDesc);
    }

    void stop()
    {
        plugin_stop();
    }

    int enable()
    {
        return plugin_enable();
    }

    void disable()
    {
        return plugin_disable();
    }

    void receive_message(XPLMPluginID inFrom, int inMsg, void* inParam)
    {
        plugin_receive_message(inFrom, inMsg, inParam);
    }
};
