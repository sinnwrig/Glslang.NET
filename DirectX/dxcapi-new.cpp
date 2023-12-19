///////////////////////////////////////////////////////////////////////////////
//                                                                           //
// dxcapi.cpp                                                                //
// Copyright (C) Microsoft Corporation. All rights reserved.                 //
// This file is distributed under the University of Illinois Open Source     //
// License. See LICENSE.TXT for details.                                     //
//                                                                           //
// Implements the DxcCreateInstance function for the DirectX Compiler.       //
//                                                                           //
///////////////////////////////////////////////////////////////////////////////

#pragma region CXXAPI

#include "dxc/Support/WinIncludes.h"

#ifdef _WIN32
#define DXC_API_IMPORT __declspec(dllexport)
#else
#define DXC_API_IMPORT __attribute__((visibility("default")))
#endif

#include "dxc/Support/Global.h"
#include "dxc/Support/FileIOHelper.h"
#include "dxc/config.h"
#include "dxc/dxcisense.h"
#include "dxc/dxctools.h"
#ifdef _WIN32
#include "dxcetw.h"
#endif
#include "dxc/DxilContainer/DxcContainerBuilder.h"
#include "dxillib.h"
#include <memory>

HRESULT CreateDxcCompiler(REFIID riid, _Out_ LPVOID *ppv);
HRESULT CreateDxcDiaDataSource(REFIID riid, _Out_ LPVOID *ppv);
HRESULT CreateDxcIntelliSense(REFIID riid, _Out_ LPVOID *ppv);
HRESULT CreateDxcCompilerArgs(REFIID riid, _Out_ LPVOID *ppv);
HRESULT CreateDxcUtils(REFIID riid, _Out_ LPVOID *ppv);
HRESULT CreateDxcRewriter(REFIID riid, _Out_ LPVOID *ppv);
HRESULT CreateDxcValidator(REFIID riid, _Out_ LPVOID *ppv);
HRESULT CreateDxcAssembler(REFIID riid, _Out_ LPVOID *ppv);
HRESULT CreateDxcOptimizer(REFIID riid, _Out_ LPVOID *ppv);
HRESULT CreateDxcContainerBuilder(REFIID riid, _Out_ LPVOID *ppv);
HRESULT CreateDxcLinker(REFIID riid, _Out_ LPVOID *ppv);
HRESULT CreateDxcPdbUtils(REFIID riid, _Out_ LPVOID *ppv);

namespace hlsl {
void CreateDxcContainerReflection(IDxcContainerReflection **ppResult);
void CreateDxcLinker(IDxcContainerReflection **ppResult);
} // namespace hlsl

HRESULT CreateDxcContainerReflection(REFIID riid, _Out_ LPVOID *ppv) {
  	try {
    	CComPtr<IDxcContainerReflection> pReflection;
    	hlsl::CreateDxcContainerReflection(&pReflection);
    	return pReflection->QueryInterface(riid, ppv);
  	} catch (const std::bad_alloc &) {
    	return E_OUTOFMEMORY;
  	}
}

HRESULT CreateDxcContainerBuilder(REFIID riid, _Out_ LPVOID *ppv) {
  	// Call dxil.dll's containerbuilder
  	*ppv = nullptr;
  	const char *warning;
  	HRESULT hr = DxilLibCreateInstance(CLSID_DxcContainerBuilder,
  	                                   (IDxcContainerBuilder **)ppv);
  	if (FAILED(hr)) {
  	  	warning = "Unable to create container builder from dxil.dll. Resulting "
  	            "container will not be signed.\n";
  	} else {
  	  	return hr;
  	}

  	CComPtr<DxcContainerBuilder> Result =
  	    DxcContainerBuilder::Alloc(DxcGetThreadMallocNoRef());
  	IFROOM(Result.p);
  	Result->Init(warning);
  	return Result->QueryInterface(riid, ppv);
}

