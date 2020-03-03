#pragma once

#include <coreclr_delegates.h>

#ifdef IBM
	#include <Windows.h>

	#define STR(s) L ## s
	#define CH(c) L ## c
	#define STRING(s) std::wstring(STR(s))
	#define DIR_SEPARATOR L'\\'
#else
	#include <dlfcn.h>
	#include <limits.h>

	#define STR(s) s
	#define CH(c) c
	#define STRING(s) std::string(STR(s))
	#define DIR_SEPARATOR '/'
	#define MAX_PATH PATH_MAX
#endif

#include <string>
#include <filesystem>
#include <tl/expected.hpp>

using string_t = std::basic_string<char_t>;

namespace fs = std::filesystem;

const fs::path get_plugin_path();
const fs::path get_plugin_full_name();

tl::expected<void*, std::string> load_library(const string_t& path);
tl::expected<void*, std::string> get_export(void* handle, const std::string& name);

template <typename TFunc>
tl::expected<TFunc, std::string> get_export(void* handle, const std::string& name)
{
	return get_export(handle, name).map([] (auto f) { return (TFunc)f; });
}