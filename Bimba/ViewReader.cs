using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                //View3D view = (View3D)revit.View;
                var allWallsOfTypeX
                    = new FilteredElementCollector(revit.Application.ActiveUIDocument.Document)
                    //.OfCategory(BuiltInCategory.OST_Columns)
                    .WhereElementIsNotElementType()
                    .OfCategory(BuiltInCategory.OST_StructuralColumns);
                // .Where(e => e.Id == new ElementId(1417454));
                IEnumerable<Element> L = allWallsOfTypeX.ToElements();//.Where(e => e.Category.Name == "Несущие колонны");//.Where(e => e.Id == new ElementId(1417481));
                //.Where(e => e.Category.B
                return Result.Succeeded;
            }
            catch(InvalidCastException)
            {
                TaskDialog.Show("Предупреждение", "Текущий вид не является 3D-видом");
                return Result.Failed;
            }
        }
    }
}
