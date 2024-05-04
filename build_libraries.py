import sys
import io
import os
import platform
import subprocess
import argparse

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
def build(zig_path, src_path, output_path, is_shared, is_spirv, architecture, platform, cpu_specific = None):
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
dxc_src = path.join(dxc, 'source')
dxc_lib = path.join(dxc, 'lib')
ver = '0.12.0-dev.3180+83e578a18'

zig_path = utils.zigrunner.ensure_zig(ver, 'https://pkg.machengine.org/zig/', install_path = full_path(dxc))

print('Fetching source')

utils.github.clone_repo('sinnwrig', 'mach-dxcompiler', dxc_src, 'main')

platform_alias = utils.platformver.get_platform_aliases(platform.system())
arch = utils.platformver.get_architecture_aliases(platform.machine())

compile_platforms = platforms = [ "macos-none" ]
compile_architectures = ["aarch64", "x86_64"]

for platform in compile_platforms:
    for architecture in compile_architectures:
        output_path = full_path(path.join(dxc_lib, f'{platform}-{architecture}'))
        
        build(zig_path, full_path(dxc_src), output_path, True, True, architecture, platform)