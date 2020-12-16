
#include "proxy.h"

tl::expected<proxy, std::string> proxy::create(fs::path plugin_path, proxy_init_parameters *params) {
    auto runtime_path = plugin_path / STR("runtime");
    
    char_t buffer[MAX_PATH];
    size_t buffer_size = sizeof(buffer) / sizeof(char_t);
    get_hostfxr_parameters hostfxr_parameters
    {
        sizeof(get_hostfxr_parameters),
        nullptr,
        fs::exists(runtime_path) ? runtime_path.c_str() : nullptr
    };

    int rc = get_hostfxr_path(buffer, &buffer_size, &hostfxr_parameters);
    if (rc != 0)
        return tl::make_unexpected("Failed to find hostfxr library.");

    auto lib = load_library(buffer);
    if (!lib)
        return tl::make_unexpected(lib.error());

    auto initialize_for_runtime_config = get_export<hostfxr_initialize_for_runtime_config_fn>(*lib, "hostfxr_initialize_for_runtime_config");
    if (!initialize_for_runtime_config)
        return tl::make_unexpected(initialize_for_runtime_config.error());

    auto get_runtime_delegate = get_export<hostfxr_get_runtime_delegate_fn>(*lib, "hostfxr_get_runtime_delegate");
    if (!get_runtime_delegate)
        return tl::make_unexpected(get_runtime_delegate.error());

    auto close = get_export<hostfxr_close_fn>(*lib, "hostfxr_close");
    if (!close)
        return tl::make_unexpected(close.error());

    auto config_path = plugin_path / STR("xpproxy.runtimeconfig.json");
    hostfxr_initialize_parameters init_parameters
    {
        sizeof(hostfxr_initialize_parameters),
        nullptr,
        hostfxr_parameters.dotnet_root
    };
    hostfxr_handle handle = nullptr;

    auto result = (*initialize_for_runtime_config)(config_path.c_str(), &init_parameters, &handle);

    if (result < 0 || handle == nullptr)
    {
        (*close)(handle);
        return tl::make_unexpected("Failed to load runtime.");
    }

    void* load_assembly_and_get_function_pointer_ptr = nullptr;
    result = (*get_runtime_delegate)(handle, hdt_load_assembly_and_get_function_pointer, &load_assembly_and_get_function_pointer_ptr);
    if (result != 0 || &load_assembly_and_get_function_pointer_ptr == nullptr) {
        (*close)(handle);
        return tl::make_unexpected("Failed to load runtime.");
    }

    (*close)(handle);

    auto get_delegate = (load_assembly_and_get_function_pointer_fn)(load_assembly_and_get_function_pointer_ptr);

    const fs::path assembly_path = plugin_path / string_t(STR("xpproxy.dll"));
    const char_t* proxy_type = STR("XP.Proxy.PluginProxy, xpproxy");
    void* delegate = nullptr;
    
    result = (*get_delegate)(assembly_path.c_str(), proxy_type, STR("Initialize"), STR("XP.Proxy.InitializeDelegate, xpproxy"), nullptr, &delegate);
    if (result != 0 || delegate == nullptr)
        return tl::make_unexpected("Failed to get Initialize method.");
    auto init = (InitDelegate)delegate;
    auto initialized = init(params);
    if (!initialized)
        return tl::make_unexpected("Failed to initialize the plugin proxy.");

    return std::move(proxy(params));
}
