using System;

namespace TermProject
{
    [Serializable]
    class DirtyFieldException : Exception
    {
        public DirtyFieldException() { }

        public DirtyFieldException(string field, string handler) : base($"Field {field} is handled by {handler}. You are not allowed to set it explicitly.") { }
    }
}
