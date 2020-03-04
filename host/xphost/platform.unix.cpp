#include "platform.h"
#include "XPLMUtilities.h"
#include "XPLMPlugin.h"

template <typename T>
std::string format_error(const char* format, T param)
{
    char t;
    auto len = snprintf(&t, 1, format, param) + 1;
    char err_str[len];
    snprintf(err_str, len, format, param);
    return std::string(err_str);
}

const fs::path get_plugin_path()
{
    char path[MAX_PATH];
    XPLMGetPluginInfo(XPLMGetMyID(), nullptr, path, nullptr, nullptr);
    return fs::path(path).parent_path();
}

const fs::path get_plugin_full_name()
{
    char path[MAX_PATH];
    XPLMGetPluginInfo(XPLMGetMyID(), nullptr, path, nullptr, nullptr);
    return fs::path(path);
}

const fs::path get_startup_path()
{
    char path[MAX_PATH];
    XPLMGetSystemPath(path);
    return fs::path(path);
}

tl::expected<void*, std::string> load_library(const string_t& path)
{
    void* h = dlopen(path.c_str(), RTLD_LAZY | RTLD_LOCAL);
    if (h == nullptr) 
    {
        tl::make_unexpected(format_error("Failed to load library '%s'.", path.c_str()));
    }
    return h;
}

tl::expected<void*, std::string> get_export(void* h, const std::string& name)
{
    void* f = dlsym(h, name.c_str());
    if (f == nullptr)
    {
        tl::make_unexpected(format_error("Failed to load library '%s'.", name.c_str()));
    }
    return f;
}