static HRESULT ThreadMallocDxcCreateInstance(REFCLSID rclsid, REFIID riid, _Out_ LPVOID *ppv) {
  	HRESULT hr = S_OK;
  	*ppv = nullptr;
  	if (IsEqualCLSID(rclsid, CLSID_DxcCompiler)) {
  	  	hr = CreateDxcCompiler(riid, ppv);
  	} else if (IsEqualCLSID(rclsid, CLSID_DxcCompilerArgs)) {
  	  	hr = CreateDxcCompilerArgs(riid, ppv);
  	} else if (IsEqualCLSID(rclsid, CLSID_DxcUtils)) {
  	  	hr = CreateDxcUtils(riid, ppv);
  	} else if (IsEqualCLSID(rclsid, CLSID_DxcValidator)) {
  	  	if (DxilLibIsEnabled()) {
  	    	hr = DxilLibCreateInstance(rclsid, riid, (IUnknown **)ppv);
  	  	} else {
  	    	hr = CreateDxcValidator(riid, ppv);
  	  	}
  	} else if (IsEqualCLSID(rclsid, CLSID_DxcAssembler)) {
  	  	hr = CreateDxcAssembler(riid, ppv);
  	} else if (IsEqualCLSID(rclsid, CLSID_DxcOptimizer)) {
  	  	hr = CreateDxcOptimizer(riid, ppv);
  	} else if (IsEqualCLSID(rclsid, CLSID_DxcIntelliSense)) {
  	  	hr = CreateDxcIntelliSense(riid, ppv);
  	} else if (IsEqualCLSID(rclsid, CLSID_DxcContainerBuilder)) {
  	  	hr = CreateDxcContainerBuilder(riid, ppv);
  	} else if (IsEqualCLSID(rclsid, CLSID_DxcContainerReflection)) {
  	  	hr = CreateDxcContainerReflection(riid, ppv);
  	} else if (IsEqualCLSID(rclsid, CLSID_DxcPdbUtils)) {
  	  	hr = CreateDxcPdbUtils(riid, ppv);
  	} else if (IsEqualCLSID(rclsid, CLSID_DxcRewriter)) {
  	  	hr = CreateDxcRewriter(riid, ppv);
  	} else if (IsEqualCLSID(rclsid, CLSID_DxcLinker)) {
  	  	hr = CreateDxcLinker(riid, ppv);
  	}
// Note: The following targets are not yet enabled for non-Windows platforms.
#ifdef _WIN32
  	else if (IsEqualCLSID(rclsid, CLSID_DxcDiaDataSource)) {
  	  hr = CreateDxcDiaDataSource(riid, ppv);
  	}
#endif
  	else {
  	  hr = REGDB_E_CLASSNOTREG;
  	}
  	return hr;
}

DXC_API_IMPORT HRESULT __stdcall DxcCreateInstance(REFCLSID rclsid, REFIID riid, _Out_ LPVOID *ppv) {
  	if (ppv == nullptr) {
  		return E_POINTER;
  	}

  	HRESULT hr = S_OK;
  	DxcEtw_DXCompilerCreateInstance_Start();
  	DxcThreadMalloc TM(nullptr);
  	hr = ThreadMallocDxcCreateInstance(rclsid, riid, ppv);
  	DxcEtw_DXCompilerCreateInstance_Stop(hr);
  	return hr;
}

DXC_API_IMPORT HRESULT __stdcall DxcCreateInstance2(IMalloc *pMalloc,
                                                	REFCLSID rclsid,
                                                	REFIID riid,
                                                	_Out_ LPVOID *ppv) {
  	if (ppv == nullptr) {
  		return E_POINTER;
  	}
#ifdef DXC_DISABLE_ALLOCATOR_OVERRIDES
  	if (pMalloc != DxcGetThreadMallocNoRef()) {
  		return E_INVALIDARG;
  	}
#endif // DXC_DISABLE_ALLOCATOR_OVERRIDES

  	HRESULT hr = S_OK;
  	DxcEtw_DXCompilerCreateInstance_Start();
  	DxcThreadMalloc TM(pMalloc);
  	hr = ThreadMallocDxcCreateInstance(rclsid, riid, ppv);
  	DxcEtw_DXCompilerCreateInstance_Stop(hr);
  	return hr;
}

