using System;
using System.Collections.Generic;

#region Autodesk
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using acadApp = Autodesk.AutoCAD.ApplicationServices.Application;
#endregion


namespace SelectionFence
{
    class _Selection
    {
        public static Point3dCollection ListVertices()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;

            Point3dCollection result = new Point3dCollection();

            PromptEntityResult per = ed.GetEntity("Select a polyline");
            if (per.Status == PromptStatus.OK)
            {
                Transaction tr = db.TransactionManager.StartTransaction();
                using (tr)
                {
                    DBObject obj = tr.GetObject(per.ObjectId, OpenMode.ForRead);
                    Polyline lwp = obj as Polyline;

                    if (lwp != null)
                    {
                        // Use a for loop to get each vertex, one by one
                        int vn = lwp.NumberOfVertices;
                        for (int i = 0; i < vn; i++)
                        {
                            // Could also get the 3D point here
                            Point2d pt = lwp.GetPoint2dAt(i);
                            result.Add(new Point3d(pt.X, pt.Y, 0));
                        }
                    }
                    else
                    {
                        // If an old-style, 2D polyline
                        Polyline2d p2d = obj as Polyline2d;
                        if (p2d != null)
                        {
                            // Use foreach to get each contained vertex
                            foreach (ObjectId vId in p2d)
                            {
                                Vertex2d v2d = (Vertex2d)tr.GetObject(vId, OpenMode.ForRead);
                                result.Add(new Point3d(v2d.Position.X, v2d.Position.Y, v2d.Position.Z));
                            }
                        }
                        else
                        {
                            // If an old-style, 3D polyline
                            Polyline3d p3d = obj as Polyline3d;
                            if (p3d != null)
                            {
                                // Use foreach to get each contained vertex
                                foreach (ObjectId vId in p3d)
                                {
                                    PolylineVertex3d v3d = (PolylineVertex3d)tr.GetObject(vId, OpenMode.ForRead);
                                    result.Add(new Point3d(v3d.Position.X, v3d.Position.Y, v3d.Position.Z));
                                }
                            }
                        }
                    }
                    tr.Commit();
                }
            }
            return result;
        }
    }
}
