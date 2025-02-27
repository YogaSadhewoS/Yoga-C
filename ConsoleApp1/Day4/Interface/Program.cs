//Interface

using System;

public interface IBisaBerkendara
{
    void Berkendara();
}

public interface IBisaTerbang
{
    void Terbang();
}

public interface IBisaBerenang
{
    void Berenang();
}

public class Motor : IBisaBerkendara
{
    public void Berkendara() => Console.WriteLine("Motor sedang berjalan");
}

public class Bebek : IBisaTerbang, IBisaBerenang
{
    public void Terbang() => Console.WriteLine("Bebek sedang terbang");
    public void Berenang() => Console.WriteLine("Bebek sedang berenang");
}

class Program
{
    static void Main()
    {
        Motor myMotor = new Motor();
        myMotor.Berkendara();

        Bebek myBebek = new Bebek();
        myBebek.Terbang();
        myBebek.Berenang();
    }
}