# CMakeList.txt : CMake project for xphost, include source and define
# project specific logic here.
#
cmake_minimum_required (VERSION 3.14)

set (XPHOST_SOURCES "xphost.cpp" "xphost.h" "clrhost.cpp" "clrhost.h" "platform.h")

if (WIN32)
	set (XPHOST_SOURCES ${XPHOST_SOURCES} "platform.win.cpp")
else ()
	set (XPHOST_SOURCES ${XPHOST_SOURCES} "platform.unix.cpp")
endif ()

# Add source to this project's executable.
add_library (xphost SHARED ${XPHOST_SOURCES})

# TODO: Add tests and install targets if needed..

target_include_directories (xphost PRIVATE "${XPLANE_SDK_PATH}/CHeaders/XPLM" "${XPLANE_SDK_PATH}/CHeaders/Widgets")

if (WIN32)
    target_link_directories(xphost PRIVATE "${XPLANE_SDK_PATH}/Libraries/Win/" )
	target_link_libraries(xphost PRIVATE "XPLM_64.lib" "XPWidgets_64.lib")
endif (WIN32)

find_path(NETHOST_INCLUDE_DIRS nethost.h)
find_library(NETHOST_LIBRARY NAMES libnethost nethost)
target_include_directories(xphost PRIVATE ${NETHOST_INCLUDE_DIRS})
target_link_libraries(xphost PRIVATE ${NETHOST_LIBRARY})

find_package(tl-expected CONFIG REQUIRED)
target_link_libraries(xphost PRIVATE tl::expected)