import shutil
import sys
import requests
import io
import os

# take some bytes, save it to a file, and try unziping it
def extract(data, old_name, destination, name, compression_type):
    new_folder_path = os.path.join(destination, name)
    temp_source = os.path.join(destination, old_name)

    with open(temp_source, 'wb') as file:
        file.write(data)

    if compression_type == 'Any' or compression_type == 'Compressed':
        try: 
            shutil.unpack_archive(temp_source, new_folder_path)
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


# download a github repo release and try unzipping it to the destination directory under a new name
def download_release(release, destination_directory, new_name, compression_type):
    response = requests.get(release["browser_download_url"], stream = True)

    total_size = int(response.headers.get('content-length', 0))
    block_size = 1024 * 8

    downloaded_data = b""
    downloaded_size = 0

    for data in response.iter_content(block_size):
        downloaded_size += len(data)
        downloaded_data += data

        progress = (downloaded_size / total_size) * 100
        sys.stdout.write("\rDownloading " + release["name"] + ": [{:<50}] {:.2f}%".format('=' * int(progress / 2), progress))
        sys.stdout.flush()

    print('')
    
    if response.status_code != 200:
        print(f"Failed to download file. Status code: {response.status_code}")

    if new_name is None:
        new_name = release["name"]

    extract(downloaded_data, release["name"], destination_directory, new_name, compression_type)



# get latest github release for a repository
def get_latest_release(owner, repo):
    response = requests.get(f"https://api.github.com/repos/{owner}/{repo}/releases/latest")

    if (response.status_code != 200):
        return None

    data = response.json()
    assets = data.get("assets")
    if assets:
        return assets
