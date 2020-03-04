#if (IBM)
#define WINDOWS
#include <Windows.h>
#else
#include <dlfcn.h>
#include <limits.h>
#endif

#include <iostream>
#include <filesystem>
#include <string>
#include <assert.h>
#include <cstdio>

using namespace std;

#if __has_include(<filesystem>)
#include <filesystem>
namespace fs = std::filesystem;
#elif __has_include(<experimental/filesystem>)
#include <experimental/filesystem>
namespace fs = std::experimental::filesystem;
#elif
#include <boost/filesystem>
namespace fs = boost::filesystem;
#endif

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
        void* h = dlopen(path, RTLD_LAZY | RTLD_GLOBAL);
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
typedef int  (*XPluginStart)(char* outName, char* outSig, char* outDesc);
typedef void (*XPluginStop)(void);
typedef int  (*XPluginEnable)(void);
typedef void (*XPluginDisable)(void);
typedef void (*XPluginReceiveMessage)(int inFrom, int inMsg, void* inParam);


#if defined(WINDOWS)
int __cdecl wmain(int argc, wchar_t* argv[])
#else
int main(int argc, char* argv[])
#endif
{
	auto startup_folder = fs::canonical(fs::path(argv[0]).parent_path());
    auto plugins_folder = startup_folder / STR("Resources") / STR("plugins");
    auto sample_plugin_folder = plugins_folder / STR("sample") / XP_PLATFORM;
    auto sample_plugin_path = sample_plugin_folder / STR("sample.xpl");
    assert(fs::exists(sample_plugin_path));
#if defined(WINDOWS)
    AddDllDirectory(plugins_folder.c_str());
    AddDllDirectory(sample_plugin_folder.c_str());
    auto xplm_path = plugins_folder / STR("XPLM_64.dll");
    auto xplm_handle = load_library(xplm_path.c_str());
    char plugin_path_utf8[256];
    WideCharToMultiByte(CP_UTF8, WC_ERR_INVALID_CHARS, sample_plugin_path.c_str(), -1, plugin_path_utf8, 256, NULL, NULL);
    auto register_plugin = (SimRegisterPlugin)get_export(xplm_handle, "SimRegisterPlugin");
    register_plugin(1, plugin_path_utf8);
#else
#if LIN
    auto xplm_path = plugins_folder / STR("XPLM_64.so");
#else
    auto xplm_path = plugins_folder / STR("XPLM.framework") / STR("XPLM");
#endif
    auto xplm_handle = load_library(xplm_path.c_str());
    auto register_plugin = (SimRegisterPlugin)get_export(xplm_handle, "SimRegisterPlugin");
    register_plugin(1, sample_plugin_path.c_str());

#endif
    auto plugin_handle = load_library(sample_plugin_path.c_str());
    auto plugin_start = (XPluginStart)get_export(plugin_handle, "XPluginStart");
    char name[256], sig[256], desc[256];
    if (!plugin_start(name, sig, desc))
    {
        cout << "Failed to start plugin." << endl;
        return 1;
    }
    auto plugin_enable = (XPluginEnable)get_export(plugin_handle, "XPluginEnable");
    if (!plugin_enable())
    {
        cout << "Failed to enable plugin." << endl;
        return 1;
    }
    auto plugin_receive_message = (XPluginReceiveMessage)get_export(plugin_handle, "XPluginReceiveMessage");
    plugin_receive_message(0, 42, (void*)0xDEADBEEFDEADBEEF);
    auto plugin_disable = (XPluginDisable)get_export(plugin_handle, "XPluginDisable");
    plugin_disable();
    auto plugin_stop = (XPluginStop)get_export(plugin_handle, "XPluginStop");
    plugin_stop();

    return 0;
}