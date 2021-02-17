using System;
using System.Diagnostics;

namespace Tests.DataModel
{
    [DebuggerDisplay("{_id} {RaiseException}")]
    public class SpecialEntity
    {
        private int _id;

        public int Id
        {
            get
            {
                if (RaiseException)
                {
                    throw Exception;
                }

                return _id;
            }

            set
            {
                _id = value;
            }
        }

        public bool RaiseException
        {
            get;
            set;
        }

        public Exception Exception
        {
            get;
            set;
        }
        
    }
}