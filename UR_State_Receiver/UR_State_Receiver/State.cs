using System;

public class State
{
    public string Description { get; set; }
    public int Length { get; set; }
    public bool Radian { get; set; }
    public int Start { get; set; }
    public Type ValueType { get; set; }
    public bool Visible { get; set; }
    public ValueType Value { get; set; }

    public State(string description, int length, bool radian, int start, string valueType, bool visible)
    {
        Description = description;
        Length = length;
        Radian = radian;
        Start = start;
        Visible = visible;
        ValueType = valueType switch
        {
            "!i" => typeof(int),
            "!d" => typeof(double),
            _ => typeof(string),
        };
    }

    public override string ToString()
    {
        if (Radian)
        {
            return $"{Description}: \t {Truncate((180/Math.PI)*(double)Value)}";
        }
        else
        {
            return $"{Description}: \t {Truncate(Convert.ToDouble(Value))}";
        }
    }

    private static double Truncate(double number)
    {
        double stepper = Math.Pow(10, 4);
        return Math.Truncate(stepper * number) / stepper;
    }
}

