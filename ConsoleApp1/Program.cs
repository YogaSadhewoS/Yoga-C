// See https://aka.ms/new-console-template for more information

//Day 2
Console.WriteLine("Masukkan angka: ");
int x = Convert.ToInt32(Console.ReadLine());

int n = 1;
Console.WriteLine("Hasilnya adalah: ");

while (n <= x)
{
    if (n % 3 == 0 && n % 5 == 0) 
    {
        Console.Write("foobar");
    }

    else if (n % 3 == 0)
    {
        Console.Write("foo");
    }

    else if (n % 5 == 0)
    {
        Console.Write("bar");
    }

    else
    {
        Console.Write(n);
    }

    Console.Write(", ");
    n++;
}
