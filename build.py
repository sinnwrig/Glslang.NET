import shutil
import requests
import zipfile
import tarfile
import io
import os
import platform
import subprocess



# should not run as submodule
if __name__ != "__main__":
    exit()


def extract_temp(compressed_file, destination_directory, extract_name):
    # Extract the contents to a temporary directory
    temp_extract_path = os.path.join(destination_directory, 'temp')
    compressed_file.extractall(temp_extract_path)

    # Rename the extracted folder to the desired name
    extracted_folder_path = os.path.join(temp_extract_path, os.listdir(temp_extract_path)[0])
    new_folder_path = os.path.join(destination_directory, extract_name)
    shutil.move(extracted_folder_path, new_folder_path)

    # Clean up the temporary directory
    os.rmdir(temp_extract_path)


def download_zip(url, destination_directory, new_name):
    # Make a request to get the content
    response = requests.get(url)
    
    # Check if the request was successful (status code 200)
    if response.status_code == 200:
        # Create a zip file object from the content
        with zipfile.ZipFile(io.BytesIO(response.content)) as zip_file:
            extract_temp(zip_file, destination_directory, new_name)
        print("Download and extraction successful.")
    else:
        print(f"Failed to download file. Status code: {response.status_code}")


def download_tar_gz(url, destination_directory, new_name):
    # Make a request to get the content
    response = requests.get(url)
    
    # Check if the request was successful (status code 200)
    if response.status_code == 200:
        # Create a tar file object from the content
        with tarfile.open(fileobj=io.BytesIO(response.content), mode="r:gz") as tar_file:
            extract_temp(tar_file, destination_directory, new_name)
        print("Download and extraction successful.")
    else:
        print(f"Failed to download file. Status code: {response.status_code}")


dxc_win_build = "https://ci.appveyor.com/api/projects/dnovillo/directxshadercompiler/artifacts/build%2FRelease%2Fdxc-artifacts.zip?branch=main&pr=false&job=image%3A%20Visual%20Studio%202022"
dxc_lin_build = "https://ci.appveyor.com/api/projects/dnovillo/directxshadercompiler/artifacts/build%2Fdxc-artifacts.tar.gz?branch=main&pr=false&job=image%3A%20Ubuntu"


source_directory = os.getcwd()
platform_type = platform.system()

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

clang_command = [
    'clang++',
    '-shared',
    '-o',
    'library/' + wrapper_name,
    '-fPIC',
    'dxcwrapper.cpp',
    '-I' + dest_include_path,
    '-L' + dest_lib_path,
    '-ldxcompiler',
    '-Wl,-rpath=$ORIGIN'
]

subprocess.run(clang_command, cwd=parent_dir, check=True)

shutil.copy2(os.path.join(dest_lib_path, dxc_lib), os.path.join(parent_lib_path, dxc_lib))