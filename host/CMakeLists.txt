# CMakeList.txt : Top-level CMake project file, do global configuration
# and include sub-projects here.
#
cmake_minimum_required (VERSION 3.15)

project ("host" CXX)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
set(CMAKE_CXX_EXTENSIONS OFF)

set(XPLANE_SDK_PATH "${CMAKE_CURRENT_SOURCE_DIR}/SDK")

if (CMAKE_SYSTEM_NAME MATCHES "Windows")
	add_compile_definitions(IBM=1 UNICODE _UNICODE)
	set(CMAKE_MSVC_RUNTIME_LIBRARY "MultiThreaded$<$<CONFIG:Debug>:Debug>DLL")
elseif (CMAKE_SYSTEM_NAME MATCHES "Linux")
	add_compile_definitions(LIN=1)
elseif (CMAKE_SYSTEM_NAME MATCHES "Darwin")
	add_compile_definitions(APL=1)
endif ()

add_compile_definitions(XPLM200 XPLM210 XPLM300 XPLM301 XPLM302 XPLM303)

# Include sub-projects.
add_subdirectory ("xphost")
add_subdirectory ("sim_xplm")
add_subdirectory ("sim")

