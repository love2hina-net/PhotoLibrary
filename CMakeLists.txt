﻿# CMakeList.txt : 最上位の CMake プロジェクト ファイル。グローバル構成を行います
# また、サブプロジェクトをここに含めます。
#
cmake_minimum_required(VERSION 3.24)

project("PhotoViewer")

# サブプロジェクトを含めます。
add_subdirectory("firebird")