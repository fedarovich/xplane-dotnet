#include <XPLMPlugin.h>
#if IBM
#define WINDOWS
#include <Windows.h>
#endif

#include <string>
#include <map>

static std::map<int, std::string> plugins;

extern "C" XPLM_API void SimRegisterPlugin(int id, const char* path);

void SimRegisterPlugin(int id, const char* path)
{
    plugins[id] = std::string(path);
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
            strcpy_s(outFilePath, 256, value->second.c_str());
        }
    }
}
