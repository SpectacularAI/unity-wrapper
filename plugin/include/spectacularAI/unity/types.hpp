#pragma once

/** Identifies functions and methods that are a part of the API */
#ifdef _MSC_VER
    #define EXPORT_API __declspec(dllexport)
#else
    #define EXPORT_API __attribute__((visibility("default")))
#endif

#include <spectacularAI/types.hpp>

// Keeps std::shared_ptr alive
template<typename T>
struct Wrapper {
    Wrapper(std::shared_ptr<T> handle) : _handle(handle) {}
    std::shared_ptr<T> getHandle() const { return _handle; }

private:
    std::shared_ptr<T> _handle;
};

/**
 * Wrap matrices, because internally they use std::array, which gives
 * 'C linkage function cannot return C++ class 'std::array<std::array<double,4>,4>'
 */
struct Matrix3dWrapper {
    double m00, m01, m02;
    double m10, m11, m12;
    double m20, m21, m22;
};

struct Matrix4dWrapper {
    double m00, m01, m02, m03;
    double m10, m11, m12, m13;
    double m20, m21, m22, m23;
    double m30, m31, m32, m33;
};
