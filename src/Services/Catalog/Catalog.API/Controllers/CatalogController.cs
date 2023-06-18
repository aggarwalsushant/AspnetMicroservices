using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CatalogController : ControllerBase
{
  private readonly IProductRepository _repository;
  private readonly ILogger<CatalogController> _logger;

  public CatalogController(IProductRepository repository, ILogger<CatalogController> logger)
  {
    _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    _logger = logger ?? throw new ArgumentNullException(nameof(_logger));
  }

  #region getters
  [HttpGet]
  [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)] //Can be used the following way too.
  //[ProducesResponseType(200)]
  public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
  {
    var products = await _repository.GetProducts();
    return Ok(products);
  }

  [HttpGet(template: "{id: Length(24)}", Name = "GetProduct")] //ensure param name and the keyword matches
  [ProducesResponseType(200)]
  [ProducesResponseType((int)HttpStatusCode.NotFound)] // another way of writing 404 - makes more intuitive.
  public async Task<ActionResult<Product>> GetProductById(string id)
  {
    var product = await _repository.GetProduct(id);

    if (product is null)
    {
      _logger.LogError($"Product with id: {id}, not found.");
      return NotFound();
    }

    return Ok(product);
  }

  [HttpGet]
  [Route(template: "[action]/{category}", Name = "GetProductByCategory")]
  [ProducesResponseType(200)]
  [ProducesResponseType(404)]
  public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category)
  {
    var products = await _repository.GetProductByCategory(category);

    if (products is null)
    {
      _logger.LogError($"Products with category: {category}, not found");
      return NotFound();
    }

    return Ok(products);
  }
  #endregion

  #region RemainingCruds
  [HttpPost]
  //[ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)] //One and the same thing.
  [ProducesResponseType(200)]
  public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
  {
    await _repository.CreateProduct(product);

    return CreatedAtRoute(routeName: "GetProduct", routeValues: new { id = product.Id }, value: product); // named params can be simplified. Used only for clarity.
  }

  [HttpPut]
  [ProducesResponseType(200)]
  public async Task<IActionResult> UpdateProduct([FromBody] Product product) //since we are returning only bool and not any specific type, we used IActionResult else we would have gone for ActionResult<T>
  {
    return Ok(await _repository.UpdateProduct(product));
  }

  [HttpDelete(template: "{id: Length(24)}", Name = "DeleteProduct")]
  [ProducesResponseType((int)HttpStatusCode.OK)]
  public async Task<IActionResult> DeleteProductById(string id)
  {
    return Ok(await _repository.DeleteProduct(id));
  }
  #endregion

}
