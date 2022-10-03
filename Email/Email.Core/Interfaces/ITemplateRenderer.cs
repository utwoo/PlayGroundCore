using System.Threading;
using System.Threading.Tasks;

namespace Email.Core.Interfaces
{
    public interface ITemplateRenderer
    {
        string Parse<T>(string template, T model, bool isHtml = true);
        Task<string> ParseAsync<T>(string template, T model, bool isHtml = true, CancellationToken cancellationToken = default);
    }
}