#include <windows.h>
#include <metahost.h>
#pragma comment(lib, "mscoree.lib")

int main() {
    ICLRMetaHost* pMetaHost = NULL;
    ICLRRuntimeInfo* pRuntimeInfo = NULL;
    ICLRRuntimeHost* pRuntimeHost = NULL;

    if (CLRCreateInstance(CLSID_CLRMetaHost, IID_ICLRMetaHost, reinterpret_cast<LPVOID*>(&pMetaHost)) == S_OK) {
        if (pMetaHost->GetRuntime(L"v4.0.30319", IID_ICLRRuntimeInfo, reinterpret_cast<LPVOID*>(&pRuntimeInfo)) == S_OK) {
            if (pRuntimeInfo->GetInterface(CLSID_CLRRuntimeHost, IID_ICLRRuntimeHost, reinterpret_cast<LPVOID*>(&pRuntimeHost)) == S_OK) {
                if (pRuntimeHost->Start() == S_OK) {
                    DWORD dwReturnValue;
                    pRuntimeHost->ExecuteInDefaultAppDomain(L"C:\\Users\\Kaitlyn\\Documents\\GitHub\\NeoPolaris\\NeoPolaris\\bin\\Release\\Polaris.dll", L"NeoPolaris.Loader", L"HostedMain", L"", &dwReturnValue);
                }
            }
        }
    }

    return 0;
}

BOOL APIENTRY DllMain(HMODULE hModule, DWORD dwReason, LPVOID lpReserved) {
    if (dwReason == DLL_PROCESS_ATTACH) {
        if (CreateThread(0, 0, reinterpret_cast<LPTHREAD_START_ROUTINE>(main), 0, 0, 0)) {
            return TRUE;
        } else {
            return FALSE;
        }
    }

    return TRUE;
}
