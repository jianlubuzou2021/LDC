using System.Collections.Generic;

namespace LDC
{
    interface Operation
    {
        void CloseBtd();

        List<string> FindBtdWalletPid();

        void CleanData();

        void FetchData();

        void StartBtd();

        void Dispose();
    }
}
