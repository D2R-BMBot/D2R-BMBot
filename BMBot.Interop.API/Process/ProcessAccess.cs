namespace BMBot.Interop.API.Process;

public enum ProcessAccess
{
    PROCESS_QUERY_INFORMATION = 0x0400,
    MEM_COMMIT                = 0x00001000,
    PROCESS_VM_OPERATION      = 0x0008,
    PROCESS_VM_READ           = 0x0010,
    PROCESS_VM_WRITE          = 0x0020,
    SYNCHRONIZE               = 0x00100000
}