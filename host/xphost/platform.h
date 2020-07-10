#pragma once

#include <coreclr_delegates.h>

#ifdef IBM
	#include <Windows.h>

	#define STR(s) L ## s
	#define CH(c) L ## c
	#define STRING(s) std::wstring(STR(s))
	#define DIR_SEPARATOR L'\\'
	#define ENDL "\r\n"
#else
	#include <dlfcn.h>
	#include <limits.h>

	#define STR(s) s
	#define CH(c) c
	#define STRING(s) std::string(STR(s))
	#define DIR_SEPARATOR '/'
	#define MAX_PATH PATH_MAX
	#define ENDL "\n"
#endif

#include <string>
#include <tl/expected.hpp>

using string_t = std::basic_string<char_t>;

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

const fs::path get_plugin_path();
const fs::path get_plugin_full_name();
const fs::path get_startup_path();

tl::expected<void*, std::string> load_library(const string_t& path);
tl::expected<void*, std::string> get_export(void* handle, const std::string& name);

template <typename TFunc>
tl::expected<TFunc, std::string> get_export(void* handle, const std::string& name)
{
	return get_export(handle, name).map([] (auto f) { return (TFunc)f; });
}
