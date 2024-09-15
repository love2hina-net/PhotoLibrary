using System.Runtime.InteropServices;
using System.Runtime.Loader;
using love2hina.Windows.MAUI.PhotoLibrary.Common.DependencyInjection;
using love2hina.Windows.MAUI.PhotoLibrary.Common.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace love2hina.Windows.MAUI.PhotoLibrary.Common.Database;

[DeclareService(ServiceLifetime.Singleton)]
public class AssemblyResolver
{

    protected readonly ILogger<AssemblyResolver> logger;

    public AssemblyResolver(ILoggerFactory loggerFactory)
    {
        logger = loggerFactory.CreateLogger<AssemblyResolver>();

        if (RuntimeInformationExtensions.OSPlatformIsMac())
        {
            // macOSの場合、libfbclient.dylibを読み込むためのResolverを登録
            logger.LogInformation("Registering NativeLibrary Resolver");

            // アセンブリパスと相対パスを結合して、正規化し、絶対パスを取得
            var path = Path.GetFullPath("./lib/libfbclient.dylib",
                Path.GetDirectoryName(GetType().Assembly.Location) ?? throw new InvalidOperationException());
            logger.LogDebug($"libfbclient.dylib: {path}");

            AssemblyLoadContext.Default.ResolvingUnmanagedDll += (assembly, libraryName) =>
            {
                var dll = IntPtr.Zero; 
                
                try {
                    if (libraryName == "fbclient")
                    {
                        dll = NativeLibrary.Load(path);
                    }
                }
                catch (Exception e)
                {
                    logger.LogError(e, $"Error loading library: {path} as {libraryName}");
                }

                return dll;
            };
        }
    }

}
