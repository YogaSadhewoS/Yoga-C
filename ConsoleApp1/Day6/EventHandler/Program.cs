//Event Handler

using System;

public class Alarm
{
    //Buat event pakai delegate EventHandler
    public event EventHandler? OnAlarmRinging;

    public void Ringing()
    {
        Console.WriteLine("Alarm Berbunyi");
        OnAlarmRinging?.Invoke(this, EventArgs.Empty);
    }
}

//Class Person yang mendengarkan event
public class Person
{
    public void AlarmPerson(object? sender, EventArgs e)
    {
        Console.WriteLine("Event: Alarm sudah berbunyi, tolong segera matikan!");
    }
}

class Program
{
    static void Main()
    {
        Alarm alarm = new Alarm();
        Person person = new Person();

        //Subscribe ke Event
        alarm.OnAlarmRinging += person.AlarmPerson;
        //Simulasi event
        alarm.Ringing();
    }
    
}