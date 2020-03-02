#pragma once

#include <nethost.h>
#include <coreclr_delegates.h>
#include <hostfxr.h>
#include <tl/expected.hpp>

#include "platform.h"

struct start_parameters
{

};

class clr_host
{
private:
    load_assembly_and_get_function_pointer_fn load_assembly_and_get_function_pointer_f;

    clr_host(load_assembly_and_get_function_pointer_fn load_assembly_and_get_function_pointer_f)
        : load_assembly_and_get_function_pointer_f(load_assembly_and_get_function_pointer_f)
    {
    }

    
public:
    static tl::expected<clr_host, std::string> create(fs::path plugin_path = fs::path());

    int load_assembly_and_get_function_pointer(
        const char_t* assembly_path      /* Fully qualified path to assembly */,
        const char_t* type_name          /* Assembly qualified type name */,
        const char_t* method_name        /* Public static method name compatible with delegateType */,
        const char_t* delegate_type_name /* Assembly qualified delegate type name or null */,
        void* reserved           /* Extensibility parameter (currently unused and must be 0) */,
        /*out*/ void** delegate          /* Pointer where to store the function pointer result */) 
        const
    {
        return load_assembly_and_get_function_pointer_f(
            assembly_path,
            type_name,
            method_name,
            delegate_type_name,
            reserved,
            delegate
        );
    }
};
