cmake_minimum_required (VERSION 3.15)

set (SIM_SOURCES "main.cpp")

# Add source to this project's executable.
add_executable (sim ${SIM_SOURCES})


if (CMAKE_SYSTEM_NAME MATCHES "Windows")
	add_custom_command (TARGET sim POST_BUILD COMMAND ${CMAKE_COMMAND} -E 
		copy "$<TARGET_FILE:xphost>" "$<TARGET_FILE_DIR:sim>/Resources/plugins/sample/win_x64/sample.xpl")
	add_custom_command (TARGET sim POST_BUILD COMMAND ${CMAKE_COMMAND} -E 
		copy "$<TARGET_FILE_DIR:xphost>/nethost.dll" "$<TARGET_FILE_DIR:sim>/Resources/plugins/sample/win_x64/")
elseif (CMAKE_SYSTEM_NAME MATCHES "Linux")
	target_link_libraries(sim "dl" "stdc++fs")
	add_custom_command (TARGET sim POST_BUILD COMMAND ${CMAKE_COMMAND} -E 
		copy "$<TARGET_FILE:xphost>" "$<TARGET_FILE_DIR:sim>/Resources/plugins/sample/lin_x64/sample.xpl")
elseif (CMAKE_SYSTEM_NAME MATCHES "Darwin")
	add_custom_command (TARGET sim POST_BUILD COMMAND ${CMAKE_COMMAND} -E 
		copy "$<TARGET_FILE:xphost>" "$<TARGET_FILE_DIR:sim>/Resources/plugins/sample/mac_x64/sample.xpl")
endif ()

add_custom_command (TARGET sim POST_BUILD COMMAND ${CMAKE_COMMAND} -E 
	copy "$<TARGET_FILE:sim_xplm>" "$<TARGET_FILE_DIR:sim>/Resources/plugins/")
