﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Confluent.Kafka;
using System.Net;

namespace message_broker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private ProducerConfig _config;

        public ValuesController()
        {
          _config = new ProducerConfig
            {
                BootstrapServers = "host1:9092,host2:9092",
                ClientId = Dns.GetHostName(),
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
        public async Task<ActionResult> Post([FromBody] string value)
        {
            using (var producer = new ProducerBuilder<Guid, string>(_config).Build())
            {
                await producer.ProduceAsync("gators", new Message<Guid, string> { Key = Guid.NewGuid(), Value = "value" });
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
