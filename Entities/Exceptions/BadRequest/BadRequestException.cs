using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions.BadRequest
{
    public abstract class BadRequestException : Exception
    {
        protected BadRequestException(string message) : base(message)
        {
            
        }
    }

    public sealed class IdsOfCollectionBadRequest : BadRequestException
    {
        public IdsOfCollectionBadRequest():base("Ids of collection are not valid")
        {
            
        }
    }

    public sealed class CollectionBadRequest : BadRequestException
    {
        public CollectionBadRequest():base("Collection didn't match ids")
        {
            
        }
    }

    public sealed class CompanyCollectionBadRequest : BadRequestException
    {
        public CompanyCollectionBadRequest():base("Companies Collection sent from client is null")
        {
            
        }
    }
}
