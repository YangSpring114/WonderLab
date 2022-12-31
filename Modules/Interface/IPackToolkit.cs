using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WonderLab.Modules.Interface
{
    /// <summary>
    /// 包工具接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPackToolkit<T>
    {
        public Task<ImmutableArray<T>> LoadAllAsync();
        public Task<ImmutableArray<T>> MoveLoadAllAsync(IEnumerable<string> paths);
    }
}
