using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;


namespace five_lab_obebrabotka.Objects
{
     class Player : BaseObject
    {
        public float vX, vY;
        public Player(float x,float y, float angle) : base(x,y,angle)
        {
        }
        public Action<Marker> onMarkerOverlap;
        public Action<Krug> onKrugOverlap;
        public override void Render(Graphics g) // отрисовка
        {
            g.FillEllipse(new SolidBrush(Color.Yellow), -20, -20, 40, 40);  // заполнепние
            g.DrawEllipse(new Pen(Color.Blue,4), -20, -20, 40, 40); // obvodka
            g.DrawLine(new Pen(Color.Blue, 4), 0, 0, 30, 0); // стрлелка направления игрока
        }
        public override GraphicsPath GetGraphicsPath() // создаёт передвиджение
        {

          var path = base.GetGraphicsPath();
            path.AddEllipse(-10, -10, 20, 20);
            return path;

        }
        public override void  Overlap(BaseObject obj) // пересечение игрока c обьектом, в последсвтии удаляются, если не пересекётся
        {
            base.Overlap(obj);
            if(obj is Marker)
            {
                onMarkerOverlap(obj as Marker);
            }
            else if(obj is Krug krug)
            {
                onKrugOverlap.Invoke(krug);
            }
        }

    }
}
