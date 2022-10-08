using ConsoleTables;
using InsuranceLib.DataBaseContollers;
using InsuranceLib.Models;

void PrintTable<T>(IQueryable<T> dbSet) where T : class
{
    ConsoleTable table = new ConsoleTable();
    table.AddColumn(typeof(T).GetProperties().Where(x => !x.PropertyType.IsGenericType).Select(x => x.Name).ToList());

    foreach (var data in dbSet)
    {
        table.AddRow(data.GetType().GetProperties().Where(x => !x.PropertyType.IsGenericType).Select(x => x.GetValue(data)).ToArray());
    }
    table.Write();
}

using (InsuranceContext db = new InsuranceContext())
{

    var risks = db.Risks;
    Console.WriteLine("1. FullRisksTable (Press Enter)");
    Console.ReadLine();
    PrintTable(risks);

    Console.WriteLine("2.RisksTable with AverageProperty more than 50 (Press Enter)");
    Console.ReadLine();
    PrintTable(risks.Where(x => x.AverageProbability > 50));

    var polisis = db.Policies;
    Console.WriteLine("3.NotFinish Policies (Press Enter)");
    Console.ReadLine();
    Console.WriteLine(polisis.Count(x => x.IsFinish == 0));

    Console.WriteLine("4. (Press Enter)");
    Console.ReadLine();
    PrintTable( db.Policies.Join(db.TypesOfPolicies, x=> x.TypesOfPolicieId,
        y => y.Id,
        (x, y ) => new { 
                StartDate = x.StartDate,
                FinishDate = x.FinishDate,
                TypesOfPolicy= y.Name
        }));

    Console.WriteLine("5. (Press Enter)");
    Console.ReadLine();
    PrintTable(db.Policies.Where(x => x.Price > 550).Join(db.TypesOfPolicies, x => x.TypesOfPolicieId,
        y => y.Id,
        (x, y) => new {
            StartDate = x.StartDate,
            FinishDate = x.FinishDate,
            TypesOfPolicy = y.Name
        }));

    //6
    var newRisk = new Risk()
    {
        Name = "newRisk",
        Description = "newDisc",
        AverageProbability = 90
    };
    db.Risks.Add(newRisk);

    //7
    var policies = new Policy()
    {
        StartDate = new DateTime(2021, 1, 1),
        FinishDate = new DateTime(2029, 1, 1),
        Price = 940,
        Payment = 940,
        IsFinish = 0,
        IsPaied = 1
    };
    policies.Client = new Client()
    {
        FirstName = "Name",
        LastName = "Name",
        MiddleName = "Name",
        BornDate = new DateTime(1998,4,1),
        Adres = "adres",
        Gender = "m",
        Phone = 346421,
        Passport = "rd4645",
    };
    policies.Employee = new Employee()
    {
        FirstName = "Name",
        LastName= "Name",
        MiddleName= "Name", 
        Age = 45,
        Experience = 20,
        Gender = "m",
        Post = "Post"
    };
    policies.TypesOfPolicie = new TypesOfPolicy()
    {
        Name = "Name",
        Description = "Des",
        Condition = "Con"
    };
    db.Policies.Add(policies);

    //8
    var risk = db.Risks.First();
    db.Risks.Remove(risk);

    //9
    var policies1 = db.Policies.First();
    db.Policies.Remove(policies);

    //10
    var risk1 = db.Risks.Where(x => x.AverageProbability > 60).FirstOrDefault();
    risk1.Name = "Avg600";
    db.SaveChanges();
}







