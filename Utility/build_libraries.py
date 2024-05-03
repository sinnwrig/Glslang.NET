import sys
import io
import os
import platform
import subprocess
import argparse
import json
import gitreleases
import zigrunner
import requests


# should not run as submodule
if __name__ != "__main__":
    exit()

# -------------------------------------------------------- 
# ------------- Compilation Target Arguments ------------- 
# -------------------------------------------------------- 

parser = argparse.ArgumentParser(description = 'Build DirectX Compiler wrapper into a shared library')

parser.add_argument('-P', '--platform', dest = 'platform', required = False, help = 'Platform type- Can be [Windows, Linux, Darwin (MacOS), All]. Defaults to current platform.')
parser.add_argument('-A', '--architecture', dest = 'architecture', required = False, help = 'Platform architecture- Can be [aarch64, x86_64, All]. Defaults to current architecture.')

platform_type = parser.parse_args().platform or platform.system()
platform_arch = parser.parse_args().architecture or platform.machine()

platforms = ["Windows", "Linux", "Darwin", "All"]
architectures = ["aarch64", "x86_64", 'All']

def match_any(input, list):
    return any(input == element for element in list)

if not match_any(platform_type, platforms):
    raise Exception(f"Platform '{platform_type}' does not match available platforms. Must be Windows, Linux, Darwin, or All.")

if not match_any(platform_arch, architectures):
    raise Exception(f"Architecture '{platform_arch}' does not match available architectures. Must be aarch64 or x86_64.")

wrapper_name = 'machdxcompiler'

parent_dir = os.path.join(os.getcwd(), 'DirectX')

lib_path = os.path.join(parent_dir, 'lib')
bin_path = os.path.join(parent_dir, 'bin')

print(f"Compiling for {platform_type} on {platform_arch} architecture.")

