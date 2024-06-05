import sys
import io
import os
import platform
import subprocess
import argparse
import shutil

import utils.platformver

from os import path

platforms = utils.platformver.platforms('zig-build-alias')
architectures = utils.platformver.architectures('zig-build-alias')

def match_any(input, list):
    return any(input == element for element in list)

def build(src_path, output_path, is_shared, architecture, platform, cpu_specific = None, clear_cache = True, debug_symbols = False):
    zig_cache_dir = path.join(src_path, 'zig-cache')

    if clear_cache and path.isdir(zig_cache_dir):
        shutil.rmtree(zig_cache_dir)

    if not match_any(platform, platforms):
        raise Exception(f"Invalid platform '{platform}'. Must be one of the following: {platforms}.") 

    if not match_any(architecture, architectures):
        raise Exception(f"Invalid architecture '{platform}'. Must be one of the following: {architectures}.")

    print(f"Compiling for {architecture}-{platform}. Shared Library: {is_shared}. Output directory: {output_path}")

    zig_cmd = [
        'zig', 'build',
        '-p', output_path,
        '-Dshared', '-Doptimize=ReleaseFast',
        f'-Dtarget={architecture}-{platform}'
    ]

    if debug_symbols:
        zig_cmd.append("-Ddebug")

    if cpu_specific is not None:
        zig_cmd.append(f'-Dcpu={cpu_specific}')
    
    subprocess.run(zig_cmd, cwd = src_path, check = True)


# should not run as submodule
if __name__ != "__main__":
    exit()

cwd = os.getcwd()

def full_path(relative_path):
    return path.join(cwd, relative_path)

glslang = 'glslang'
glslang_src = path.join(glslang, 'source')
glslang_lib = path.join(glslang, 'lib')

version = 'any'

# Ensure a copy of glslang exists
if len(os.listdir(full_path(glslang_src))) == 0:
    print("glslang source directory is empty! Please ensure this repository was cloned with `--recurse-submodules`, or run `git submodule update --init --recursive`.")

# Ensure the correct zig version (if any) is installed on the system
try: 
    sys_result = subprocess.run([ 'zig', 'version'], check = True, capture_output = True, text = True)
    res_strip = sys_result.stdout.rstrip()

    if res_strip != version and version != 'any':
        print(f"Invalid zig version: {res_strip}. Please install zig version {version}")
        exit(1)
except subprocess.CalledProcessError:
    print("Could not run `zig version`. Is zig installed on the system?")
    exit(1)

# ---------------
# Parse arguments
# ---------------

parser = argparse.ArgumentParser(prog = 'Build glslang', description = 'Build and compile glslang libraries')
parser.add_argument('-P', '--platform', nargs = '+', dest = 'platform', required = False)
parser.add_argument('-A', '--architecture', nargs = '+', dest = 'architecture', required = False)
parser.add_argument('-D', '--debug', dest = 'debug', required = False)

args = parser.parse_args()

platform_args = args.platform or [ platform.system() ]
arch_args = args.architecture or [ platform.machine() ]
debug_symbols = (args.debug or 'off').lower() == 'on'


def ensure(array, options):
    if array == None or len(array) == 0 or array[0] == None:
        print(f"Could not determine value for option(s): {options}")
        exit(1)

platform_aliases = None
arch_aliases = None 

if len(platform_args) == 1 and str(platform_args[0]).lower() == 'all':
    platform_aliases = utils.platformver.platform_aliases
else:
    platform_aliases = [ utils.platformver.get_platform_alias(x) for x in platform_args ]

# Make sure we have at least 1 target platform
ensure(platform_aliases, platform_args)

if len(arch_args) == 1 and str(arch_args[0]).lower() == 'all':
    arch_aliases = utils.platformver.architecture_aliases
else:
    arch_aliases = [ utils.platformver.get_architecture_alias(x) for x in arch_args ]

# Make sure we have at least 1 target architecture
ensure(arch_aliases, arch_args)

# -------------
# Build sources
# -------------

print('Updating glslang sources:')

subprocess.run([ 'python3', './update_glslang_sources.py', '--site', 'zig' ], cwd = full_path('glslang/source'), check = True)

print('Building:')

for p_alias in platform_aliases:
    for a_alias in arch_aliases:
        print(f"\t {p_alias['platform']}-{a_alias['architecture']}")

for p_alias in platform_aliases:
    for a_alias in arch_aliases:
        pname = p_alias['zig-build-alias']
        aname = a_alias['zig-build-alias']

        output_path = full_path(path.join(glslang_lib, f"{p_alias['platform']}-{a_alias['architecture']}"))
        
        build(full_path(glslang_src), output_path, True, aname, pname)