# CMakeList.txt : CMake project for xphost, include source and define
# project specific logic here.
#
cmake_minimum_required (VERSION 3.15)

# Add source to this project's executable.
add_library (sim_xplm SHARED "XPLMPlugin.cpp" "XPLMUtilities.cpp")

set_target_properties(sim_xplm PROPERTIES OUTPUT_NAME "XPLM_64")

add_compile_definitions(XPLM=1)

target_include_directories (sim_xplm PRIVATE "${XPLANE_SDK_PATH}/CHeaders/XPLM")

# TODO: Add tests and install targets if needed..



