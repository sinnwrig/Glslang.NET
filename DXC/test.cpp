#include "source/src/mach_dxc.h"
#include <iostream>
#include <string>


char* includeFile(void* ctx, const wchar_t* filename)
{
    return strdup(u8"Lmao error dumbass");
}


int main(void) 
{ 
    MachDxcCompiler comp = machDxcInit(); 

    std::string utf8String = R"(
#include "dumbassfile.hlsl"

struct VertexInput
{
    float2 Position : POSITION;
    float4 Color : COLOR0;
};

struct VertexOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
};


VertexOutput vertex(VertexInput input)
{
    VertexOutput output;
    output.Position = float4(input.Position, 0, 1);
    output.Color = input.Color;
    return output;
}


float4 pixel(VertexOutput input) : SV_Target
{
    return input.Color;
}
)";

    const char* utf8Strings[] = {
        u8"-encoding", u8"utf8", u8"-E", u8"vertex", u8"-T", u8"vs_6_0", u8"-Zi", u8"-spirv"
    } ;

    MachDxcCompileResult result = machDxcCompile(comp, utf8String.c_str(), utf8String.size(), utf8Strings, 7, includeFile, nullptr);

    MachDxcCompileError error = machDxcCompileResultGetError(result);

    if (error != nullptr)
    {
        std::cout << machDxcCompileErrorGetString(error) << std::endl;
        machDxcCompileErrorDeinit(error);
        machDxcDeinit(comp); 
        return 1;
    }

    std::cout << "success frfr!" << std::endl;

    machDxcDeinit(comp); 

    return 0;
}