#pragma endregion

#pragma region CAPI


struct WritableDxcBuffer
{
    void* Ptr;
    size_t Size;
    uint32_t Encoding;
};


/* Callback for local file inclusion */
typedef WritableDxcBuffer (*IncludeFunc)(void* ctx, char* pFilename);


class DelegateIncludeHandler : public IDxcIncludeHandler 
{
public:

    void* context;
    IncludeFunc delegate;

    DelegateIncludeHandler(void* ctx, IncludeFunc deleg) : context(ctx), delegate(deleg) { }


    ULONG STDMETHODCALLTYPE AddRef() override
    {
        return 1;
    }


    ULONG STDMETHODCALLTYPE Release() override
    {
        return 1;
    }


    HRESULT STDMETHODCALLTYPE QueryInterface(REFIID riid, void **ppvObject) override 
    {
        if (riid == __uuidof(IDxcIncludeHandler) || riid == __uuidof(IUnknown))
        {
            AddRef();
            *ppvObject = this;
            return S_OK;
        }

        *ppvObject = nullptr;
        return E_NOINTERFACE;
    }
    

    HRESULT STDMETHODCALLTYPE LoadSource(_In_ LPCWSTR pFilename, _COM_Outptr_result_maybenull_ IDxcBlob **ppIncludeSource) override 
    {
        try 
        {   
            size_t mbSize = std::wcstombs(nullptr, pFilename, 0);
            if (mbSize == (size_t)-1)
                throw std::runtime_error("wcstombs error");

            char* filenameUtf8 = new char[mbSize + 1]; // +1 for null terminator

            // Convert the wide string to multibyte
            if (wcstombs(filenameUtf8, pFilename, mbSize + 1) == (size_t)-1) 
            {
                delete[] filenameUtf8;
                throw std::runtime_error("wcstombs error");
            }

            WritableDxcBuffer result = delegate(context, filenameUtf8);

            delete[] filenameUtf8;

            CComPtr<IDxcBlobEncoding> textBlob;
            HRESULT hr = ::hlsl::DxcCreateBlob(result.Ptr, result.Size, false, true, true, result.Encoding, nullptr, &textBlob);

            if (result.Ptr != nullptr)
                free(result.Ptr);

            if (SUCCEEDED(hr)) 
                *ppIncludeSource = textBlob.Detach();

            return hr;
        }
        CATCH_CPP_RETURN_HRESULT();
    }

    ~DelegateIncludeHandler() { }
};  


