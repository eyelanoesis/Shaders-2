add_library(SketchbookReveal SHARED
    SketchbookReveal.cpp
    SketchbookReveal.h
)

target_include_directories(SketchbookReveal PRIVATE
    ${CMAKE_SOURCE_DIR}/dependencies/ffgl/source
)

target_link_libraries(SketchbookReveal
    OpenGL::GL
)

# Output naming
set_target_properties(SketchbookReveal PROPERTIES
    PREFIX ""
    OUTPUT_NAME "SketchbookReveal"
)

# Platform-specific settings
if(WIN32)
    set_target_properties(SketchbookReveal PROPERTIES SUFFIX ".dll")
elseif(APPLE)
    set_target_properties(SketchbookReveal PROPERTIES
        SUFFIX ".bundle"
        BUNDLE TRUE
        MACOSX_BUNDLE_INFO_PLIST ${CMAKE_CURRENT_SOURCE_DIR}/Info.plist
    )
endif()
