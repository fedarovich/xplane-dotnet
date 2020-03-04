cmake_minimum_required (VERSION 3.15)

set (SIM_SOURCES "main.cpp")

# Add source to this project's executable.
add_executable (sim ${SIM_SOURCES})

string(TOUPPER "${CMAKE_BUILD_TYPE}" CMAKE_BUILD_TYPE_UPPER)
if (CMAKE_BUILD_TYPE_UPPER STREQUAL "DEBUG")
    message("debug mode")
	set (DOTNET_CONFIG Debug)
else ()
	message("release mode")
	set (DOTNET_CONFIG Release)
endif ()


if (CMAKE_SYSTEM_NAME MATCHES "Windows")
	set (DOTNET_RID win-x64)
	set (XP_RID win_x64)
	set (NETHOST nethost.dll)
elseif (CMAKE_SYSTEM_NAME MATCHES "Linux")
	set (DOTNET_RID linux-x64)
	set (XP_RID lin_x64)
	set (NETHOST libnethost.so)
	target_link_libraries(sim "dl" "stdc++fs")
elseif (CMAKE_SYSTEM_NAME MATCHES "Darwin")
	set (DOTNET_RID osx-x64)
	set (XP_RID mac_x64)
	set (NETHOST libnethost.dylib)
endif ()

add_custom_command (TARGET sim POST_BUILD COMMAND ${CMAKE_COMMAND} -E 
	copy "$<TARGET_FILE:xphost>" "$<TARGET_FILE_DIR:sim>/Resources/plugins/sample/${XP_RID}/sample.xpl")
	   
add_custom_command (TARGET sim POST_BUILD COMMAND ${CMAKE_COMMAND} -E 
	copy "$<TARGET_FILE:sim_xplm>" "$<TARGET_FILE_DIR:sim>/Resources/plugins/")

add_custom_command (TARGET sim POST_BUILD COMMAND 
	dotnet publish "${CMAKE_CURRENT_LIST_DIR}/../../src/XP.Proxy/XP.Proxy.csproj" -c ${DOTNET_CONFIG} -r ${DOTNET_RID}
		-o "$<TARGET_FILE_DIR:sim>/Resources/plugins/sample/${XP_RID}/")

add_custom_command (TARGET sim POST_BUILD COMMAND 
	dotnet publish "${CMAKE_CURRENT_LIST_DIR}/../../src/XP.SamplePlugin/XP.SamplePlugin.csproj" -c ${DOTNET_CONFIG} -r ${DOTNET_RID}
		-o "$<TARGET_FILE_DIR:sim>/Resources/plugins/sample/${XP_RID}/")

if (CMAKE_SYSTEM_NAME MATCHES "Windows")
	add_custom_command (TARGET sim POST_BUILD COMMAND ${CMAKE_COMMAND} -E 
		copy "$<TARGET_FILE_DIR:xphost>/${NETHOST}" "$<TARGET_FILE_DIR:sim>/Resources/plugins/sample/win_x64/")
endif ()