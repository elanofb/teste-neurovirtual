using System;
using System.Runtime.Serialization;

namespace DAL.Contribuicoes.Exceptions {

    public class ContribuicaoException : Exception {

        //
        public ContribuicaoException() : base() {

        }

        //
        public ContribuicaoException(String message) : base(message) {

        }

        //
        public ContribuicaoException(String message, Exception innerException) : base(message, innerException) {

        }

        //
        protected ContribuicaoException(SerializationInfo info, StreamingContext context) : base(info, context) {

        }
    }
}
