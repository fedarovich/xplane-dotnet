#include "platform.h"
#include "XPLMUtilities.h"
#include "XPLMPlugin.h"

template <typename T>
std::string format_error(const char* format, T param)
{
    char t;
    auto len = snprintf(&t, 1, format, param);
    auto err_str = new char[len + 1];
    snprintf(err_str, len + 1, format, param);
    auto result = std::string(err_str);
    delete[] err_str;
    return result;
}

const std::filesystem::path get_plugin_path()
{
    char path[MAX_PATH * 2];
    XPLMGetPluginInfo(XPLMGetMyID(), nullptr, path, nullptr, nullptr);
    return std::filesystem::u8path(path).parent_path();
}

tl::expected<void*, std::string> load_library(const string_t& path)
{
    auto h = ::LoadLibraryW(path.c_str());
    if (h == nullptr)
    {
        return tl::make_unexpected(format_error("LoadLibraryW failed, error = %d", GetLastError()));
    }
    return (void*)h;
}

tl::expected<void*, std::string> get_export(void* h, const std::string& name)
{
    auto f = GetProcAddress((HMODULE)h, name.c_str());
    if (f == nullptr)
    {
        return tl::make_unexpected(format_error("GetProcAddress failed, error = %d", GetLastError()));
    }
    return f;
}