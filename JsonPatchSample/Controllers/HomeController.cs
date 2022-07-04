using JsonPatchSample.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;

namespace JsonPatchSample.Controllers
{
    [Route("jsonpatch/[action]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        /*
         * PUT & PATCH 都为更新资源
         * PUT 更新整个资源
         * PATCH 更新变更部分
         */

        [HttpPatch]
        public IActionResult JsonPatchWithModelState([FromBody] JsonPatchDocument<Customer> patchDoc)
        {
            if (patchDoc != null)
            {
                var customer = CreateCustomer();

                patchDoc.ApplyTo(customer, ModelState);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                return new ObjectResult(customer);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPatch]
        public IActionResult JsonPatchWithModelStateAndPrefix([FromBody] JsonPatchDocument<Customer> patchDoc,
            string prefix)
        {
            var customer = CreateCustomer();

            patchDoc.ApplyTo(customer, ModelState, prefix);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return new ObjectResult(customer);
        }

        public IActionResult JsonPatchWithoutModelState([FromBody] JsonPatchDocument<Customer> patchDoc)
        {
            var customer = CreateCustomer();

            patchDoc.ApplyTo(customer);

            return new ObjectResult(customer);
        }

        [HttpPatch]
        public IActionResult JsonPatchForProduct([FromBody] JsonPatchDocument<Product> patchDoc)
        {
            var product = new Product();

            patchDoc.ApplyTo(product);

            return new ObjectResult(product);
        }

        [HttpPatch]
        public IActionResult JsonPatchForDynamic([FromBody] JsonPatchDocument patch)
        {
            dynamic obj = new ExpandoObject();
            patch.ApplyTo(obj);

            return Ok(obj);
        }

        #region Utility
        private Customer CreateCustomer()
        {
            return new Customer
            {
                CustomerName = "John",
                Orders = new List<Order>
                {
                    new Order
                    { OrderName="Order0", },
                    new Order
                    { OrderName="Order1", },
                },
            };
        }
        #endregion
    }
}
