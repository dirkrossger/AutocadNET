using System;
#region Autodesk
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
#endregion

namespace AcadNet
{
    class Mleader
    {
        public static void Create(Point3d flagPosition, string contents)
        {

            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            using (Transaction Tx = db.TransactionManager.StartTransaction())
            {
                BlockTable table = Tx.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord model = Tx.GetObject(table[BlockTableRecord.ModelSpace], OpenMode.ForWrite)
                as BlockTableRecord;

                MLeader leader = new MLeader();
                leader.SetDatabaseDefaults();

                leader.ContentType = ContentType.MTextContent;

                MText mText = new MText();
                mText.SetDatabaseDefaults();
                //mText.Width = 100;
                //mText.Height = 50;
                mText.SetContentsRtf(contents);
                
                mText.Location = new Point3d(flagPosition[0]+1, flagPosition[1] + 1, 0); 

                leader.MText = mText;

                int idx = leader.AddLeaderLine(flagPosition);
                leader.AddFirstVertex(idx, flagPosition);

                model.AppendEntity(leader);
                Tx.AddNewlyCreatedDBObject(leader, true);

                Tx.Commit();
            }

        }
    }
}
