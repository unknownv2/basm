using System.Diagnostics;
using System.ServiceModel;
using System.Threading;
using Binarysharp.MemoryManagement.Threading;

namespace Binarysharp.MemoryManagement.Assembly.Assembler
{
    /// <summary>
    /// Manages the fasm proxy and the named pipe communication with the proxy.
    /// </summary>
    public class NamedPipeFasmProxy
    {
        #region Fields

        /// <summary>
        /// The name of the event used to signal that the proxy has to be shut down.
        /// </summary>
        public const string ExitEventNamePrefix = "FasmProxy/Exit/";

        /// <summary>
        /// The name of the event used to signal that the proxy is ready to receive connection.
        /// </summary>
        public const string ReadyEventNamePrefix = "FasmProxy/Ready/";

        /// <summary>
        /// The event signaling that the process can be shut down.
        /// </summary>
        private InterProcessEventWaitHandle _exitEvent;

        /// <summary>
        /// The event signaling that the proxy is ready to be consumed.
        /// </summary>
        private InterProcessEventWaitHandle _readyEvent;

        /// <summary>
        /// The process of fasm proxy.
        /// </summary>
        private Process _fasmProxyProcess;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the hosted assembler connected to the fasm proxy using named pipes.
        /// </summary>
        /// <value>The hosted assembler.</value>
        public IHostedAssembler HostedAssembler { get; private set; }

        #endregion

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedPipeFasmProxy"/> class.
        /// This starts a new process of fasm proxy.
        /// </summary>
        public NamedPipeFasmProxy()
        {
            InitializeSynchronizationEvents();

            StartFasmProxy();

            _readyEvent.WaitOne();

            HostedAssembler = InitializeNamedPipeAssembler();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="NamedPipeFasmProxy"/> class.
        /// This shut down the process of fasm proxy.
        /// </summary>
        ~NamedPipeFasmProxy()
        {
            _exitEvent.Set();

            bool hasExited = _fasmProxyProcess.WaitForExit(3000);

            if (!hasExited)
            {
                _fasmProxyProcess.Kill();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Starts a process hosting the fasm proxy.
        /// </summary>
        private void StartFasmProxy()
        {
            var processInfo = new ProcessStartInfo("Binarysharp.FasmProxy.exe")
            {
                CreateNoWindow = true,
                UseShellExecute = false
            };

            _fasmProxyProcess = Process.Start(processInfo);
        }

        /// <summary>
        /// Initializes the events used to synchronize the the proxy with the library.
        /// </summary>
        private void InitializeSynchronizationEvents()
        {
            var processId = Process.GetCurrentProcess().Id.ToString();
            _exitEvent = new InterProcessEventWaitHandle(ExitEventNamePrefix + processId, EventResetMode.AutoReset);
            _readyEvent = new InterProcessEventWaitHandle(ReadyEventNamePrefix + processId, EventResetMode.AutoReset);
        }

        /// <summary>
        /// Initializes the named pipe assembler.
        /// </summary>
        private IHostedAssembler InitializeNamedPipeAssembler()
        {
            var uri = "net.pipe://localhost/FasmHostedAssembler/" + Process.GetCurrentProcess().Id;
            var binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
            var endpoint = new EndpointAddress(uri);

            return ChannelFactory<IHostedAssembler>.CreateChannel(binding, endpoint);
        }

        #endregion
    }
}
