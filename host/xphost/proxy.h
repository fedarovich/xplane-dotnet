#pragma once

#include <nethost.h>
#include <coreclr_delegates.h>
#include <hostfxr.h>
#include <tl/expected.hpp>
#include <XPLMDefs.h>

#include "platform.h"

struct start_parameters
{
    char* name;
    char* sig;
    char* desc;
    const void* xplm_handle;
    const void* widgets_handle;
    const char* plugin_path;
};

typedef int (*StartDelegate)(start_parameters* params);
typedef int (*EnableDelegate)(void);
typedef void (*DisableDelegate)(void);
typedef void (*StopDelegate)(void);
typedef void (*ReceiveMessageDelegate)(XPLMPluginID inFrom, int inMsg, void* inParam);

class proxy
{
private:
    StartDelegate plugin_start;
    StopDelegate plugin_stop;
    EnableDelegate plugin_enable;
    DisableDelegate plugin_disable;
    ReceiveMessageDelegate plugin_receive_message;

    proxy(
        StartDelegate plugin_start,
        StopDelegate plugin_stop,
        EnableDelegate plugin_enable,
        DisableDelegate plugin_disable,
        ReceiveMessageDelegate plugin_receive_message)
        :
        plugin_start(plugin_start),
        plugin_stop(plugin_stop),
        plugin_enable(plugin_enable),
        plugin_disable(plugin_disable),
        plugin_receive_message(plugin_receive_message)
    {
    }

    
public:
    static tl::expected<proxy, std::string> create(fs::path plugin_path);

    int start(start_parameters* params)
    {
        return plugin_start(params);
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
