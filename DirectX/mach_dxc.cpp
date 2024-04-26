#define MACH_DXC_C_SHARED_LIBRARY
#define MACH_DXC_C_IMPLEMENTATION

#include <iostream>
#include <string>
#include <vector>
#include "mach_dxc.h"

extern "C" 
{

// For a reason I do not know, I MUST at least use mach_dxc functions otherwise the compiler will not include the shared object.
// This might be because of some optimizations, but I do not know for sure. Either way, putting in a small function that uses machDxcInit fixes it.
int TestCompile() 
{
    MachDxcCompiler compiler = machDxcInit();

    machDxcDeinit(compiler);

    return 0;
}

}