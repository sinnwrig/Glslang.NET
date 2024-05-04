import shutil
import sys
import requests
import io
import os
import subprocess

import utils.download

# download a github repo release and try unzipping it to the destination directory under a new name
def download_release(release, destination_directory, new_name, compression_type):
    response, downloaded_data = download.progress_download(release["browser_download_url"], f"Downloading {release['name']}")
    
    if response.status_code != 200:
        print(f"Failed to download file. Status code: {response.status_code}")

    if new_name is None:
        new_name = release["name"]

    download.extract(downloaded_data, release["name"], destination_directory, new_name, compression_type)


def download_file(owner, repo, branch, path, destination):
    response, downloaded_data = download.progress_download(f"https://raw.githubusercontent.com/{owner}/{repo}/{branch}/{path}", f"Downloading file {os.path.basename(os.path.normpath(path))}")

    if response.status_code != 200:
        print(f"Failed to download file. Status code: {response.status_code}")

    with open(destination, 'wb') as file:
        file.write(downloaded_data)


# get latest github release for a repository
def get_latest_release(owner, repo):
    response = requests.get(f"https://api.github.com/repos/{owner}/{repo}/releases/latest")

    if (response.status_code != 200):
        return None

    data = response.json()
    assets = data.get("assets")
    if assets:
        return assets
        

def clone_repo(owner, repo, dest_folder = None, branch_name = None, update_submodules = True, cwd = None, pull_if_exists = True):

    # pull and update repo if destination directory already has a .git file
    if pull_if_exists:
        dest_path = os.path.join(cwd or os.getcwd(), dest_folder)
        dest_git = os.path.join(dest_path, '.git')

        if os.path.isdir(dest_git):
            subprocess.run([ 'git', 'pull' ], cwd = os.path.join(cwd or os.getcwd(), dest_path), check = True)
            return

    git_command = [ 'git', 'clone' ]

    if update_submodules:
        git_command.extend([ '--recurse-submodules', '-j8' ])
    
    if branch_name is not None:
        git_command.extend([ '-b', branch_name ])

    git_command.append(f'https://github.com/{owner}/{repo}.git')

    if dest_folder is not None:
        git_command.append(dest_folder)

    subprocess.run(git_command, cwd = cwd or os.getcwd(), check = True)
