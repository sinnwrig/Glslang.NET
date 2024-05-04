import platform


platform_aliases = [
    {
        "platform": "macos",
        "zig-build-alias": "macosx-none",
        "python-alias": "Darwin",
        "executable-ext": "",
        "compress-ext": ".tar.xz"
    },

    {
        "platform": "windows",
        "zig-build-alias": "windows-gnu",
        "python-alias": "Windows",
        "exeuctable-ext": ".exe",
        "compress-ext": ".zip"
    },

    {
        "platform": "linux",
        "zig-build-alias": "linux-gnu",
        "python-alias": "Linux",
        "executable-ext": "",
        "compress-ext": ".tar.xz"
    }
]


architecture_aliases = [
    {
        "architecture": "arm64",
        "zig-build-alias": "aarch64",
    },

    {
        "architecture": "x86_64",
        "zig-build-alias": "x86_64",
        "intel-alias": "x86_64",
        "amd-alias": "amd64",
    }
]



def get_platform_aliases(any_name):
    any_name = any_name.lower()
    return next(x for x in platform_aliases if (any_name in x.values()))

def get_architecture_aliases(any_name):
    any_name = any_name.lower()
    return next(x for x in architecture_aliases if (any_name in x.values()))

    