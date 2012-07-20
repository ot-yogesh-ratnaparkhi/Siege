using System;
using System.Runtime.Serialization;

namespace Siege.Repository.Web
{
	[Serializable]
	public class HttpUnitOfWorkStoreManagerModuleException : Exception
	{
		public HttpUnitOfWorkStoreManagerModuleException()
		{
		}

		public HttpUnitOfWorkStoreManagerModuleException(string message)
			: base(message)
		{
		}

		public HttpUnitOfWorkStoreManagerModuleException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected HttpUnitOfWorkStoreManagerModuleException(
			SerializationInfo info,
			StreamingContext context)
			: base(info, context)
		{
		}
	}
}
