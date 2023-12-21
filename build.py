import os
import shutil
import subprocess
import platform
import argparse
import os
import requests
from pathlib import Path


def download_folder(repo_owner, repo_name, folder_path, branch='main', output_dir='.'):
    # Create the GitHub API URL for the contents of the folder
    api_url = f'https://api.github.com/repos/{repo_owner}/{repo_name}/contents/{folder_path}?ref={branch}'

    # Send a GET request to the GitHub API
    response = requests.get(api_url)

    # Check if the request was successful
    if response.status_code == 200:
        # Create the output directory if it doesn't exist
        output_path = Path(output_dir)
        output_path.mkdir(parents=True, exist_ok=True)

        # Iterate through the files in the folder and download each one
        for file_info in response.json():
            download_url = file_info['download_url']
            file_name = os.path.join(output_path, file_info['name'])

            with requests.get(download_url) as file_response:
                with open(file_name, 'wb') as file:
                    file.write(file_response.content)

        print(f"Downloaded files from '{folder_path}' to '{output_path}'")
    else:
        print(f"Failed to fetch contents. Status code: {response.status_code}")
        print(response.text)


# should not run as submodule
if __name__ != "__main__":
    exit()

parser = argparse.ArgumentParser(description='Build DirectXShaderCompiler from source with changes')
parser.add_argument('-T', '--target', dest='target', required=False, help='Specify the target platform- options are (Windows, Linux, Darwin)')
args = parser.parse_args()

build_platform = platform.system()
target_platform = args.target or build_platform
if target_platform != 'Windows' and target_platform != 'Darwin' and target_platform != 'Linux':
    print('Invalid platform: ' + target_platform)
    exit()

source_directory = os.getcwd()

# modified version of DirectXShaderCompiler
dx_repo = "https://github.com/sinnwrig/DirectX-C-API"
dia_repo = "https://github.com/milostosic/DIA.git"
deps_repo = "https://github.com/visualboyadvance-m/msvc-deps"

dx_directory = os.path.join(source_directory, 'DirectX')
dx_source_dir = os.path.join(dx_directory, 'DXCompiler')
dx_build_dir = os.path.join(dx_directory, 'DXBuild-' + target_platform)
dia_source_dir = os.path.join(dx_directory, 'DiaSDK')
deps_source_dir = os.path.join(dx_directory, 'Dependencies')


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
    exit()

# ensure Ninja installation
if not check_tool('ninja'):
    print("Ninja is required to build DXC libraries. Make sure the Ninja build system is downloaded and on the PATH")
    exit()


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

# Replace these values with your GitHub repository information
repository_owner = 'your_username'
repository_name = 'your_repository'
folder_to_download = 'path/to/folder'
branch_name = 'main'  # Replace with the branch you want to download from
output_directory = 'downloaded_files'

# Download the specified folder
download_folder(repository_owner, repository_name, folder_to_download, branch_name, output_directory)

if target_platform == 'Windows':
    git_clone_or_pull(dia_repo, dia_source_dir)
    dia_include_dir = os.path.join(dia_source_dir, 'include')
    dia_lib_path = os.path.join(dia_source_dir, 'lib' 'X64', 'diaguids.lib')

    git_clone_or_pull(deps_repo, deps_source_dir)
    deps_include_dir = os.path.join(dia_source_dir, 'include')
    deps_lib_path = os.path.join(dia_source_dir, 'lib' 'X64', 'diaguids.lib')

# create build directory
os.makedirs(dx_build_dir, exist_ok=True)

dx_predefined = os.path.join(dx_source_dir, 'cmake', 'caches', 'PredefinedParams.cmake')


c_compiler = '/usr/bin/x86_64-w64-mingw32-gcc'
cxx_compiler = '/usr/bin/x86_64-w64-mingw32-g++'
#c_flags = '-DCMAKE_C_FLAGS="-L/usr/lib/gcc/x86_64-w64-mingw32/10-win32"'
#cxx_flags = '-DCMAKE_CXX_FLAGS="-L/usr/lib/gcc/x86_64-w64-mingw32/10-win32"'

args = ['cmake']

args.append('-DCMAKE_SYSTEM_NAME=' + target_platform)

if target_platform == 'Windows':
    args.append('-DDIASDK_INCLUDE_DIR=' + dia_include_dir)
    args.append('-DCMAKE_C_COMPILER=' + c_compiler)
    args.append('-DCMAKE_CXX_COMPILER=' + cxx_compiler)
    args.append('-DDIASDK_GUIDS_LIBRARY=' + dia_lib_path)
    

args.append('-C')
args.append(dx_predefined)
args.append(dx_source_dir)
args.append('-GNinja')
args.append('-DCMAKE_BUILD_TYPE=Release')
args.append('-DENABLE_SPIRV_CODEGEN=ON')


# run Cmake with predefined settings
subprocess.run(args, cwd=dx_build_dir, check=True)

# compile with ninja
subprocess.run(['ninja', '-j4'], cwd=dx_build_dir, check=True)


# default to windows DLL name
lib_name = 'dxcompiler.dll'

if target_platform == 'Linux':
    lib_name = 'libdxcompiler.so'
elif target_platform == 'Darwin':
    # No clue if this is the correct library name on macOS (i dont have a mac)
    lib_name = 'dxcompiler.dylib'

dx_library_src = os.path.join(dx_build_dir, 'lib', lib_name)
dx_library_dest = os.path.join(dx_directory, lib_name)

# clone the DXC library into the directory C# will look for it
shutil.copyfile(dx_library_src, dx_library_dest)