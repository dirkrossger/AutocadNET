using System;
using System.Collections.Generic;

#region Autodesk
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
#endregion

namespace AcadNet
{
    class Select
    {
        public static List<Datas> Lines()
        {
            List<Datas> list = new List<Datas>();
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database acadDb = doc.Database;
            Editor ed = doc.Editor;

            TypedValue[] filterlist = new TypedValue[1];
            filterlist[0] = new TypedValue(0, "LINE");
            SelectionFilter filter = new SelectionFilter(filterlist);

            PromptSelectionResult psr = ed.SelectAll(filter);
            SelectionSet sset = null;
            //Entity ent = null;
            Line line = null;

            if (psr.Status == PromptStatus.OK)
            {
                sset = psr.Value;
            }

            using (Transaction tr = acadDb.TransactionManager.StartTransaction())
            {
                if (sset != null)
                {
                    for (int i = 0; i < sset.Count; i++)
                    {
                        try
                        {
                            line = tr.GetObject(sset[i].ObjectId, OpenMode.ForWrite) as Line;
                            if (line != null)
                            {
                                list.Add(new Datas { Position = line.StartPoint, Content = line.Layer });
                            }
                        }
                        catch (System.Exception ex)
                        {
                            ed.WriteMessage("\nËrror: " + ex);
                        }

                    }

                }
                tr.Commit();


                return list;
            }
        }
    }
}
