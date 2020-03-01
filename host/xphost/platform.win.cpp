#include "platform.h"
#include "XPLMUtilities.h"

inline void log_windows_error(const char* format)
{
    int err = GetLastError();
    char err_str[64];
    snprintf(err_str, 64, format, err);
    XPLMDebugString(err_str);
}

const std::filesystem::path get_plugin_path()
{
    char_t path[MAX_PATH];

    HMODULE hm = NULL;
    if (GetModuleHandleEx(GET_MODULE_HANDLE_EX_FLAG_FROM_ADDRESS | GET_MODULE_HANDLE_EX_FLAG_UNCHANGED_REFCOUNT, (LPCTSTR)&get_plugin_path, &hm) == 0)
    {
        log_windows_error("GetModuleHandle failed, error = %d");
        return std::filesystem::path();
    }

    if (GetModuleFileName(hm, path, MAX_PATH) == 0)
    {
        log_windows_error("GetModuleFileName failed, error = %d");
        return std::filesystem::path();
    }

    return std::filesystem::path(path).parent_path();
}

const void* load_library(string_t path)
{
    auto h = ::LoadLibraryW(path.c_str());
    if (h == nullptr)
    {
        log_windows_error("LoadLibraryW failed, error = %d");
    }
    return (void*)h;
}

const void* get_export(const void* h, const char* name)
{
    void* f = GetProcAddress((HMODULE)h, name);
    if (f == nullptr)
    {
        log_windows_error("GetProcAddress failed, error = %d");
    }
    return f;
}