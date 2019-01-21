using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cellenza.Pinpon.Front;
using Cellenza.Pinpon.Front.Models;
using Microsoft.AspNetCore.SignalR;
using Cellenza.Pinpon.Front.Hubs;

namespace Cellenza.Pinpon.Front.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class QueueInformationsController : ControllerBase
    {
        private readonly PinponQueueContext _context;
        private readonly IHubContext<PinponHub> hubContext;

        public QueueInformationsController(
            PinponQueueContext context,
            IHubContext<PinponHub> hubContext)
        {
            _context = context;
            this.hubContext = hubContext;
        }

        // GET: QueueInformations
        [HttpGet]
        public IEnumerable<QueueInformationLight> GetQueueInformations()
        {
            return _context.QueueInformations.ToQueryableQueueInformationLight();
        }

        // GET: QueueInformations/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetQueueInformation([FromRoute] int id)
        {
            var queueInformation = await _context.QueueInformations.FindAsync(id);

            if (queueInformation == null)
            {
                return NotFound();
            }

            return Ok(queueInformation.ToQueueInformationLight());
        }

        // POST: QueueInformations
        [HttpPost]
        public async Task<IActionResult> PostQueueInformation([FromBody] QueueInformation queueInformation)
        {
            queueInformation.IsActive = true;
            _context.QueueInformations.Add(queueInformation);
            await _context.SaveChangesAsync();

            await hubContext.Clients.All.SendAsync("AddQueue", queueInformation);

            return CreatedAtAction("GetQueueInformation", new { id = queueInformation.Id }, queueInformation.ToQueueInformationLight());
        }

        [HttpGet("{id}/Deactivate")]
        public async Task<IActionResult> DeactivateQueueInformation([FromRoute]int id)
        {
            var queue = await ChangeIsActive(false, id);

            if (queue == null)
            {
                return NotFound();
            }

            await hubContext.Clients.All.SendAsync("RemoveQueue", id);
            return Ok(queue.ToQueueInformationLight());
        }

        [HttpGet("{id}/Activate")]
        public async Task<IActionResult> ActivateQueueInformation([FromRoute]int id)
        {
            var queue = await ChangeIsActive(true, id);

            if (queue == null)
            {
                return NotFound();
            }

            await hubContext.Clients.All.SendAsync("AddQueue", queue);
            return Ok(queue.ToQueueInformationLight());
        }

        // DELETE: QueueInformations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQueueInformation([FromRoute] int id)
        {
            var queueInformation = await _context.QueueInformations.FindAsync(id);
            if (queueInformation == null)
            {
                return NotFound();
            }

            _context.QueueInformations.Remove(queueInformation);
            await _context.SaveChangesAsync();

            return Ok(queueInformation.ToQueueInformationLight());
        }

        private async Task<QueueInformation> ChangeIsActive(bool value, int id)
        {
            var queueInformation = await _context.QueueInformations.FindAsync(id);
            if (queueInformation == null)
            {
                return null;
            }

            queueInformation.IsActive = value;

            try
            {
                _context.QueueInformations.Update(queueInformation);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QueueInformationExists(id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return queueInformation;
        }

        private bool QueueInformationExists(int id)
        {
            return _context.QueueInformations.Any(e => e.Id == id);
        }
    }
}