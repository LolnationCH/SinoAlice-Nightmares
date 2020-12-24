namespace SinoAlice_Nightmares.Objects
{
  public class Settings
  {
    private double width = 450;
    private double height = 450;

    private int numColumns = 2;

    public double Width { get => width; set => width = value; }
    public double Height { get => height; set => height = value; }
    public int NumColumns { get => numColumns; set => numColumns = value; }
  }
}
