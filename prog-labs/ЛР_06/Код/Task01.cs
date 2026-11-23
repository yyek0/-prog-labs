using System;
using System.Text;

class Professia
{
    protected string name, workplace;
    protected int experience;
    public string Name { get { return name; } }
    public string Workplace { get { return workplace; } }
    public int Experience { get { return experience; } }
    protected Professia(string name, string workplace, int experience)
    {
        this.name = name; this.workplace = workplace; this.experience = experience;
    }
    public virtual string Describe()
    { return string.Format("{0}, место работы: {1}, стаж: {2}", Name, Workplace, Experience); }
}

class Teacher : Professia
{
    private int school;
    private string subject;
    public int School { get { return school; } }
    public string Subject { get { return subject; } }
    public Teacher(string name, string workplace, int experience, int school, string subject)
        : base(name, workplace, experience) { this.school = school; this.subject = subject; }
    public override string Describe()
    { return "Учитель: " + base.Describe() + ", школа №" + School + ", предмет: " + Subject; }
}
class Driver : Professia
{
    private string transport;
    private int distance;
    public string Transport { get { return transport; } }
    public int Distance { get { return distance; } }
    public Driver(string name, string workplace, int experience, string transport, int distance)
        : base(name, workplace, experience) { this.transport = transport; this.distance = distance; }
    public override string Describe()
    { return "Водитель: " + base.Describe() + ", транспорт: " + Transport + ", дальность: " + Distance + " км"; }
}
class Doktor : Professia
{
    private string department, specialty;
    public string Department { get { return department; } }
    public string Specialty { get { return specialty; } }
    public Doktor(string name, string workplace, int experience, string department, string specialty)
        : base(name, workplace, experience) { this.department = department; this.specialty = specialty; }
    public override string Describe()
    { return "Врач: " + base.Describe() + ", отделение: " + Department + ", профиль: " + Specialty; }
}
class Task1
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.Write("ФИО учителя: "); string teacher = Console.ReadLine();
        Console.Write("ФИО водителя: "); string driver = Console.ReadLine();
        Console.Write("ФИО врача: "); string doctor = Console.ReadLine();
        Professia[] people = {
            new Teacher(teacher, "Школа", 8, 12, "математика"),
            new Driver(driver, "Автопарк", 6, "автобус", 300),
            new Doktor(doctor, "Больница", 10, "терапия", "терапевт")
        };
        foreach (Professia person in people) Console.WriteLine(person.Describe());
    }
}
