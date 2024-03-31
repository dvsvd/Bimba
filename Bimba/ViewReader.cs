using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;

namespace Bimba
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class ViewReader : IExternalCommand
    {
        public Result Execute(ExternalCommandData revit,
            ref string message, ElementSet elements)
        {
            try
            {
                View3D view = (View3D)revit.View;
                IEnumerable<FamilyInstance> beams
                    = new FilteredElementCollector(view.Document)
                    .OfCategory(BuiltInCategory.OST_StructuralColumns)
                    .OfType<FamilyInstance>()
                    .Where(e => Regex.IsMatch(e.Symbol.FamilyName,
                    "^M_HSS Square-Column.*", RegexOptions.Compiled |
                    RegexOptions.IgnoreCase));
                string msg = beams.Aggregate(new StringBuilder(4096),
                    (builder, p2) => { builder.AppendLine($"Название: {p2.Name}, ID: {p2.Id}"); return builder; }).ToString();
                TaskDialog dlg = new TaskDialog("Колонны")
                {
                    MainContent = msg
                };
                using (StreamWriter writer = new StreamWriter(
                    $@"C:/Users/{Environment.UserName}/Desktop/Columns.txt",
                    false, new UTF8Encoding(false, true)))
                {
                    _ = writer.WriteAsync(msg);
                }
                _ = dlg.Show();
                return Result.Succeeded;
            }
            catch(InvalidCastException)
            {
                TaskDialog.Show("Предупреждение", "Текущий вид не является 3D-видом");
                return Result.Failed;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Ошибка", ex.ToString());
                return Result.Failed;
            }
        }
    }
}
