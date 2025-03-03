//Event Handler

using System;

public class FireAlarmEventArgs : EventArgs
{
    public string SumberApi { get; }
    public bool Notified { get; }

    public FireAlarmEventArgs(string sumber, bool notified)
    {
        SumberApi = sumber;
        Notified = notified;
    }
}

//Event Handler pada Class Alarm
public class Alarm
{
    //Buat event pakai delegate EventHandler
    public event EventHandler<FireAlarmEventArgs>? OnAlarmRinging;

    public void FireRinging(string sumber, bool notified)
    {
        Console.WriteLine($"ALARM KEBAKARAN! Sumber api: {sumber}. Pemadam kebakaran {(notified ? "sudah" : "belum")} diinformasikan");
        OnAlarmRinging?.Invoke(this, new FireAlarmEventArgs(sumber, notified));
    }
}

//Class Person yang mendengarkan event (subscriber)
public class Person
{
    public void FireAlarmHandler(object? sender, FireAlarmEventArgs e)
    {
        Console.WriteLine($"Peringatan Darurat! Kebakaran terjadi karena {e.SumberApi}! Pemadam kebakaran {(e.Notified ? "sudah" : "belum")} berangkat ke lokasi");
    }
}

class Program
{
    static void Main()
    {
        Alarm alarm = new Alarm();
        Person person = new Person();

        //Subscribe ke Event
        alarm.OnAlarmRinging += person.FireAlarmHandler;
        //Simulasi event
        alarm.FireRinging("Kompor Gas", true);
    }
}