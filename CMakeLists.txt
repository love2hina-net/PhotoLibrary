# CMakeList.txt : 最上位の CMake プロジェクト ファイル。グローバル構成を行います
# また、サブプロジェクトをここに含めます。
#
cmake_minimum_required(VERSION 3.24)

project("PhotoLibrary")

# サブプロジェクトを含めます。
add_subdirectory("firebird")

if(CMAKE_SIZEOF_VOID_P EQUAL 8)
    # 64 bits
    set(PLATFORM "x64")
elseif(CMAKE_SIZEOF_VOID_P EQUAL 4)
    # 32 bits
    set(PLATFORM "x86")
endif()

add_custom_target(PhotoLibrary ALL
    COMMAND dotnet build PhotoLibrary.sln -p:Platform=${PLATFORM} --configuration ${CMAKE_BUILD_TYPE}
    DEPENDS
        "${CMAKE_BINARY_DIR}/firebird/exec/main/intl/fbintl.conf"
        "${CMAKE_BINARY_DIR}/firebird/exec/main/intl/fbintl.dll"
        "${CMAKE_BINARY_DIR}/firebird/exec/main/plugins/engine13.dll"
        "${CMAKE_BINARY_DIR}/firebird/exec/main/firebird.conf"
        "${CMAKE_BINARY_DIR}/firebird/exec/main/firebird.msg"
        "${CMAKE_BINARY_DIR}/firebird/exec/main/fbclient.dll"
        "${CMAKE_BINARY_DIR}/firebird/exec/main/ib_util.dll"
        "${CMAKE_BINARY_DIR}/firebird/exec/main/icudt63l.dat"
        "${CMAKE_BINARY_DIR}/firebird/exec/main/icudt63.dll"
        "${CMAKE_BINARY_DIR}/firebird/exec/main/icuin63.dll"
        "${CMAKE_BINARY_DIR}/firebird/exec/main/icuuc63.dll"
    WORKING_DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}
    VERBATIM
)
