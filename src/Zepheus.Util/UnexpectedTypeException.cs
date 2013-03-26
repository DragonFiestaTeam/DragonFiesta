using System;
using System.Runtime.Serialization;

namespace DragonFiesta.Util
{
	[Serializable]
	public class UnexpectedTypeException : Exception
	{
		//
		// For guidelines regarding the creation of new exception types, see
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
		// and
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
		//

		public UnexpectedTypeException()
		{
		}

		public UnexpectedTypeException(string message) : base(message)
		{
		}

		public UnexpectedTypeException(string message, Exception inner) : base(message, inner)
		{
		}

		public UnexpectedTypeException(Type pExpectedType, Type pRealTime)
		{
			ExpectedType = pExpectedType;
			RealType = pRealTime;
		}

		protected UnexpectedTypeException(
			SerializationInfo info,
			StreamingContext context) : base(info, context)
		{
		}

		public Type ExpectedType { get; set; }
		public Type RealType { get; set; }
	}
}