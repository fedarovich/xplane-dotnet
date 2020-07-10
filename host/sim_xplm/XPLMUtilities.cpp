#include <XPLMUtilities.h>
#include <XPLMPlugin.h>

#include <cstdio>
#include <cstring>
#include <string>
#include <filesystem>

#if LIN
#include <dlfcn.h>
#include <limits.h>
#include <unistd.h>
#elif APL
#include <dlfcn.h>
#include <limits.h>
#endif

void XPLMDebugString(const char* inString)
{
	printf("%s", inString);
}

#if IBM
void XPLMGetSystemPath(char* outSystemPath)
{
    wchar_t buffer[512];
    GetModuleFileNameW(NULL, buffer, 512);
    auto path = std::filesystem::path(buffer).parent_path();
    std::string path_str = XPLMIsFeatureEnabled("XPLM_USE_NATIVE_PATHS")
        ? path.generic_u8string()
        : path.u8string();
    strcpy(outSystemPath, path_str.c_str());
}
#elif LIN
void XPLMGetSystemPath(char* outSystemPath)
{
    char buffer[PATH_MAX];
    int len = readlink("/proc/self/exe", buffer, PATH_MAX);
    if (len == -1)
    {
        *outSystemPath = '\0';
        return;
    }
    
    buffer[len] = '\0';
    auto path = std::filesystem::path(buffer).parent_path();
    std::string path_str = XPLMIsFeatureEnabled("XPLM_USE_NATIVE_PATHS")
        ? path.generic_u8string()
        : path.u8string();
    strcpy(outSystemPath, path_str.c_str());
}
#endif