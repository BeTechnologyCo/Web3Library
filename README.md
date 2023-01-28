# Web3Library for Unity
A powerfull library to interact with EVM blockchains (Ethereum/Polygon/Fantom/BSC/Avalanche...)

You have 3 possibilities to connect to an account :
* With a private key and rpc url, usefull for test or server client !!!Don't publish your private key, a malicious user can steal your key!!!
* With Metamask, works only in WebGL mode, the apps will use the metamsak extension wallet of the user to interact with the blockchain
* With WalletConnect and rpc url, the app will use the rpc url to call node and use the wallet client to send/sign transaction

## Compatibility
I don't have test all platform for the moment,  but the goal is to work on WebGL/Windows/Editor/Mobile
| Connection Mode  | WebGL          | Editor| iOs | Android | Windows |
| :--------------- |:---------------| :-----|:-----|:-----|:-----|
| RPC  |   :white_check_mark:     |  :white_check_mark: | ontest | ontest | ontest |  ontest: |
| Metamask  | :white_check_mark: |  :no_entry_sign: | :no_entry_sign: | :no_entry_sign: | :no_entry_sign: |  :no_entry_sign: |
| Wallet connect  | :white_check_mark: | :white_check_mark: | ontest | ontest | ontest |  ontest |

## Third Party
[Nethereum](https://nethereum.com/) rewrite partially to encode/decode data and send request to blockchain node

[WalletConnect](https://github.com/WalletConnect/WalletConnectSharp) to connect to your personnal wallet

[UniTask](https://github.com/Cysharp/UniTask) to have task who works on every Unity platforms

[NativeWebSocket](https://github.com/endel/NativeWebSocket) to use WalletConnect with all Unity export platforms

[Zxing](https://github.com/micjahn/ZXing.Net) to generate QR code