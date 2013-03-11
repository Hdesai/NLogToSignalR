using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Http;
using Microsoft.AspNet.SignalR.Client.Hubs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace NLog.Targets.SignalR.Tests
{
    [TestClass]
    public class PublishToHubTests
    {
        private IHubConnectionProxy _connectionProxyMock;
        private IHttpClient _httpClientMock;


        [TestInitialize]
        public void StartTest()
        {
            _connectionProxyMock = Substitute.For<IHubConnectionProxy>();
            _httpClientMock = Substitute.For<IHttpClient>();
        }

        [TestMethod]
        public void Connect_Should_Set_IsConnected_To_True_When_Start_Task_Returns_Success()
        {
            var hub = new PublishToHub(_connectionProxyMock, "", "", "");

            _connectionProxyMock.CreateProxy("").Returns(x => new HubProxy(null, null));

            _connectionProxyMock.StartConnection(_httpClientMock).Returns(info => Task.FromResult(true));

            _connectionProxyMock.State.Returns(ConnectionState.Connected);

            hub.Connect(_httpClientMock);

            Assert.IsTrue(hub.IsConnected);
        }


        [TestMethod]
        public void Connect_Should_Not_Set_IsConnected_To_True_When_Connection_State_Is_Not_Connected()
        {
            var hub = new PublishToHub(_connectionProxyMock, "", "", "");

            _connectionProxyMock.StartConnection(_httpClientMock).Returns(info => Task.FromResult(true));

            _connectionProxyMock.State.Returns(ConnectionState.Connecting);

            try
            {
                hub.Connect(_httpClientMock);
            }
            catch (NLogToSignalRTargetException)
            {
            }

            Assert.IsFalse(hub.IsConnected);
        }

        [TestMethod]
        public void SendToSignalR_Should_Set_SentToSignalR_Flag_To_True_On_Success()
        {
            //Arrange
            var publisher = new FakePublishToHub(_connectionProxyMock, "", "", "");
            _connectionProxyMock.StartConnection(_httpClientMock).Returns(info => Task.FromResult(true));

            var proxy = Substitute.For<IHubProxy>();
            _connectionProxyMock.CreateProxy("").Returns(proxy);

            _connectionProxyMock.State.Returns(ConnectionState.Connected);

            publisher.Connect(_httpClientMock);

            //Act
            publisher.Execute_SendToSignalR(new Message());

            Assert.IsTrue(publisher.SentToSignalR);
        }


        [TestMethod]
        public void SendToSignalR_Send_Message_To_Specific_MethodCall_Only()
        {
            //Arrange
            var publisher = new FakePublishToHub(_connectionProxyMock, "groupName", "methodToCallOnServer", "");
            _connectionProxyMock.StartConnection(_httpClientMock).Returns(info => Task.FromResult(true));

            var proxy = Substitute.For<IHubProxy>();
            _connectionProxyMock.CreateProxy("").Returns(proxy);

            _connectionProxyMock.State.Returns(ConnectionState.Connected);

            publisher.HubProxy = proxy;

            publisher.Connect(_httpClientMock);

            var message = new Message();

            //Act
            publisher.Execute_SendToSignalR(message);

            proxy.Received(1)
                 .Invoke(Arg.Is("methodToCallOnServer"),
                         Arg.Is<object[]>(x => x[0] == message && x[1].ToString() == "groupName"));
        }


        [TestMethod]
        public void Should_Throw_SignalR_Exception_If_Connection_Is_Not_Made()
        {
            var publisher = new FakePublishToHub(_connectionProxyMock, "groupName", "methodToCallOnServer", "");
            _connectionProxyMock.StartConnection(_httpClientMock).Returns(info => Task.FromResult(true));

            var proxy = Substitute.For<IHubProxy>();
            _connectionProxyMock.CreateProxy("").Returns(proxy);

            _connectionProxyMock.State.Returns(ConnectionState.Connecting);

            var message = new Message();

            //Act
            try
            {
                publisher.Execute_SendToSignalR(message);
                Assert.Fail("Above line-Should have thrown exception");
            }
            catch (NLogToSignalRTargetException)
            {
              
            }
            
        }

    }

    public class FakePublishToHub : PublishToHub
    {
        public FakePublishToHub(IHubConnectionProxy hubConnectionProxy, string groupName, string methodToCallOnServer,
                                string hubName) : base(hubConnectionProxy, groupName, methodToCallOnServer, hubName)
        {
        }

        public IHubProxy HubProxy { get; set; }

        public void Execute_SendToSignalR(Message message)
        {
            if (HubProxy != null)
            {
                _hubProxy = HubProxy;
            }

            base.SendToSignalR(message);
        }
    }
}