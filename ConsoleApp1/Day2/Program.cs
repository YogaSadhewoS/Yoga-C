// See https://aka.ms/new-console-template for more information

//Day 2 (Logic Exercises)
/*
Console.WriteLine("Masukkan jumlah angka: ");
int x;

while(!int.TryParse(Console.ReadLine(), out x) && x <= 0)
{
    Console.WriteLine("Mohon masukkan angka bulat positif");
}

int n = 1;
while(n <= x)
{
    if(n % 5 == 0 && n % 3 == 0)
    {
        Console.Write("foobar");
    }

    else if(n % 3 == 0)
    {
        Console.Write("foo");
    }

    else if(n % 5 == 0)
    {
        Console.Write("bar");
    }

    else
    {
        Console.Write(n);
    }

    if(n < x) Console.Write(", ");
    n++;
}
*/

//Mini calculator

using System;

Console.WriteLine("Masukkan angka pertama: ");
int firstNumber = Convert.ToInt32(Console.ReadLine());

Console.WriteLine("Masukkan angka kedua: ");
int secondNumber = Convert.ToInt32(Console.ReadLine());

Console.WriteLine("Pilih operasi: \n 1. Penjumlahan \n 2. Pengurangan \n 3. Perkalian \n 4. Pembagian");

string? input = Console.ReadLine();

if(!string.IsNullOrEmpty(input) && input.Length == 1)
{
    char operation = Convert.ToChar(input);
    switch (operation)
    {
        case '1':
            Console.WriteLine($"Result: {firstNumber + secondNumber}");
            break;
        case '2':
            Console.WriteLine($"Result: {firstNumber - secondNumber}");
            break;
        case '3':
            Console.WriteLine($"Result: {firstNumber * secondNumber}");
            break;
        case '4':
            if (secondNumber == 0)
            {
                Console.WriteLine("Error: Division by zero");
            }
            else
            {
                Console.WriteLine($"Hasil: {firstNumber / (double)secondNumber}");
            }
            break;
        default:
            Console.WriteLine("Invalid operation");
            break;
    }
}

else
{
    Console.WriteLine("Input harus berupa angka dan operator");
}

//Number guessing game
/*
Random random = new Random();
int randomNumber = random.Next(1, 101);

Console.WriteLine("Tebak angka antara 1 sampai 100!");

while (true)
{
    Console.Write("Masukkan tebakan anda: ");
    int guess = Convert.ToInt32(Console.ReadLine());
    
    if (guess == randomNumber)
    {
        Console.WriteLine("Selamat, anda telah menebak angka yang benar!");
        break;
    }
    else if (guess < randomNumber)
    {
        Console.WriteLine("Angka yang anda coba tebakan lebih kecil!");
    }
    else if (guess > randomNumber)
    {
        Console.WriteLine("Angka yang anda coba tebakan lebih besar!");
    }
    else
    {
        Console.WriteLine("Input harus berupa angka!");
    }
}
*/