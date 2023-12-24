import shutil
import sys
import requests
import zipfile
import tarfile
import io
import os
import platform
import subprocess
import argparse



# should not run as submodule
if __name__ != "__main__":
    exit()

def download_file(url):
    response = requests.get(url, stream=True)

    total_size = int(response.headers.get('content-length', 0))
    block_size = 1024 * 8  # Adjust the block size as needed

    downloaded_data = b""
    
    downloaded_size = 0
    for data in response.iter_content(block_size):
        downloaded_size += len(data)
        downloaded_data += data

        progress = (downloaded_size / total_size) * 100
        sys.stdout.write("\rDownloading: [{:<50}] {:.2f}%".format('=' * int(progress / 2), progress))
        sys.stdout.flush()

    return response, downloaded_data


def extract_temp(compressed_file, destination_directory, extract_name):
    # Extract the contents to a temporary directory
    temp_extract_path = os.path.join(destination_directory, 'temp')
    compressed_file.extractall(temp_extract_path)

    # Rename the extracted folder to the desired name
    new_folder_path = os.path.join(destination_directory, extract_name)
    shutil.move(temp_extract_path, new_folder_path)

    # Clean up the temporary directory
    os.rmdir(temp_extract_path)


def download_zip(url, destination_directory, new_name):
    # Make a request to get the content
    response, data = download_file(url)
    
    # Check if the request was successful (status code 200)
    if response.status_code == 200:
        # Create a zip file object from the content
        with zipfile.ZipFile(io.BytesIO(data)) as zip_file:
            extract_temp(zip_file, destination_directory, new_name)
        print("Download and extraction successful.")
    else:
        print(f"Failed to download file. Status code: {response.status_code}")


def download_tar_gz(url, destination_directory, new_name):
    # Make a request to get the content
    response, data = download_file(url)
    
    # Check if the request was successful (status code 200)
    if response.status_code == 200:
        # Create a tar file object from the content
        with tarfile.open(fileobj=io.BytesIO(data), mode="r:gz") as tar_file:
            extract_temp(tar_file, destination_directory, new_name)
        print("Download and extraction successful.")
    else:
        print(f"Failed to download file. Status code: {response.status_code}")


parser = argparse.ArgumentParser(description='Build DirectX Compiler wrapper into a shared library')
parser.add_argument('-P', '--platform', dest='platform', required=False, help='Platform type- Can be (Windows, Linux)')

platform_type = parser.parse_args().platform or platform.system()

print('Compiling for platform: ' + platform_type)


dxc_win_build = "https://ci.appveyor.com/api/projects/dnovillo/directxshadercompiler/artifacts/build%2FRelease%2Fdxc-artifacts.zip?branch=main&pr=false&job=image%3A%20Visual%20Studio%202022"
dxc_lin_build = "https://ci.appveyor.com/api/projects/dnovillo/directxshadercompiler/artifacts/build%2Fdxc-artifacts.tar.gz?branch=main&pr=false&job=image%3A%20Ubuntu"


source_directory = os.getcwd()

parent_dir = os.path.join(source_directory, 'DirectX')
parent_lib_path = os.path.join(parent_dir, 'library')

dest_name = 'Artifacts-' + platform_type

dest_path = os.path.join(parent_dir, dest_name)
dest_include_path = os.path.join(dest_path, 'include')
dest_lib_path = os.path.join(dest_path, 'lib')

if platform_type == 'Windows':
    dxc_lib = 'dxcompiler.dll'
    wrapper_name = 'dxcwrapper.dll'
elif platform_type == 'Linux':
    dxc_lib = 'libdxcompiler.so'
    wrapper_name = 'libdxcwrapper.so'


if not os.path.exists(parent_dir):
    print('Build directory \'' + parent_dir  + '\' does not exist')
    exit()

if not os.path.exists(dest_path):
    if platform_type == 'Windows':
        download_zip(dxc_win_build, parent_dir, dest_name)
    elif platform_type == 'Linux':
        download_tar_gz(dxc_lin_build, parent_dir, dest_name)
    else:
        print("Invalid platform: " + platform_type)


# create the output library path
os.makedirs(parent_lib_path, exist_ok=True)

# make a version of this that works on Windows since c++ compilers are a pain to get working on PowerShell
clang_command = [ 'clang++' ]

if (platform_type == 'Windows'):
    clang_command.append('-target')
    clang_command.append('x86_64-w64-mingw32')
    clang_command.append('-D')
    clang_command.append('_WIN32')

clang_command.append('-shared')
clang_command.append('-o')
clang_command.append('library/' + wrapper_name)
clang_command.append('-fPIC')
clang_command.append('dxcwrapper.cpp')
clang_command.append('-I' + dest_include_path)
clang_command.append('-L' + dest_lib_path)
clang_command.append('-ldxcompiler')
clang_command.append('-Wl,-rpath=$ORIGIN')

subprocess.run(clang_command, cwd=parent_dir, check=True)

shutil.copy2(os.path.join(dest_lib_path, dxc_lib), os.path.join(parent_lib_path, dxc_lib))