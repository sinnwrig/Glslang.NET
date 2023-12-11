#!/bin/bash


if [ "$1" = "test" ]; then
    clang++ -o testApp -fPIC glslang.cpp -I../glslang/include -L../glslang/lib -lglslangmerged
else
    clang++ -shared -o glslang.so -fPIC glslang.cpp -I../glslang/include -L../glslang/lib -lglslangmerged
fi
