using Hans.Redis.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Sockets;

namespace Hans.Redis.Test
{
    /// <summary>
    ///  Test containing all methods related to the base interactions with the <see cref="RedisHost" /> class.
    /// </summary>
    [TestClass]
    public class RedisHostTest
    {
        /// <summary>
        ///  Ensures the Redis instance starts successfully when requested.
        /// </summary>
        [TestMethod]
        public void RedisHost_ProcessStartsSuccessfully()
        {
            // Create a new Redis host, and ensure the process is running.
            RedisHost testHost = new RedisHost();
            testHost.Start();

            // Process running.
            Assert.IsTrue(testHost.Running);

            // Make sure the port we've requested is actually open.
            bool portOpen = false;
            try
            {
                using (TcpClient tcpClient = new TcpClient())
                {
                    tcpClient.Connect("localhost", InstanceParams.DefaultPortNumber);
                    portOpen = true;
                }
            }
            catch { }

            // Port can be spoken to.
            Assert.IsTrue(portOpen);
        }

        /// <summary>
        ///  Ensures the process terminates successfully when the object is finished being used.
        /// </summary>
        [TestMethod]
        public void RedisHost_ProcessTerminatesSuccessfully()
        {
            // Create a new Redis host, and ensure the process is running.
            RedisHost testHost = new RedisHost();
            testHost.Start();

            // Dispose the host, and ensure that we've finished.
            testHost.Dispose();
            Assert.IsFalse(testHost.Running);
        }

        /// <summary>
        ///  Ensures we successfully return an active accessor, when the service is running.
        /// </summary>
        [TestMethod]
        public void RedisHost_ReturnsAccessorWhileRunning()
        {
            // Build the test host.
            RedisHost testHost = new RedisHost();
            testHost.Start();

            // Grab the accessor.
            var accessor = testHost.CreateAccessor();
            Assert.IsNotNull(accessor);
        }

        [TestMethod]
        public void RedisHost_ReturnsNullAccessorWhileNotRunning()
        {
            // Build the test host.
            RedisHost testHost = new RedisHost();
            testHost.Start();

            // Kill the process.
            testHost.Dispose();

            // Grab the accessor.
            var accessor = testHost.CreateAccessor();
            Assert.IsNull(accessor);
        }
    }
}
