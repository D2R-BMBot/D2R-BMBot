namespace BMBot.GUI.Avalonia.Models.DataStructures.Inventory;

public class InventoryData
{
    public InventoryData()
    {
        for ( var i = 0; i < Cells.GetLength(0); i++ )
        {
            for ( var j = 0; j < Cells.GetLength(1); j++ )
            {
                Cells[i, j] = new InventoryCell();
            }
        }
    }

    public InventoryCell[,] Cells { get; } = new InventoryCell[10, 4];

    public InventoryCell InventoryCell00 => Cells[0, 0];
    public InventoryCell InventoryCell01 => Cells[0, 1];
    public InventoryCell InventoryCell02 => Cells[0, 2];
    public InventoryCell InventoryCell03 => Cells[0, 3];
    public InventoryCell InventoryCell10 => Cells[1, 0];
    public InventoryCell InventoryCell11 => Cells[1, 1];
    public InventoryCell InventoryCell12 => Cells[1, 2];
    public InventoryCell InventoryCell13 => Cells[1, 3];
    public InventoryCell InventoryCell20 => Cells[2, 0];
    public InventoryCell InventoryCell21 => Cells[2, 1];
    public InventoryCell InventoryCell22 => Cells[2, 2];
    public InventoryCell InventoryCell23 => Cells[2, 3];
    public InventoryCell InventoryCell30 => Cells[3, 0];
    public InventoryCell InventoryCell31 => Cells[3, 1];
    public InventoryCell InventoryCell32 => Cells[3, 2];
    public InventoryCell InventoryCell33 => Cells[3, 3];
    public InventoryCell InventoryCell40 => Cells[4, 0];
    public InventoryCell InventoryCell41 => Cells[4, 1];
    public InventoryCell InventoryCell42 => Cells[4, 2];
    public InventoryCell InventoryCell43 => Cells[4, 3];
    public InventoryCell InventoryCell50 => Cells[5, 0];
    public InventoryCell InventoryCell51 => Cells[5, 1];
    public InventoryCell InventoryCell52 => Cells[5, 2];
    public InventoryCell InventoryCell53 => Cells[5, 3];
    public InventoryCell InventoryCell60 => Cells[6, 0];
    public InventoryCell InventoryCell61 => Cells[6, 1];
    public InventoryCell InventoryCell62 => Cells[6, 2];
    public InventoryCell InventoryCell63 => Cells[6, 3];
    public InventoryCell InventoryCell70 => Cells[7, 0];
    public InventoryCell InventoryCell71 => Cells[7, 1];
    public InventoryCell InventoryCell72 => Cells[7, 2];
    public InventoryCell InventoryCell73 => Cells[7, 3];
    public InventoryCell InventoryCell80 => Cells[8, 0];
    public InventoryCell InventoryCell81 => Cells[8, 1];
    public InventoryCell InventoryCell82 => Cells[8, 2];
    public InventoryCell InventoryCell83 => Cells[8, 3];
    public InventoryCell InventoryCell90 => Cells[9, 0];
    public InventoryCell InventoryCell91 => Cells[9, 1];
    public InventoryCell InventoryCell92 => Cells[9, 2];
    public InventoryCell InventoryCell93 => Cells[9, 3];
}