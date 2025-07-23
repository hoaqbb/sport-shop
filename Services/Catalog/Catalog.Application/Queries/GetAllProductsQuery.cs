using Catalog.Application.Responses;
using Catalog.Core.Specifications;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Queries
{
    public class GetAllProductsQuery : IRequest<Pagination<ProductResponse>>
    {
        public CatalogSpecificationParams CatalogSpecificationParams { get; set; }
        public GetAllProductsQuery(CatalogSpecificationParams catalogSpecificationParams)
        {
            CatalogSpecificationParams = catalogSpecificationParams;
        }
    }
}
