using System;
using System.Collections.Generic;
#region Autodesk
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
#endregion

[assembly: CommandClass(typeof(SelectionFence.Commands))]

namespace SelectionFence
{
    public class Commands
    {
        [CommandMethod("FenceSelect", CommandFlags.Modal)]
        public void FenceSelectAndLinetype_Method()
        {
            Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
            try
            {
                Point3dCollection pts = new Point3dCollection();

                PromptPointOptions prPntOpt = new PromptPointOptions("\nFence point: (Enter to finish)");
                prPntOpt.AllowArbitraryInput = true;
                prPntOpt.AllowNone = true;
                prPntOpt.UseBasePoint = false;
                prPntOpt.UseDashedLine = true;

                Point3d prePnt = new Point3d(double.NegativeInfinity, 0, 0);
                while (true)
                {
                    if (!double.IsNegativeInfinity(prePnt.X))
                    {
                        prPntOpt.UseBasePoint = true;
                        prPntOpt.BasePoint = prePnt;
                    }
                    PromptPointResult prPntRes = ed.GetPoint(prPntOpt);
                    if (prPntRes.Status == PromptStatus.OK)
                    {
                        pts.Add(prPntRes.Value);
                        prePnt = prPntRes.Value;
                    }
                    else
                        break;
                }

                PromptSelectionResult prSelRes = ed.SelectFence(pts);
                if (prSelRes.Status == PromptStatus.OK)
                {
                    SelectionSet ss = prSelRes.Value;
                    if (ss != null)
                        ed.WriteMessage("\nThe SS is good and has {0} entities.", ss.Count);
                    else
                        ed.WriteMessage("\nThe SS is bad!");
                }
                else
                    ed.WriteMessage("\nFence selection failed!");
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage(Environment.NewLine + ex.ToString());
            }
        }


        [CommandMethod("FencePolyline")]
        public void GetObjSelectPolyline()
        {
            Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;

            try
            {
                Point3dCollection pts = _Selection.ListVertices();
                PromptSelectionResult prSelRes = ed.SelectFence(pts);
                if (prSelRes.Status == PromptStatus.OK)
                {
                    SelectionSet ss = prSelRes.Value;
                    if (ss != null)
                        ed.WriteMessage("\nThe SS is good and has {0} entities.", ss.Count);
                    else
                        ed.WriteMessage("\nThe SS is bad!");
                }
                else
                    ed.WriteMessage("\nFence selection failed!");
            }
            catch(System.Exception ex) { }
        }
    }
}
