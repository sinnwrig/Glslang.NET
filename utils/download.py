import shutil
import sys
import requests
import io
import os
import subprocess

# take some bytes, save it to a file, and try unziping it
def extract(data, old_name, destination, name, compression_type):
    new_folder_path = os.path.join(destination, name)
    temp_source = os.path.join(destination, old_name)

    with open(temp_source, 'wb') as file:
        file.write(data)

    if compression_type == 'Any' or compression_type == 'Compressed':
        try: 
            shutil.unpack_archive(temp_source, destination)
            os.remove(temp_source)
            print("Extracted file")
        except:
            if compression_type != 'Any':
                print(f"Could not extract file.")
                os.remove(temp_source)
                raise
    
    if compression_type == 'Raw' or compression_type == 'Any':
        print("Copied raw file")
        os.rename(temp_source, new_folder_path)


def progress_download(url, download_label):
    response = requests.get(url, stream = True)

    total_size = int(response.headers.get('content-length', 0))
    block_size = 1024 * 8

    downloaded_data = b""
    downloaded_size = 0

    for data in response.iter_content(block_size):
        downloaded_size += len(data)
        downloaded_data += data

        progress = (downloaded_size / total_size) * 100
        sys.stdout.write(f"\r{download_label}" + ": [{:<50}] {:.2f}%".format('=' * int(progress / 2), progress))
        sys.stdout.flush()

    print('')

    return response, downloaded_data