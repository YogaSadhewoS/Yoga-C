//Interface

using System;

public interface IBisaBerkendara
{
    void Berkendara();
}

public class Motor : IBisaBerkendara
{
    public void Berkendara() => Console.WriteLine("Motor sedang berjalan");
}

class Program
{
    static void Main()
    {
        Motor myMotor = new Motor();
        myMotor.Berkendara();
    }
}