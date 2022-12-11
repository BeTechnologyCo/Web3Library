mergeInto(LibraryManager.library, {
    Connect: async function()
    {
      const accounts = await ethereum.request({ method: 'eth_requestAccounts' });
    },
    CallContract: async function(id, parametersJson)
    {
       const parametersString = UTF8ToString(parametersJson);
       let parsedMessage = JSON.parse(parametersString);
       console.log("parsedMessage",parsedMessage);
       let response = await ethereum.request({ method: 'eth_call',params :[parsedMessage] });
       console.log("response",response);
       if(!window.map){
            window.map = {};
       }

        window.map[UTF8ToString(id)] = JSON.stringify(response);

        if(response !== null) {
            var bufferSize = lengthBytesUTF8(response) + 1;
            var buffer = _malloc(bufferSize);
            stringToUTF8(response, buffer, bufferSize);
            return buffer;
        }
        return "";
    },
    GetResult: function (id){
    if(window.map && window.map[UTF8ToString(id)]){
         var value = window.map[UTF8ToString(id)];
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