using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WalletConnectSharp.Core;
using WalletConnectSharp.Core.Events;
using WalletConnectSharp.Core.Models;
using WalletConnectSharp.Core.Network;
using WalletConnectSharp.Unity.Network;

namespace Web3Unity
{
    public class WalletConnect : WalletConnectSession
    {

        static WalletConnect()
        {
            TransportFactory.Instance.RegisterDefaultTransport((eventDelegator) => new WebsharpTransport(eventDelegator));
        }

        public WalletConnect(ClientMeta clientMeta, string bridgeUrl = null, ITransport transport = null, ICipher cipher = null, int chainId = 1, EventDelegator eventDelegator = null) : base(clientMeta, bridgeUrl, transport, cipher, chainId, eventDelegator)
        {
        }

        public WalletConnect(SavedSession savedSession, ITransport transport = null, ICipher cipher = null,
            EventDelegator eventDelegator = null) : base(savedSession, transport, cipher, eventDelegator)
        {
        }
    }
}