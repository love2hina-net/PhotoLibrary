# CMakeList.txt : 最上位の CMake プロジェクト ファイル。グローバル構成を行います
# また、サブプロジェクトをここに含めます。
#
cmake_minimum_required(VERSION 3.24)

project("PhotoLibrary")

# サブプロジェクトを含めます。
add_subdirectory("firebird")

if("${CMAKE_APPLE_SILICON_PROCESSOR}" STREQUAL "arm64")
    # Apple Silicon(aarch64)
    set(PLATFORM "arm64")
elseif(CMAKE_SIZEOF_VOID_P EQUAL 8)
    # 64 bits
    set(PLATFORM "x64")
elseif(CMAKE_SIZEOF_VOID_P EQUAL 4)
    # 32 bits
    set(PLATFORM "x86")
endif()

# 共通出力パスの設定
if(WIN32)
    cmake_path(SET CURRENT_BINARY_DIR NORMALIZE "${CMAKE_BINARY_DIR}/../win-current-$<LOWER_CASE:${CMAKE_BUILD_TYPE}>")
elseif(APPLE)
    cmake_path(SET CURRENT_BINARY_DIR NORMALIZE "${CMAKE_BINARY_DIR}/../mac-current-$<LOWER_CASE:${CMAKE_BUILD_TYPE}>")
else()
    message(FATAL_ERROR "Unsupported platform")
endif()

if(WIN32)
    add_custom_target(
        OUTPUT "${CURRENT_BINARY_DIR}"
        COMMAND mklink /d "${CURRENT_BINARY_DIR}" "${CMAKE_BINARY_DIR}"
        VERBATIM
    )
    add_custom_target(PhotoLibrary ALL
        COMMAND dotnet build PhotoLibrary.sln -p:Platform=${PLATFORM} --configuration ${CMAKE_BUILD_TYPE}
        DEPENDS
            "${CURRENT_BINARY_DIR}"
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
else()
    add_custom_target(
        OUTPUT "${CURRENT_BINARY_DIR}"
        COMMAND ln -s "${CMAKE_BINARY_DIR}" "${CURRENT_BINARY_DIR}"
        VERBATIM
    )
    add_custom_target(PhotoLibrary ALL
        COMMAND dotnet build PhotoLibrary.sln -p:Platform=${PLATFORM} --configuration ${CMAKE_BUILD_TYPE}
        DEPENDS
            "${CURRENT_BINARY_DIR}"
            "${CMAKE_BINARY_DIR}/firebird/exec/main/intl/fbintl.conf"
            "${CMAKE_BINARY_DIR}/firebird/exec/main/intl/${CMAKE_SHARED_LIBRARY_PREFIX}fbintl${CMAKE_SHARED_LIBRARY_SUFFIX}"
            "${CMAKE_BINARY_DIR}/firebird/exec/main/plugins/${CMAKE_SHARED_LIBRARY_PREFIX}engine13${CMAKE_SHARED_LIBRARY_SUFFIX}"
            "${CMAKE_BINARY_DIR}/firebird/exec/main/firebird.conf"
            "${CMAKE_BINARY_DIR}/firebird/exec/main/firebird.msg"
            "${CMAKE_BINARY_DIR}/firebird/exec/main/lib/${CMAKE_SHARED_LIBRARY_PREFIX}fbclient${CMAKE_SHARED_LIBRARY_SUFFIX}"
            "${CMAKE_BINARY_DIR}/firebird/exec/main/lib/${CMAKE_SHARED_LIBRARY_PREFIX}ib_util${CMAKE_SHARED_LIBRARY_SUFFIX}"
            # "${CMAKE_BINARY_DIR}/firebird/exec/main/lib/${CMAKE_SHARED_LIBRARY_PREFIX}icudata.73.2${CMAKE_SHARED_LIBRARY_SUFFIX}"
            # "${CMAKE_BINARY_DIR}/firebird/exec/main/lib/${CMAKE_SHARED_LIBRARY_PREFIX}icui18n.73.2${CMAKE_SHARED_LIBRARY_SUFFIX}"
            # "${CMAKE_BINARY_DIR}/firebird/exec/main/lib/${CMAKE_SHARED_LIBRARY_PREFIX}icuuc.73.2${CMAKE_SHARED_LIBRARY_SUFFIX}"
        WORKING_DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}
        VERBATIM
    )
endif()
