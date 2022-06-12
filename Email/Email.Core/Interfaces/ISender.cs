using System.Threading;
using System.Threading.Tasks;
using Email.Core.Models;

namespace Email.Core.Interfaces
{
    public interface ISender
    {
        SendResponse Send(IEmail email);
        Task<SendResponse> SendAsync(IEmail email, CancellationToken cancellationToken = default);
    }
}