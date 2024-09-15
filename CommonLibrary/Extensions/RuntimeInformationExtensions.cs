using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace love2hina.Windows.MAUI.PhotoLibrary.Common.Extensions;

public static class RuntimeInformationExtensions
{

    private static readonly Regex regexDarwin = new(@"darwin", RegexOptions.IgnoreCase);

    public static bool OSPlatformIsMac() {
        // TODO: 2024/09/22現在、RuntimeInformation.IsOSPlatform(OSPlatform.OSX)はmacOSでもfalseを返す
        // https://github.com/dotnet/runtime/issues/104160
        return RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || regexDarwin.IsMatch(RuntimeInformation.OSDescription);
    }

}
