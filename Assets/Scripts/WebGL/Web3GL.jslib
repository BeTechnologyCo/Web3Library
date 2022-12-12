mergeInto(LibraryManager.library, {
    Connect: async function(callback)
    {
       if(!window.map){
            window.map = {};
       }
      const accounts = await ethereum.request({ method: 'eth_requestAccounts' });
      //window.map[-1]=;
       let convertResponse = "";
        if(accounts[0] !== null) {
            var bufferSize = lengthBytesUTF8(accounts[0]) + 1;
            var buffer = _malloc(bufferSize);
            stringToUTF8(accounts[0], buffer, bufferSize);
            convertResponse = buffer;
        }
        dynCall_vi(callback, convertResponse);

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
          dynCall_vi(callback, convertResponse);
       });
    },
    CallContract: async function(index, parametersJson, callback)
    {
       const parametersString = UTF8ToString(parametersJson);
       let parsedMessage = JSON.parse(parametersString);
       console.log("parsedMessage",parsedMessage);
       let response = await ethereum.request({ method: 'eth_call',params :[parsedMessage] });
       console.log("response",response);
       if(!window.map){
            window.map = {};
       }
       console.log(index);
        window.map[index] = response;
        let convertResponse = "";
        if(response !== null) {
            var bufferSize = lengthBytesUTF8(response) + 1;
            var buffer = _malloc(bufferSize);
            stringToUTF8(response, buffer, bufferSize);
            convertResponse = buffer;
        }
        dynCall_vii(callback, index, convertResponse);
    },
     SendContract: async function(index, parametersJson)
    {
       const parametersString = UTF8ToString(parametersJson);
       let parsedMessage = JSON.parse(parametersString);
       console.log("parsedMessage",parsedMessage);
       let response = await ethereum.request({ method: 'eth_sendTransaction',params :[parsedMessage] });
       console.log("response",response);
       if(!window.map){
            window.map = {};
       }
       console.log(index);
        window.map[index] = response;

        if(response !== null) {
            var bufferSize = lengthBytesUTF8(response) + 1;
            var buffer = _malloc(bufferSize);
            stringToUTF8(response, buffer, bufferSize);
            return buffer;
        }
        return "";
    },
    GetResult: function (index){
    if(window.map){
    console.log("map",window.map);
        var value = window.map[index];
        if(!value !== null)
        {
            if(typeof value === 'object' || Array.isArray(value))
            {
                // stringify object and array
              var value = JSON.stringify(value);
            }
            var bufferSize = lengthBytesUTF8(value) + 1;
            var buffer = _malloc(bufferSize);
            stringToUTF8(value, buffer, bufferSize);
            return buffer;
        }
    }
    return null;
  }

});