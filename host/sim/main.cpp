#if (IBM)
#define WINDOWS
#include <Windows.h>
#else
#include <dlfcn.h>
#include <limits.h>
#endif

#include <iostream>
#include <filesystem>
#include <assert.h>

using namespace std;

namespace fs = std::filesystem;

#ifdef WINDOWS
    #define STR(s) wstring(L ## s)
    #define XP_PLATFORM STR("win_x64")
    void* load_library(const wchar_t* path)
    {
        HMODULE h = ::LoadLibraryExW(path, NULL, LOAD_LIBRARY_SEARCH_USER_DIRS);
        if (h == nullptr)
        {
            auto error = GetLastError();
            cout << "Failed to load plugin: " << error << endl;
        }
        assert(h != nullptr);
        return (void*)h;
    }
    void* get_export(void* h, const char* name)
    {
        void* f = ::GetProcAddress((HMODULE)h, name);
        assert(f != nullptr);
        return f;
    }
#else
    #define STR(s) string(s)
    #if (APL)
        #define XP_PLATFORM STR("mac_x64")
    #else
        #define XP_PLATFORM STR("lin_x64")
    #endif
    void* load_library(const char* path)
    {
        void* h = dlopen(path, RTLD_LAZY | RTLD_LOCAL);
        assert(h != nullptr);
        return h;
    }
    void* get_export(void* h, const char* name)
    {
        void* f = dlsym(h, name);
        assert(f != nullptr);
        return f;
    }
#endif

typedef void (*SimRegisterPlugin)(int id, const char* path);
typedef int (*XPluginStart)(char* outName, char* outSig, char* outDesc);



#if defined(WINDOWS)
int __cdecl wmain(int argc, wchar_t* argv[])
#else
int main(int argc, char* argv[])
#endif
{
	auto startup_folder = fs::path(argv[0]).parent_path();
    auto plugins_folder = startup_folder / STR("Resources") / STR("plugins");
    auto sample_plugin_folder = plugins_folder / STR("sample") / XP_PLATFORM;
    auto sample_plugin_path = sample_plugin_folder / STR("sample.xpl");
    assert(fs::exists(sample_plugin_path));
#if defined(WINDOWS)
    AddDllDirectory(plugins_folder.wstring().c_str());
    AddDllDirectory(sample_plugin_folder.wstring().c_str());
    auto xplm_handle = load_library((plugins_folder / STR("XPLM_64.dll")).wstring().c_str());
    char plugin_path_utf8[256];
    WideCharToMultiByte(CP_UTF8, WC_ERR_INVALID_CHARS, sample_plugin_path.wstring().c_str(), -1, plugin_path_utf8, 256, NULL, NULL);
    auto register_plugin = (SimRegisterPlugin)get_export(xplm_handle, "SimRegisterPlugin");
    register_plugin(1, plugin_path_utf8);
    auto plugin_handle = load_library(sample_plugin_path.wstring().c_str());
#else
    auto plugin_handle = load_library(plugin_path.string().c_str());
#endif
    auto plugin_start = (XPluginStart)get_export(plugin_handle, "XPluginStart");
    char name[256], sig[256], desc[256];
    plugin_start(name, sig, desc);
   
    return 0;
}