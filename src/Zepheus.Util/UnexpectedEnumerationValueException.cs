using System;
using System.Runtime.Serialization;

namespace DragonFiesta.Util
{
	[Serializable]
	public class UnexpectedEnumerationValueException : Exception
	{
		//
		// For guidelines regarding the creation of new exception types, see
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
		// and
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
		//

		public UnexpectedEnumerationValueException()
		{
		}

		public UnexpectedEnumerationValueException(string message) : base(message)
		{
		}

		public UnexpectedEnumerationValueException(string message, Exception inner) : base(message, inner)
		{
		}

		protected UnexpectedEnumerationValueException(
			SerializationInfo info,
			StreamingContext context) : base(info, context)
		{
		}
	}
}