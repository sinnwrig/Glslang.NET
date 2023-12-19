import os
import shutil
import subprocess
import platform


# should not run as submodule
if __name__ != "__main__":
    exit()


def check_tool(tool_name):
    try:
        # Try running the tool with the "--version" flag to check if it's installed
        subprocess.run([tool_name, '--version'], stdout=subprocess.PIPE, stderr=subprocess.PIPE, check=True)
        return True
    except subprocess.CalledProcessError:
        return False 

# ensure CMake installation
if not check_tool('cmake'):
    print("CMake is required to build DXC libraries. Make sure the CMake build system is downloaded and on the PATH")

# ensure Ninja installation
if not check_tool('ninja'):
    print("Ninja is required to build DXC libraries. Make sure the Ninja build system is downloaded and on the PATH")


source_directory = os.getcwd()
platform_type = platform.system()

dx_repo = "https://github.com/microsoft/DirectXShaderCompiler.git"
dx_directory = os.path.join(source_directory, 'DirectX')
dx_source_dir = os.path.join(dx_directory, 'DXCompiler')
dx_build_dir = os.path.join(dx_directory, 'DXBuild')


# no project directory means a problem
if not os.path.exists(dx_directory):
    raise FileNotFoundError(f"DirectX Source directory: '{dx_directory}' does not exist.")


def git_clone_or_pull(repo_url, destination_dir):
    # check if the destination directory is nonexistent or empty
    is_empty = not os.path.exists(destination_dir) or not any(os.scandir(destination_dir))

    if is_empty:
        # clone repo if the directory is empty
        subprocess.run(['git', 'clone', '--recursive', repo_url, destination_dir])
    else:
        # update directory if not empty
        subprocess.run(['git', 'pull'], cwd=destination_dir)

# clone original dxc repository
git_clone_or_pull(dx_repo, dx_source_dir)

# create build directory
os.makedirs(dx_build_dir, exist_ok=True)


src_dxcapi = os.path.join(dx_directory, 'dxcapi-new.cpp')
dest_dxcapi = os.path.join(dx_source_dir, 'tools', 'clang', 'tools', 'dxcompiler', 'dxcapi.cpp')

# overwrite dxcapi.cpp with our fancy new version that has a C API (wow)
shutil.copy2(src_dxcapi, dest_dxcapi)


dx_predefined = os.path.join(dx_source_dir, 'cmake', 'caches', 'PredefinedParams.cmake')

# run Cmake with predefined settings
subprocess.run(['cmake', '-C', dx_predefined, dx_source_dir, '-GNinja', '-DCMAKE_BUILD_TYPE=Release', '-DENABLE_SPIRV_CODEGEN=ON'], cwd=dx_build_dir, check=True)

# compile with ninja
subprocess.run(['ninja', '-j4'], cwd=dx_build_dir, check=True)


# default to windows DLL name
lib_name = 'dxcompiler.dll'

if platform_type == 'Linux':
    lib_name = 'libdxcompiler.so'
elif platform_type == 'Darwin':
    # No clue if this is the correct library name on macOS (i dont have a mac)
    lib_name = 'dxcompiler.dylib'

dx_library_src = os.path.join(dx_build_dir, 'lib', lib_name)
dx_library_dest = os.path.join(dx_directory, lib_name)

# clone the DXC library into the directory C# will look for it
shutil.copyfile(dx_library_src, dx_library_dest)