namespace D2MapApi.Common.DataStructures
{
    public struct Point2D(uint p_x, uint p_y)
    {
        public uint X { get; set; } = p_x;

        public uint Y { get; set; } = p_y;
    }
}
