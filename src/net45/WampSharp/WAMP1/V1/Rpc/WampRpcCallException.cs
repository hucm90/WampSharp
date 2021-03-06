﻿using System;
using System.Runtime.Serialization;

namespace WampSharp.V1.Rpc
{
    /// <summary>
    /// An exception having details that will be sent
    /// through a CALLERROR WAMP message.
    /// </summary>
    [Serializable]
    public class WampRpcCallException : Exception
    {
        private readonly string mProcUri;
        private readonly string mCallId;
        private readonly string mErrorUri;
        private readonly object mErrorDetails;

        /// <summary>
        /// Initializes a new instance of <see cref="WampRpcCallException"/>.
        /// </summary>
        /// <param name="errorUri"><see cref="ErrorUri"/></param>
        /// <param name="errorDesc">The error description, <see cref="Exception.Message"/>.</param>
        /// <param name="errorDetails"><see cref="ErrorDetails"/></param>
        public WampRpcCallException(string errorUri, string errorDesc, object errorDetails)
            : this(null, null, errorUri, errorDesc, errorDetails)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="WampRpcCallException"/>.
        /// </summary>
        /// <param name="procUri"><see cref="ProcUri"/></param>
        /// <param name="callId"><see cref="CallId"/></param>
        /// <param name="errorUri"><see cref="ErrorUri"/></param>
        /// <param name="errorDesc">The error description, <see cref="Exception.Message"/>.</param>
        /// <param name="errorDetails"><see cref="ErrorDetails"/></param>
        public WampRpcCallException(string procUri, string callId, string errorUri, string errorDesc, object errorDetails)
            : base(errorDesc)
        {
            mProcUri = procUri;
            mCallId = callId;
            mErrorUri = errorUri;
            mErrorDetails = errorDetails;
        }

        protected WampRpcCallException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// The called method's proc uri.
        /// </summary>
        public string ProcUri
        {
            get
            {
                return mProcUri;
            }
        }

        /// <summary>
        /// The call id of the WAMP CALL.
        /// </summary>
        public string CallId
        {
            get
            {
                return mCallId;
            }
        }

        /// <summary>
        /// The error uri.
        /// </summary>
        public string ErrorUri
        {
            get
            {
                return mErrorUri;
            }
        }

        /// <summary>
        /// The error details.
        /// </summary>
        public object ErrorDetails
        {
            get
            {
                return mErrorDetails;
            }
        }
    }
}