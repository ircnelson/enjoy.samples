using System;
using System.Threading.Tasks;
using System.Web.Http;
using EnjoyCQRS.EventSource;
using EnjoyCQRS.MessageBus;
using EnjoySample.Restaurant.Commands;

namespace EnjoySample.Restaurant.Command.WebApi.Controllers
{
    public class TabController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICommandDispatcher _commandDispatcher;

        public TabController(IUnitOfWork unitOfWork, ICommandDispatcher commandDispatcher)
        {
            _unitOfWork = unitOfWork;
            _commandDispatcher = commandDispatcher;
        }
        
        [HttpPost]
        public async Task<IHttpActionResult> Open(int number, string waiter)
        {
            try
            {
                var tabId = Guid.NewGuid();

                await _commandDispatcher.DispatchAsync(new OpenTabCommand(tabId, number, waiter));

                await _unitOfWork.CommitAsync();

                return Ok(tabId);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
        
        [HttpPost]
        public async Task<IHttpActionResult> Close(string id, decimal amountPaid)
        {
            try
            {
                await _commandDispatcher.DispatchAsync(new CloseTabCommand(Guid.Parse(id), amountPaid));
                await _unitOfWork.CommitAsync();

                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}