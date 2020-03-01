#pragma once

#include <coreclr_delegates.h>

#ifdef IBM
	#include <Windows.h>

	#define STR(s) L ## s
	#define CH(c) L ## c
	#define DIR_SEPARATOR L'\\'
#else
	#include <dlfcn.h>
	#include <limits.h>

	#define STR(s) s
	#define CH(c) c
	#define DIR_SEPARATOR '/'
	#define MAX_PATH PATH_MAX
#endif

#include <string>
#include <filesystem>

using string_t = std::basic_string<char_t>;

const std::filesystem::path get_plugin_path();
const void* load_library(string_t path);
const void* get_export(const void* handle, const char* name);