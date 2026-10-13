using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Models;

namespace ProductCatalog.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private static readonly List<Product> Products =
    [
        new()
        {
            Id = 1,
            Name = "Keyboard K280",
            Brand = "Logitech",
            Category = "Аксесуари",
            Price = 899.99m,
            AvailabilityStatus = AvailabilityStatus.InStock
        },
        new()
        {
            Id = 2,
            Name = "Mouse M185",
            Brand = "Logitech",
            Category = "Аксесуари",
            Price = 499.50m,
            AvailabilityStatus = AvailabilityStatus.LowStock
        },
        new()
        {
            Id = 3,
            Name = "Monitor 24 Plus",
            Brand = "Samsung",
            Category = "Монітори",
            Price = 5299.00m,
            AvailabilityStatus = AvailabilityStatus.InStock
        },
        new()
        {
            Id = 4,
            Name = "USB-C Hub 7-in-1",
            Brand = "Baseus",
            Category = "Аксесуари",
            Price = 1299.00m,
            AvailabilityStatus = AvailabilityStatus.OutOfStock
        }
    ];

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<Product>> GetAll([FromQuery] string? category)
    {
        IEnumerable<Product> result = Products;

        if (!string.IsNullOrWhiteSpace(category))
        {
            result = result.Where(product =>
                string.Equals(product.Category, category, StringComparison.OrdinalIgnoreCase));
        }

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Product> GetById(int id)
    {
        Product? product = Products.FirstOrDefault(product => product.Id == id);

        return product is null
            ? NotFound(new { message = $"Product with id {id} was not found." })
            : Ok(product);
    }

    [HttpGet("category/{category}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<Product>> GetByCategory(string category)
    {
        var result = Products.Where(product =>
            string.Equals(product.Category, category, StringComparison.OrdinalIgnoreCase));

        return Ok(result);
    }

    [HttpGet("brand/{brand}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<Product>> GetByBrand(string brand)
    {
        var result = Products.Where(product =>
            string.Equals(product.Brand, brand, StringComparison.OrdinalIgnoreCase));

        return Ok(result);
    }

    [HttpGet("price-range/{min:decimal}/{max:decimal}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<IEnumerable<Product>> GetByPriceRange(decimal min, decimal max)
    {
        if (min > max)
        {
            return BadRequest(new
            {
                message = "Invalid price range: min must be less than or equal to max."
            });
        }

        var result = Products.Where(product => product.Price >= min && product.Price <= max);

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<Product> Create(Product product)
    {
        product.Id = Products.Count == 0 ? 1 : Products.Max(existingProduct => existingProduct.Id) + 1;
        Products.Add(product);

        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Product> Update(int id, Product updatedProduct)
    {
        if (id != updatedProduct.Id)
        {
            return BadRequest(new { message = "Route id must match product id in the request body." });
        }

        Product? product = Products.FirstOrDefault(product => product.Id == id);

        if (product is null)
        {
            return NotFound(new { message = $"Product with id {id} was not found." });
        }

        product.Name = updatedProduct.Name;
        product.Brand = updatedProduct.Brand;
        product.Category = updatedProduct.Category;
        product.Price = updatedProduct.Price;
        product.AvailabilityStatus = updatedProduct.AvailabilityStatus;

        return Ok(product);
    }

    [HttpPatch("{id:int}/toggle-stock")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Product> ToggleStock(int id)
    {
        Product? product = Products.FirstOrDefault(product => product.Id == id);

        if (product is null)
        {
            return NotFound(new { message = $"Product with id {id} was not found." });
        }

        product.AvailabilityStatus = product.AvailabilityStatus == AvailabilityStatus.OutOfStock
            ? AvailabilityStatus.InStock
            : AvailabilityStatus.OutOfStock;

        return Ok(product);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        Product? product = Products.FirstOrDefault(product => product.Id == id);

        if (product is null)
        {
            return NotFound(new { message = $"Product with id {id} was not found." });
        }

        Products.Remove(product);

        return NoContent();
    }
}
