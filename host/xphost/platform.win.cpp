#include "platform.h"
#include "XPLMUtilities.h"
#include "XPLMPlugin.h"

template <typename T>
std::string format_error(const char* format, T param)
{
    char t;
    auto len = snprintf(&t, 1, format, param) + 1;
    auto err_str = new char[len];
    snprintf(err_str, len, format, param);
    auto result = std::string(err_str);
    delete[] err_str;
    return result;
}

const std::filesystem::path get_plugin_path()
{
    return get_plugin_full_name().parent_path();
}

const std::filesystem::path get_plugin_full_name()
{
    char path[MAX_PATH * 2];
    XPLMGetPluginInfo(XPLMGetMyID(), nullptr, path, nullptr, nullptr);
    return fs::u8path(path);
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

void* get_library_handle(const void* symbol)
{
    HMODULE module = nullptr;
    GetModuleHandleEx(GET_MODULE_HANDLE_EX_FLAG_FROM_ADDRESS | GET_MODULE_HANDLE_EX_FLAG_UNCHANGED_REFCOUNT, (LPCWSTR)symbol, &module);
    return module;
}