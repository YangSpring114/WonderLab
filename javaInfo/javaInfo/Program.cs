// See https://aka.ms/new-console-template for more information

using javaInfo;

var javas = JavaInfo.FindJava();
foreach (var i in javas)
{
    Console.WriteLine(i.Path);
    Console.WriteLine(i.Version);
}

Console.WriteLine("END!!!");
