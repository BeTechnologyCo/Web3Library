mergeInto(LibraryManager.library, {
    Connect: async function(callback)
    {
        const accounts = await ethereum.request({ method: 'eth_requestAccounts' });

        let convertResponse = "";
        if(accounts[0] !== null) {
            var bufferSize = lengthBytesUTF8(accounts[0]) + 1;
            var buffer = _malloc(bufferSize);
            stringToUTF8(accounts[0], buffer, bufferSize);
            convertResponse = buffer;
        }
        dynCall_vii(callback, 1, convertResponse);

        ethereum.on("accountsChanged",
                function (accounts) {
                convertResponse = "";
                let account = "";
                if(accounts[0] !== undefined){
                   account = accounts[0];
                }
                if(account !== null) {
                   var bufferSize = lengthBytesUTF8(account) + 1;
                   var buffer = _malloc(bufferSize);
                   stringToUTF8(account, buffer, bufferSize);
                   convertResponse = buffer;
                }
                dynCall_vii(callback, 3, convertResponse);
       });
       ethereum.on("chainChanged",
                function (chainId) {
                convertResponse = "";
                if(chainId !== null) {
                   let value = chainId.toString();
                   var bufferSize = lengthBytesUTF8(value) + 1;
                   var buffer = _malloc(bufferSize);
                   stringToUTF8(value, buffer, bufferSize);
                   convertResponse = buffer;
                }
                dynCall_vii(callback, 2, convertResponse);
       });
    },
    Request: async function (jsonCall, callback) {
        const parsedJsonStr = UTF8ToString(jsonCall);
        let parsedJson = JSON.parse(parsedJsonStr);
        try {
            console.log("call",parsedJson);
            const response = await ethereum.request(parsedJson);
            let rpcResponse = {
                jsonrpc: "2.0",
                result: response,
                id: parsedJson.id,
                error: null
            }
            //console.log(rpcResponse);

            var json = JSON.stringify(rpcResponse);
            var bufferSize = lengthBytesUTF8(json) + 1;
            var buffer = _malloc(bufferSize);
            stringToUTF8(json, buffer, bufferSize);
            dynCall_vii(callback, parsedJson.id, buffer);
            return json;
        } catch (e) {
            let rpcResonseError = {
                jsonrpc: "2.0",
                id: parsedJson.id,
                error: {
                    message: e.message,
                }
            }
            var json = JSON.stringify(rpcResonseError);
            var bufferSize = lengthBytesUTF8(json) + 1;
            var buffer = _malloc(bufferSize);
            stringToUTF8(json, buffer, bufferSize);
            dynCall_vii(callback, parsedJson.id, buffer);
            return json;
        }
    },
    IsMetamaskAvailable: function () {
        if (window.ethereum) return true;
        return false;
    },
    GetSelectedAddress: function () {
        var returnValue = ethereum.selectedAddress;
        if(returnValue !== null) {
            var bufferSize = lengthBytesUTF8(returnValue) + 1;
            var buffer = _malloc(bufferSize);
            stringToUTF8(returnValue, buffer, bufferSize);
            return buffer;
        }
        return "";
    },
    IsConnected: function() {
        return ethereum.isConnected();
    },
    RequestRpcClientCallback: async function (callback, message) {
        const parsedMessageStr = UTF8ToString(message);
        const parsedCallback = UTF8ToString(callback);
        let parsedMessage = JSON.parse(parsedMessageStr);
        try {
            
            //console.log(parsedMessage);
            const response = await ethereum.request(parsedMessage);
            let rpcResponse = {
                jsonrpc: "2.0",
                result: response,
                id: parsedMessage.id,
                error: null
            }
            //console.log(rpcResponse);

            var json = JSON.stringify(rpcResponse);
            //console.log(json);
           
            var len = lengthBytesUTF8(json) + 1;
            var strPtr = _malloc(len);
            stringToUTF8(json, strPtr, len);
            Module.dynCall_vi(callback, strPtr);

            return json;
        } catch (e) {
            //console.log(e);
            let rpcResonseError = {
                jsonrpc: "2.0",
                id: parsedMessage.id,
                error: {
                    message: e.message,
                }
            }
            var json = JSON.stringify(rpcResonseError);
            //console.log(json);
            var len = lengthBytesUTF8(json) + 1;
            var strPtr = _malloc(len);
            stringToUTF8(json, strPtr, len);

            Module.dynCall_vi(callback, strPtr);
        }
    }

});