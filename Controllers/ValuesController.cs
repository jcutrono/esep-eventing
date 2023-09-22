using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace message_broker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private ProducerConfig _config;
        private IEnumerable<Guid> _items;
        private IEnumerable<Guid> _users;

        public ValuesController()
        {
            _config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
                ClientId = Dns.GetHostName(),
            };

            _items = new List<Guid>{
                { new Guid("00000000-0000-0000-0000-000000000000") }, 
                { new Guid("00000000-0000-0000-0000-000000000001") }, 
                { new Guid("00000000-0000-0000-0000-000000000002") }, 
            };   

            _users = new List<Guid>{
                { new Guid("00000000-0000-0000-0000-000000000000") }, 
                { new Guid("00000000-0000-0000-0000-000000000001") }, 
                { new Guid("00000000-0000-0000-0000-000000000002") }, 
            };   
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Order order)
        {
            if(!_users.Contains(order.UserId))
            {
                return NotFound("User not found");
            }

            if(!_items.Contains(order.ItemId))
            {
                return NotFound("Item not found");
            }

            order.Occurred = DateTime.UtcNow;
            order.EventType = "NewOrder";

            using (var producer = new ProducerBuilder<string, string>(_config).Build())
            {
                await producer.ProduceAsync("orders", new Message<string, string> { Key = Guid.NewGuid().ToString(), Value = JsonConvert.SerializeObject(order) });
            }
            
            return Created("TransactionId", "Your order is in progress");
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
