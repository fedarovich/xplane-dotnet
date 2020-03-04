#include <XPLMPlugin.h>
#if IBM
#define WINDOWS
#include <Windows.h>
#endif

#include <cstring>
#include <string>
#include <map>
#include <filesystem>

namespace fs = std::filesystem;

static std::map<int, fs::path> plugins;

static bool useNativePaths = false;

extern "C" XPLM_API void SimRegisterPlugin(int id, const char* path);

void SimRegisterPlugin(int id, const char* path)
{
    plugins[id] = fs::path(path);
}

XPLMPluginID XPLMGetMyID(void)
{
	return 1;
}

void XPLMGetPluginInfo(
    XPLMPluginID         inPlugin,
    char* outName,    /* Can be NULL */
    char* outFilePath,    /* Can be NULL */
    char* outSignature,    /* Can be NULL */
    char* outDescription)    /* Can be NULL */
{
    if (outFilePath)
    {
        auto value = plugins.find(inPlugin);
        if (value != plugins.cend())
        {
            std::string path = useNativePaths
                ? value->second.generic_u8string()
                : value->second.u8string();

            strcpy(outFilePath, path.c_str());
        }
    }
}

void XPLMEnableFeature(const char* inFeature, int inEnable)
{
    if (strcmp(inFeature, "XPLM_USE_NATIVE_PATHS") == 0)
    {
        useNativePaths = inEnable != 0;
    }
}

int XPLMIsFeatureEnabled(const char* inFeature)
{
    if (strcmp(inFeature, "XPLM_USE_NATIVE_PATHS") == 0)
    {
        return useNativePaths;
    }
    return 0;
}

