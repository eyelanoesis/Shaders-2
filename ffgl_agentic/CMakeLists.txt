cmake_minimum_required(VERSION 3.15)
project(FFGL_Agentic VERSION 1.0)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

# FFGL SDK (add as submodule: git submodule add https://github.com/resolume/ffgl.git dependencies/ffgl)
# For now, assume user has FFGL SDK available
# add_subdirectory(dependencies/ffgl)

# Find OpenGL
find_package(OpenGL REQUIRED)

# Plugin sources
add_subdirectory(source/plugins/SketchbookReveal)