extern "C"
{ 

    DXC_API_IMPORT IDxcCompiler3* CreateCompilerInstance()
    {
        IDxcCompiler3* pCompiler;
        DxcCreateInstance(CLSID_DxcCompiler, IID_PPV_ARGS(&pCompiler));
        return pCompiler;
    }


    DXC_API_IMPORT void DeleteCompilerInstance(IDxcCompiler3* compiler)
    {
        if (compiler != nullptr)
          compiler->Release();
    }


    DXC_API_IMPORT DelegateIncludeHandler* CreateIncludeHandler(void* ctx, IncludeFunc deleg)
    {
        return new DelegateIncludeHandler(ctx, deleg);
    }


    DXC_API_IMPORT void DeleteIncludeHandler(DelegateIncludeHandler* handler)
    {
        if (handler != nullptr)
            delete handler;
    }

    // NOTE: Interop does not play nice with UTF-16 on Linux and text was all garbage. Utf-8 works fine though so convert that into UTF-16 and then pass those args in.

    // Takes in a compiler instance, a text buffer, an arguments array in UTF-8, and an optional include handler
    // Assigns an IDxcResult instance which must be freed
    DXC_API_IMPORT HRESULT Compile(IDxcCompiler3* compiler, DxcBuffer source, const char** args, uint argsCount, DelegateIncludeHandler* includeHandler, IDxcResult** results)
    {
        // Convert the UTF-8 array to wide strings (utf-16)
        LPCWSTR* wArgs = new LPCWSTR[argsCount];
        for (uint i = 0; i < argsCount; i++)
        {
            size_t length = strlen(args[i]) + 1;
            wchar_t* wideString = new wchar_t[length];
            std::mbstowcs(wideString, args[i], length);
            wArgs[i] = const_cast<LPCWSTR>(wideString);
        }

        HRESULT res = compiler->Compile(&source, wArgs, argsCount, includeHandler, IID_PPV_ARGS(results));

        // Delete the allocated arguments
        for (uint i = 0; i < argsCount; i++)
        {
            delete[] wArgs[i];
        }
        delete[] wArgs;

        return res;
    }


    // Frees the result output of Compile()
    DXC_API_IMPORT void FreeResult(IDxcResult* result)
    {
        if (result != nullptr)
          result->Release();
    }


    void SetEmpty(WritableDxcBuffer* buffer)
    {
        if (buffer != nullptr)
        {
            buffer->Ptr = nullptr;
            buffer->Size = 0;
            buffer->Encoding = 0;
        }
    }


    void CopyBlobToBuffer(IDxcBlob* blob, WritableDxcBuffer* buffer)
    {   
        if (blob == nullptr || buffer == nullptr)
        {
            SetEmpty(buffer);

            if (blob != nullptr)
                blob->Release();

            return;
        }

        size_t bufferSize = blob->GetBufferSize();
        void* bufferPtr = blob->GetBufferPointer();

        if (bufferSize <= 0 || bufferPtr == nullptr)
        {
            SetEmpty(buffer);
            blob->Release();
            return;
        }

        buffer->Ptr = malloc(bufferSize);
        buffer->Size = bufferSize;
        buffer->Encoding = 0;

        memcpy(buffer->Ptr, bufferPtr, bufferSize);

        // Try to extract the encoding from the blob
        IDxcBlobEncoding* encodedBlob = nullptr;
        if (SUCCEEDED(blob->QueryInterface(IID_PPV_ARGS(&encodedBlob))))
        {
            // Get encoding for output blob
            BOOL encodingKnown = FALSE;
            UINT32 codePage = 0;

            if (SUCCEEDED(encodedBlob->GetEncoding(&encodingKnown, &codePage)))
                buffer->Encoding = encodingKnown ? codePage : 0;
        }

        blob->Release();
    }


    // Allocates an output and name buffer based on the result output kind. These buffers must be released with FreeResultOutput()
    DXC_API_IMPORT HRESULT GetResultOutput(IDxcResult* result, DXC_OUT_KIND kind, WritableDxcBuffer* output, WritableDxcBuffer* shaderName)
    {
        if (result == nullptr || output == nullptr)
            return E_INVALIDARG;
        
        SetEmpty(output);
        SetEmpty(shaderName);

        if (!result->HasOutput(kind))
            return E_FAIL;

        IDxcBlob* blobOutput = nullptr;
        IDxcBlobWide* blobName = nullptr;

        HRESULT hr = result->GetOutput(kind, IID_PPV_ARGS(&blobOutput), &blobName);

        if (SUCCEEDED(hr))
        {
            CopyBlobToBuffer(blobOutput, output);
            CopyBlobToBuffer(blobName, shaderName);
            return S_OK;
        }

        return hr;
    }


    DXC_API_IMPORT void FreeBuffer(WritableDxcBuffer* buffer)
    {
        if (buffer != nullptr && buffer->Ptr != nullptr)
            free(buffer->Ptr);
    }


    DXC_API_IMPORT HRESULT GetStatus(IDxcResult* results)
    {
        HRESULT status;
        results->GetStatus(&status);
        return status;
    }

}

#pragma endregion