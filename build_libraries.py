import sys
import io
import os
import platform
import subprocess
import argparse
import shutil

import utils.github
import utils.zigrunner
import utils.platformver

from os import path

platforms = ["windows-gnu", "linux-gnu", "macos-none"]
architectures = ["aarch64", "x86_64"]

def match_any(input, list):
    return any(input == element for element in list)

# NOTE: Zig path is relative to current working directory.
# NOTE: The source path must contain a build.zig file, and the output path is relative to source path. 
def build(zig_path, src_path, output_path, is_shared, is_spirv, architecture, platform, cpu_specific = None, clear_cache = True, debug_symbols = False):
    zig_cache_dir = path.join(src_path, 'zig-cache')

    if clear_cache and path.isdir(zig_cache_dir):
        shutil.rmtree(zig_cache_dir)

    if not match_any(platform, platforms):
        raise Exception(f"Invalid platform '{platform}'. Must be one of the following: {platforms}.") 

    if not match_any(architecture, architectures):
        raise Exception(f"Invalid architecture '{platform}'. Must be one of the following: {architectures}.")

    print(f"Compiling for {architecture}-{platform}. SPIR-V support: {is_spirv}. Shared Library: {is_shared}. Output directory: {output_path}")

    zig_cmd = [
        zig_path, 'build',
        '-p', output_path,
        '-Dshared', '-Dspirv',
        '-Doptimize=ReleaseFast',
        '-Dfrom_source',
        '-Dskip_executables',
        f'-Dtarget={architecture}-{platform}'
    ]

    if debug_symbols:
        zig_cmd.append("-Ddebug_symbols")

    if cpu_specific is not None:
        zig_cmd.append(f'-Dcpu={cpu_specific}')
    
    subprocess.run(zig_cmd, cwd = src_path, check = True)


# should not run as submodule
if __name__ != "__main__":
    exit()

cwd = os.getcwd()

def full_path(relative_path):
    return path.join(cwd, relative_path)

dxc = 'DXC'
dxc_zig = path.join(dxc, 'zig-installs')
dxc_src = path.join(dxc, 'source')
dxc_lib = path.join(dxc, 'lib')
ver = '0.12.0-dev.3180+83e578a18'

if len(os.listdir(full_path(dxc_src))) == 0:
    print("Dxcompiler source directory is empty! Please ensure this repository was cloned with `--recurse-submodules`, or run `git submodule update --init --recursive`.")

zig_path = utils.zigrunner.ensure_zig(ver, 'https://pkg.machengine.org/zig/', install_path = full_path(dxc_zig))

parser = argparse.ArgumentParser(prog = 'Build DirectXCompiler', description = 'Build and compile DXC libraries')
parser.add_argument('-P', '--platform', nargs = '+', dest = 'platform', required = False)
parser.add_argument('-A', '--architecture', nargs = '+', dest = 'architecture', required = False)
parser.add_argument('-D', '--debug', dest = 'debug', required = False)

args = parser.parse_args()

platform_args = args.platform or [ platform.system() ]
arch_args = args.architecture or [ platform.machine() ]
debug_symbols = (args.debug or 'Off') == 'On'

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

ensure(platform_aliases, platform_args)

if len(arch_args) == 1 and str(arch_args[0]).lower() == 'all':
    arch_aliases = utils.platformver.architecture_aliases
else:
    arch_aliases = [ utils.platformver.get_architecture_alias(x) for x in arch_args ]

ensure(arch_aliases, arch_args)

print('Building:')

for p_alias in platform_aliases:
    for a_alias in arch_aliases:
        print(f"\t {p_alias['platform']}-{a_alias['architecture']}")

for p_alias in platform_aliases:
    for a_alias in arch_aliases:
        pname = p_alias['zig-build-alias']
        aname = a_alias['zig-build-alias']

        output_path = full_path(path.join(dxc_lib, f"{p_alias['platform']}-{a_alias['architecture']}"))
        
        build(zig_path, full_path(dxc_src), output_path, True, True, aname, pname, clear_cache = True)