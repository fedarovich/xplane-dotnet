#include <XPLMUtilities.h>
#include <cstdio>

XPLM_API void XPLMDebugString(const char* inString)
{
	printf("%s\n", inString);
}