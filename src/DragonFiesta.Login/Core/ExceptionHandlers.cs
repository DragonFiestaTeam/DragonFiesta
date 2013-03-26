using System;
using DragonFiesta.Util;

namespace DragonFiesta.Login.Core
{
    public static class ExceptionHandlers
    {
        public static void RegisterHandlers()
        {
            var i = ExceptionManager.Instance;

            i.RegisterExceptionType(typeof (UnexpectedTypeException), UnexpectedTypeException);
	        i.RegisterExceptionType(typeof (UnexpectedEnumerationValueException), UnexpectedEnumerationValue);
        }

        #region Handler
        public static void UnexpectedTypeException(Exception e)
        {
            UnexpectedTypeException ex = (UnexpectedTypeException) e;

	        Logs.Main.ErrorFormat("Unexpected type: Expected: {0}; Got {1}; In {2}::{3}",
	                              ex.ExpectedType,
	                              ex.RealType,
	                              ex.TargetSite.DeclaringType,
	                              ex.TargetSite.Name);
        }
		public static void UnexpectedEnumerationValue(Exception e)
		{
			var ex = e as UnexpectedEnumerationValueException;
			if (ex == null)
				return;
			Logs.Main.ErrorFormat("Unexpected enumeration value in {0}::{1}", 
					ex.TargetSite.DeclaringType, ex.TargetSite.Name);
		}
        #endregion
    }
}