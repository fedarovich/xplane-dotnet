# CMakeList.txt : Top-level CMake project file, do global configuration
# and include sub-projects here.
#
cmake_minimum_required (VERSION 3.14)

project ("host" CXX)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
set(CMAKE_CXX_EXTENSIONS OFF)

set(XPLANE_SDK_PATH "${CMAKE_CURRENT_SOURCE_DIR}/SDK")

if (WIN32)
	add_compile_definitions(IBM=1 UNICODE _UNICODE)
endif (WIN32)

add_compile_definitions(XPLM200 XPLM210 XPLM300 XPLM301)

# Include sub-projects.
add_subdirectory ("xphost")
