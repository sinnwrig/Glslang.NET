import shutil
import os
import platform
import subprocess

import utils.platformver
import utils.download

from os import path


def ensure_zig(version, download_url, install_path):
    platform_alias = utils.platformver.get_platform_aliases(platform.system())
    arch = utils.platformver.get_architecture_aliases(platform.machine())

    zig_name = f"zig-{platform_alias['platform']}-{arch['zig-build-alias']}-{version}"
    zig_path = path.join(install_path, zig_name)
    zig_exe = path.join(zig_path, f"zig{platform_alias['executable-ext']}")
    
    # Use the locally-installed version
    if path.isdir(zig_path):
        return zig_exe

    # Try finding a system version
    try:
        sys_result = subprocess.run([ 'zig', 'version'], check = True, capture_output = True, text = True)

        if sys_result.stdout is version:
            return 'zig'
    except:
        # Download a local version

        zig_pkg_name = f"{zig_name}{platform_alias['compress-ext']}"
        download_path = f"{download_url}{zig_pkg_name}"

        response, downloaded_data = utils.download.progress_download(download_path, 'Downloading zig compiler')
        utils.download.extract(downloaded_data, zig_pkg_name, install_path, zig_name, 'Compressed')

        return zig_exe





    
