using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;

namespace Binarysharp.MemoryManagement.Threading
{
    /// <summary>
    /// Represents a thread synchronization event for interprocess usage.
    /// </summary>
    /// <remarks>
    /// The access rule for the event is defined as <see cref="EventWaitHandleRights.FullControl"/> for <see cref="WellKnownSidType.BuiltinUsersSid"/>.
    /// </remarks>
    /// <seealso cref="EventWaitHandle"/>
    public class InterProcessEventWaitHandle : EventWaitHandle
    {
        /// <summary>
        /// The prefix used to enlarge the scope of the event to all the processes on the system.
        /// </summary>
        protected const string PrefixName = @"Global\";

        /// <summary>
        /// The authorized that can interact with the event.
        /// </summary>
        protected static SecurityIdentifier AuthorizedUsers;

        /// <summary>
        /// The access rule defined for the event.
        /// </summary>
        protected static EventWaitHandleAccessRule AccessRule;

        /// <summary>
        /// The security used when the event is created, that references the authorized users and the access rule.
        /// </summary>
        protected static EventWaitHandleSecurity Security;

        /// <summary>
        /// Only used to call the base constructor that requires a memory location to stores whether this is a new event according its name.
        /// As this is a static variable, it doesn't make any sense. Do not use it.
        /// </summary>
        private static bool _lostCreatedFlag;

        /// <summary>
        /// Initializes static members of the <see cref="InterProcessEventWaitHandle"/> class.
        /// </summary>
        static InterProcessEventWaitHandle()
        {
            InitializeEventSecurity();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InterProcessEventWaitHandle"/> class.
        /// </summary>
        /// <param name="name">The name of a system-wide synchronization event.</param>
        /// <param name="mode">One of the <see cref="T:System.Threading.EventResetMode" /> values that determines whether the event resets automatically or manually.</param>
        /// <param name="initialState">true to set the initial state to signaled if the named event is created as a result of this call; false to set it to nonsignaled.</param>
        public InterProcessEventWaitHandle(string name, EventResetMode mode, bool initialState = false) : base(initialState, mode, PrefixName + name, out _lostCreatedFlag, Security)
        {
        }

        /// <summary>
        /// Initializes the security for future events creation.
        /// </summary>
        private static void InitializeEventSecurity()
        {
            AuthorizedUsers = new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null);
            AccessRule = new EventWaitHandleAccessRule(AuthorizedUsers, EventWaitHandleRights.FullControl, AccessControlType.Allow);
            Security = new EventWaitHandleSecurity();

            Security.AddAccessRule(AccessRule);
        }
    }
}
