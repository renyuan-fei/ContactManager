using Microsoft.Extensions.Logging;

using RepositoryContracts;

using Serilog;

namespace ContactsManager.Core.Services.PersonService
{
  public class PersonsGetterServiceChild : PersonsGetterService
  {
    public PersonsGetterServiceChild(IPersonsRepository personsRepository, ILogger<PersonsGetterService> logger, IDiagnosticContext diagnosticContext) : base(personsRepository, logger, diagnosticContext)
    {
    }

    //违反LSP（Liskov Substitution Principle）原则）
    //因为子类的GetPersonsExcel方法抛出了异常，而父类的GetPersonsExcel方法没有抛出异常
    //子类和父类返回的参数数量或类型不一致
    public async override Task<MemoryStream> GetPersonsExcel()
    {
      throw new NotImplementedException();
      // MemoryStream memoryStream = new MemoryStream();
      // using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
      // {
      //   ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets.Add("PersonsSheet");
      //   workSheet.Cells["A1"].Value = "Person Name";
      //   workSheet.Cells["B1"].Value = "Age";
      //   workSheet.Cells["C1"].Value = "Gender";
      //
      //   using (ExcelRange headerCells = workSheet.Cells["A1:C1"])
      //   {
      //     headerCells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
      //     headerCells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
      //     headerCells.Style.Font.Bold = true;
      //   }
      //
      //   int row = 2;
      //   List<PersonResponse> persons = await GetAllPersons();
      //
      //   foreach (PersonResponse person in persons)
      //   {
      //     workSheet.Cells[row, 1].Value = person.PersonName;
      //     workSheet.Cells[row, 2].Value = person.Age;
      //     workSheet.Cells[row, 3].Value = person.Gender;
      //
      //     row++;
      //   }
      //
      //   workSheet.Cells[$"A1:C{row}"].AutoFitColumns();
      //
      //   await excelPackage.SaveAsync();
      // }
      //
      // memoryStream.Position = 0;
      // return memoryStream;
    }
  }
}
