    public Interface ICoffeeMachineService
    {

        public ActionResult<Dictionary<string, int>> GetCoffeePrices();
        public ActionResult<Dictionary<string, int>> GetCoffeePricesInCents();
        public ActionResult<string> BuyCoffee([FromBody] OrderRequest request);
        public ActionResult<Dictionary<string, int>> GetQuantity()
        
    }
    

    
    public class CoffeMachineService : ICoffeeMachineService
    {

        public CoffeMachineService()
        private readonly Database _db;

        public CoffeMachineService(Database db)
        {
            _db = db;
        }

        public ActionResult<Dictionary<string, int>> GetCoffeePrices(){

            return Ok(_db.keyValues1);
        }

        public ActionResult<Dictionary<string, int>> GetCoffeePricesInCents(){

            return Ok(_db.keyValues2);
        }

        public ActionResult<Dictionary<string, int>> GetQuantity(){

            return Ok(_db.keyValues3);
        }
        
        public ActionResult<string> BuyCoffee([FromBody] OrderRequest request){
            if (ValidateRequest(request))
            {
                return BadRequest("Orden vacia.");
            }

            try
            {
                var costoTotal = request.Order.Sum(o => _db.keyValues2.First(c => c.Key == o.Key).Value * o.Value);

                if (request.Payment.TotalAmount < costoTotal)
                {
                    return BadRequest("Dinero insuficiente ");
                }


                foreach (var cafe in request.Order)
                {
                    var selected = _db.keyValues.First(c => c.Key == cafe.Key).Key;
                    if (cafe.Value > _db.keyValues[selected])
                    {
                        return $"No hay suficientes {selected} en la máquina.";
                    }
                    _db.keyValues[selected] -= cafe.Value;
                }

                var change = request.Payment.TotalAmount - costoTotal;
                String result = $"Su vuelto es de: {change} colones. Desglose:";

                foreach (var coin in _db.keyValues3.Keys.OrderByDescending(c => c))
                {
                    var count = Math.Min(change / coin, _db.keyValues3[coin]);
                    if (count > 0)
                    {
                        result += $" {count} moneda de {coin},  ";
                        change -= coin * count;
                    }
                }


                if (change > 0)
                {
                    return StatusCode(500, "No hay suficiente cambio en la máquina.");
                }
        }
        private bool ValidateRequest(OrderRequest request)
        {
            if (request.Order == null || request.Order.Count == 0)
            {
                return false;
            }

            if (request.Payment.TotalAmount <= 0)
            {
                return false;
            }
        }
    }
    public class OrderRequest
    {
        public Dictionary<string, int> Order { get; set; }
        public Payment Payment { get; set; }
    }

    public class Payment
    {
        public int TotalAmount { get; set; }
        public List<int> Coins { get; set; }
        public List<int> Bills { get; set; }
    }