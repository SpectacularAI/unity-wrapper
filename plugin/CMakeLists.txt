cmake_minimum_required(VERSION 3.21)
project(spectacularAI_unity)

if(MSVC)
  set(CMAKE_CXX_STANDARD 20)
  set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} /MP")
else()
  set(CMAKE_CXX_STANDARD 14)
  set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -Wall -Wextra -Wl,--no-as-needed")
endif()

find_package(depthai REQUIRED)
find_package(spectacularAI_depthaiPlugin REQUIRED)

set(PLUGIN_SRC
  src/replay.cpp
  src/output.cpp
  src/util.cpp
  src/depthai.cpp
  src/mapping.cpp
)

set(PLUGIN_LIBS
  depthai::core
  spectacularAI::depthaiPlugin)

if(MSVC)
  # Depthai-core needs this and cmake can't find it otherwise
  find_package(usb-1.0 REQUIRED)
  list(APPEND PLUGIN_LIBS usb-1.0)
endif()

# enables searching for dynamic libraries from the relative path ../lib
if(NOT MSVC)
    set(CMAKE_EXE_LINKER_FLAGS "${CMAKE_EXE_LINKER_FLAGS} -Wl,-rpath='$ORIGIN/../lib:$ORIGIN/../lib/3rdparty'")
endif()

add_library(${CMAKE_PROJECT_NAME} SHARED ${PLUGIN_SRC})
target_link_libraries(${CMAKE_PROJECT_NAME} PRIVATE ${PLUGIN_LIBS})

# C++ Replay example
add_executable(main_replay ${PLUGIN_SRC} examples/main_replay.cpp)
target_link_libraries(main_replay PRIVATE ${PLUGIN_LIBS})
target_include_directories(main_replay PRIVATE "include/spectacularAI/unity")

# C++ DepthAI example
add_executable(main_depthai ${PLUGIN_SRC} examples/main_depthai.cpp)
target_link_libraries(main_depthai PRIVATE ${PLUGIN_LIBS})
target_include_directories(main_depthai PRIVATE "include/spectacularAI/unity")

if(MSVC)
  add_custom_command(TARGET ${CMAKE_PROJECT_NAME} POST_BUILD
    COMMAND ${CMAKE_COMMAND} -E copy $<TARGET_RUNTIME_DLLS:${CMAKE_PROJECT_NAME}> $<TARGET_FILE_DIR:${CMAKE_PROJECT_NAME}>
    COMMAND_EXPAND_LISTS
  )
endif()
