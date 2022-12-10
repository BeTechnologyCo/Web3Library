mergeInto(LibraryManager.library, {
    Connect: async function()
    {
      const accounts = await ethereum.request({ method: 'eth_requestAccounts' });
    },
    CallContract: async function(parametersJson)
    {
       const parametersString = UTF8ToString(parametersJson);
       let parsedMessage = JSON.parse(parametersString);
       let response = ethereum.request({ method: 'eth_call',params :[parsedMessage] }).await();

        if(response !== null) {
            var bufferSize = lengthBytesUTF8(response) + 1;
            var buffer = _malloc(bufferSize);
            stringToUTF8(response, buffer, bufferSize);
            return buffer;
        }
        return "";
    }
});