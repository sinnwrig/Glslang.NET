import sys
import io
import os
import platform
import subprocess
import argparse
import json
import gitreleases
import requests


# should not run as submodule
if __name__ != "__main__":
    exit()

source_directory = os.getcwd()

dest_dir = os.path.join(source_directory, 'DirectX')

platforms = ["Windows", "Linux", "Darwin", "All"]
architectures = ["aarch64", "x86_64"]


parser = argparse.ArgumentParser(description = 'Build DirectX Compiler wrapper into a shared library')
parser.add_argument('-P', '--platform', dest = 'platform', required = False, help = 'Platform type- Can be [Windows, Linux, Darwin (MacOS), All]. Defaults to current platform.')
parser.add_argument('-A', '--architecture', dest = 'architecture', required = False, help = 'Platform architecture- Can be [aarch64, x86_64]. Defaults to current architecture.')


platform_type = parser.parse_args().platform or platform.system()
platform_arch = parser.parse_args().architecture or platform.machine()


def match_any(input, list):
    return any(input == element for element in list)

if not match_any(platform_type, platforms):
    print(f"Platform '{platform_type}' does not match available platforms. Must be Windows, Linux, Darwin, or All.")
    exit()

if not match_any(platform_arch, architectures):
    print(f"Architecture '{platform_arch}' does not match available architectures. Must be aarch64 or x86_64.")
    exit()


print(f"Compiling for {platform_type} on {platform_arch} architecture.")


def select_release(release):
    name = release["name"]

    # not the right release format
    if not "ReleaseFast_lib.tar.gz" in name:
        return False

    # exclude musl binaries
    if "musl" in name:
        return False
    
    # ensure proper architecture
    if not platform_arch in name:
        return False
    
    if (platform_type == 'Windows' or platform_type == 'All') and 'windows-gnu' in name:
        return True
    
    if (platform_type == 'Linux' or platform_type == 'All') and 'linux-gnu' in name:
        return True

    if (platform_type == 'Darwin' or platform_type == 'All') and 'macos-none' in name:
        return True
    
    return False

releases = gitreleases.get_latest_release('hexops', 'mach-dxcompiler')

releases = [x for x in releases if select_release(x)]



lib_path = os.path.join(dest_dir, 'library')
os.makedirs(lib_path, exist_ok = True)

wrapper_name = 'mach_dxc'

# make a version of this that works on Windows since c++ compilers are a pain to get working on PowerShell
clang_command = [ 'clang++' ]

clang_command.append('-shared')
clang_command.append('-fPIC')
clang_command.append('-o')
clang_command.append('library/')
clang_command.append('mach_dxc.cpp')
clang_command.append('-L')
clang_command.append('-lmachdxcompiler')
clang_command.append('-lc++')


for release in releases:
    name = release["name"]
    folder_name = 'Unknown Platform'
    platform_suffix = ''

    if 'windows-gnu' in name:
        folder_name = 'Artifacts-Windows'
        platform_suffix = '.dll'
    
    if 'linux-gnu' in name:
        folder_name = 'Artifacts-Linux'
        platform_suffix = '.so'

    if 'macos-none' in name:
        folder_name = 'Artifacts-Darwin'
        platform_suffix = '.dylib'

    if 'aarch64' in name:
        folder_name += '-ARM64'
    elif 'x86_64' in name:
        folder_name += '-x86_64'

    gitreleases.download_release(release, dest_dir, folder_name, 'Compressed')

    clang_command[4] = f"library/{wrapper_name}{platform_suffix}"
    clang_command[6] = f"-L{folder_name}/"

    print("Compiling")

    subprocess.run(clang_command, cwd = dest_dir, check = True)