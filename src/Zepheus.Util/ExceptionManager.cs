using System;
using System.Collections.Generic;

namespace DragonFiesta.Util
{
    public class ExceptionManager
    {
        #region .ctor

        public ExceptionManager()
        {
            registeredTypes = new Dictionary<Type, Action<Exception>>();
        }

        #endregion
        #region Properties

        public static ExceptionManager Instance { get; private set; }

        private Dictionary<Type, Action<Exception>> registeredTypes;

        #endregion
        #region Methods

        public static bool Initialize()
        {
            try
            {
                Instance = new ExceptionManager();
            }
            catch (Exception e)
            {
                Logs.Main.Fatal("Could not initialize ExceptionManager", e);
                return false;
            }
            return true;
        }

        public void RegisterExceptionType(Type pType, Action<Exception> pAction)
        {
            if (registeredTypes.ContainsKey(pType))  // override action
                registeredTypes[pType] = pAction;
            else
                registeredTypes.Add(pType, pAction);
        }
        public void UnregisterExceptionType(Type pType)
        {
            registeredTypes.Remove(pType);
        }
        public bool TypeRegistered(Type pType)
        {
            return registeredTypes.ContainsKey(pType);
        }

        public bool HandleException(Exception e)
        {
            Type type = e.GetType();
            if (TypeRegistered(type))
            {
                registeredTypes[type](e);
                return true;
            }
            else
                return false;
        }

        #endregion
    }
}