namespace ConsoleApp1;

public class CalcPercent
{
    public double GetPercent(double number1, double maxValue)
    {
        return 100.0 / (maxValue / number1);
    }
}