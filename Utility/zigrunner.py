import shutil
import subprocess


def ensure_zig():
    if shutil.which('zig') == None:
        raise Exception('Zig installation could not be found. Please download and install the Zig compiler found here: https://ziglang.org/learn/getting-started/#package-managers before running this script, and ensure it is on the system PATH.')


def quick_compile_cpp_library(directory, target_platform, src_path, dest_path, lib_path, libs):
    ensure_zig()

    zig_command = [ 
        'zig',
        'c++',
        '-target',
        target_platform,
        '-shared',
        '-fPIC',
        '-o',
        dest_path,
        src_path,
        f'-L{lib_path}',
        f'-l{libs}',
        '-lc',
        '-lc++' 
    ]

    subprocess.run(zig_command, cwd = directory, check = True)

    print(f'Compiled file {src_path}')