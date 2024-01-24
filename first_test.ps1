$methodDefinition = @'
[DllImport("ole32")]
private static extern int CLSIDFromProgIDEx([MarshalAs(UnmanagedType.LPWStr)] string lpszProgID, out Guid lpclsid);

[DllImport("oleaut32")]
private static extern int GetActiveObject([MarshalAs(UnmanagedType.LPStruct)] Guid rclsid, IntPtr pvReserved, [MarshalAs(UnmanagedType.IUnknown)] out object ppunk);
public static object GetActiveObject(string progId, bool throwOnError = false)
{
    if (progId == null)
        throw new ArgumentNullException(nameof(progId));

    var hr = CLSIDFromProgIDEx(progId, out var clsid);
    if (hr < 0)
    {
        if (throwOnError)
            Marshal.ThrowExceptionForHR(hr);

        return null;
    }

    hr = GetActiveObject(clsid, IntPtr.Zero, out var obj);
    if (hr < 0)
    {
        if (throwOnError)
            Marshal.ThrowExceptionForHR(hr);

        return null;
    }
    return obj;
}
'@
$interop = add-type -MemberDefinition $methodDefinition -Name "Interop" -Namespace "Interop" -PassThru

// All of above is for this:

Clear-Host

$SEApp = $null
Write-Host 'Connecting to Solid Edge.'

# $SEApp = [System.Runtime.InteropServices.Marshal]::GetActiveObject('SolidEdge.Application')
$SEApp = $interop::GetActiveObject('SolidEdge.Application')
Write-Host 'Connected to Solid Edge.'

$SEDoc = $SEApp.Activedocument
Write-Host Active document: $SEDoc.Name

