using System.Text;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using WalletConnectSharp.Core.Models;

namespace WalletConnectSharp.Core.Network
{
    public interface ICipher
    {
        UniTask<EncryptedPayload> EncryptWithKey(byte[] key, string data, Encoding encoding = null);

        UniTask<string> DecryptWithKey(byte[] key, EncryptedPayload encryptedData, Encoding encoding = null);
    }
}