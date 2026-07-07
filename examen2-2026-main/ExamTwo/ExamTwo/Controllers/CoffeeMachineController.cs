using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExamTwo.Controllers
{
    public class CoffeeMachinePriceController : Controller
    {

        private readonly Database _db;
        private readonly ICoffeMachineService;

        public CoffeeMachinePriceController(Database db)
        {
            _db = db;
            _coffeeService = ICoffeMachineService;
        }

        [HttpGet("getCoffees")]
        public ActionResult<Dictionary<string, int>> GetCoffeePrices()
        {
            return Ok(_coffeeService.GetCoffeePrices());
        }

        [HttpGet("getCoffeePricesInCents")]
        public ActionResult<Dictionary<string, int>> GetCoffeePricesInCents()
        {
            return Ok(_coffeeService.GetCoffeePricesInCents());
        }
    }

    public class CoffeeMachineMoneyQuantityController : Controller
    {

        private readonly Database _db;

        public CoffeeMachineMoneyQuantityController(Database db)
        {
            _db = db;
        }

        [HttpGet("getQuantity")]
        public ActionResult<Dictionary<string, int>> GetQuantity()
        {
            return Ok(_coffeeService.GetCoffeePricesInCents())
        }

    }
    public class CoffeeMachineBuyController : Controller
    {

        private readonly Database _db;

        public CoffeeMachineBuyController(Database db)
        {
            _db = db;
        }

        [HttpPost("buyCoffee")]
        public ActionResult<string> BuyCoffee([FromBody] OrderRequest request)
        {


                return Ok(_coffeeService.BuyCoffee())
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }

