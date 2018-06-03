using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ThunderOnlineModule.Models;

namespace ThunderOnlineModule.Controllers
{
    public class apiExampleController : ApiController
    {
        private ZionEntities db = new ZionEntities();

        // GET: api/apiExample
        public IQueryable<tcClient> GettcClients()
        {
            return db.tcClients;
        }

        // GET: api/apiExample/5
        [ResponseType(typeof(tcClient))]
        public IHttpActionResult GettcClient(string id)
        {
            tcClient tcClient = db.tcClients.Find(id);
            if (tcClient == null)
            {
                return NotFound();
            }

            return Ok(tcClient);
        }

        // PUT: api/apiExample/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PuttcClient(string id, tcClient tcClient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tcClient.Id)
            {
                return BadRequest();
            }

            db.Entry(tcClient).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tcClientExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/apiExample
        [ResponseType(typeof(tcClient))]
        public IHttpActionResult PosttcClient(tcClient tcClient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tcClients.Add(tcClient);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (tcClientExists(tcClient.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tcClient.Id }, tcClient);
        }

        // DELETE: api/apiExample/5
        [ResponseType(typeof(tcClient))]
        public IHttpActionResult DeletetcClient(string id)
        {
            tcClient tcClient = db.tcClients.Find(id);
            if (tcClient == null)
            {
                return NotFound();
            }

            db.tcClients.Remove(tcClient);
            db.SaveChanges();

            return Ok(tcClient);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tcClientExists(string id)
        {
            return db.tcClients.Count(e => e.Id == id) > 0;
        }
    }
}