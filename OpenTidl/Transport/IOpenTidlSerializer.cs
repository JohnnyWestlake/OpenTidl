using System.IO;

namespace OpenTidl.Transport
{
    public interface IOpenTidlSerializer
    {
        TModel DeserializeObject<TModel>(Stream data) where TModel : class;
    }
}
