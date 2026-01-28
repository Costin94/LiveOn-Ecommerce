using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using LiveOn.Ecommerce.Application.Commands.Products;
using LiveOn.Ecommerce.Application.DTOs;
using LiveOn.Ecommerce.Application.Interfaces;
using LiveOn.Ecommerce.Application.Queries.Products;

namespace LiveOn.Ecommerce.API.Controllers
{
    /// <summary>
    /// API Controller for Product operations
    /// Demonstrates CQRS pattern with commands and queries
    /// </summary>
    [RoutePrefix("api/products")]
    public class ProductsController : ApiController
    {
        private readonly ICommandHandler<CreateProductCommand, int> _createProductHandler;
        private readonly ICommandHandler<UpdateProductCommand, bool> _updateProductHandler;
        private readonly ICommandHandler<DeleteProductCommand, bool> _deleteProductHandler;
        private readonly ICommandHandler<UpdateProductStockCommand, bool> _updateStockHandler;
        private readonly IQueryHandler<GetProductByIdQuery, ProductDto> _getByIdHandler;
        private readonly IQueryHandler<GetAllProductsQuery, IEnumerable<ProductDto>> _getAllHandler;
        private readonly IQueryHandler<GetProductBySkuQuery, ProductDto> _getBySkuHandler;

        public ProductsController(
            ICommandHandler<CreateProductCommand, int> createProductHandler,
            ICommandHandler<UpdateProductCommand, bool> updateProductHandler,
            ICommandHandler<DeleteProductCommand, bool> deleteProductHandler,
            ICommandHandler<UpdateProductStockCommand, bool> updateStockHandler,
            IQueryHandler<GetProductByIdQuery, ProductDto> getByIdHandler,
            IQueryHandler<GetAllProductsQuery, IEnumerable<ProductDto>> getAllHandler,
            IQueryHandler<GetProductBySkuQuery, ProductDto> getBySkuHandler)
        {
            _createProductHandler = createProductHandler ?? throw new ArgumentNullException(nameof(createProductHandler));
            _updateProductHandler = updateProductHandler ?? throw new ArgumentNullException(nameof(updateProductHandler));
            _deleteProductHandler = deleteProductHandler ?? throw new ArgumentNullException(nameof(deleteProductHandler));
            _updateStockHandler = updateStockHandler ?? throw new ArgumentNullException(nameof(updateStockHandler));
            _getByIdHandler = getByIdHandler ?? throw new ArgumentNullException(nameof(getByIdHandler));
            _getAllHandler = getAllHandler ?? throw new ArgumentNullException(nameof(getAllHandler));
            _getBySkuHandler = getBySkuHandler ?? throw new ArgumentNullException(nameof(getBySkuHandler));
        }

        /// <summary>
        /// GET api/products
        /// Gets all products with optional filtering
        /// </summary>
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetAll(
            [FromUri] int? categoryId = null,
            [FromUri] string searchTerm = null,
            [FromUri] decimal? minPrice = null,
            [FromUri] decimal? maxPrice = null,
            [FromUri] bool? inStock = null)
        {
            try
            {
                var query = new GetAllProductsQuery
                {
                    CategoryId = categoryId,
                    SearchTerm = searchTerm,
                    MinPrice = minPrice,
                    MaxPrice = maxPrice,
                    InStock = inStock
                };

                var products = await _getAllHandler.HandleAsync(query);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// GET api/products/5
        /// Gets a product by ID
        /// </summary>
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> GetById(int id)
        {
            try
            {
                var query = new GetProductByIdQuery(id);
                var product = await _getByIdHandler.HandleAsync(query);

                if (product == null)
                    return NotFound();

                return Ok(product);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// GET api/products/sku/ABC123
        /// Gets a product by SKU
        /// </summary>
        [HttpGet]
        [Route("sku/{sku}")]
        public async Task<IHttpActionResult> GetBySku(string sku)
        {
            try
            {
                var query = new GetProductBySkuQuery(sku);
                var product = await _getBySkuHandler.HandleAsync(query);

                if (product == null)
                    return NotFound();

                return Ok(product);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// POST api/products
        /// Creates a new product
        /// </summary>
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Create([FromBody] CreateProductCommand command)
        {
            try
            {
                if (command == null)
                    return BadRequest("Product data is required");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var productId = await _createProductHandler.HandleAsync(command);
                
                return Created($"api/products/{productId}", new { Id = productId });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// PUT api/products/5
        /// Updates an existing product
        /// </summary>
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Update(int id, [FromBody] UpdateProductCommand command)
        {
            try
            {
                if (command == null)
                    return BadRequest("Product data is required");

                if (id != command.Id)
                    return BadRequest("ID mismatch");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var success = await _updateProductHandler.HandleAsync(command);
                
                if (!success)
                    return NotFound();

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// DELETE api/products/5
        /// Deletes a product
        /// </summary>
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            try
            {
                var command = new DeleteProductCommand { Id = id };
                var success = await _deleteProductHandler.HandleAsync(command);
                
                if (!success)
                    return NotFound();

                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// PATCH api/products/5/stock
        /// Updates product stock quantity
        /// </summary>
        [HttpPatch]
        [Route("{id:int}/stock")]
        public async Task<IHttpActionResult> UpdateStock(int id, [FromBody] UpdateProductStockCommand command)
        {
            try
            {
                if (command == null)
                    return BadRequest("Stock data is required");

                if (id != command.ProductId)
                    return BadRequest("ID mismatch");

                var success = await _updateStockHandler.HandleAsync(command);
                
                if (!success)
                    return NotFound();

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
