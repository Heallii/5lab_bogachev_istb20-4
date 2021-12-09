using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using five_lab_obebrabotka.Objects;


namespace five_lab_obebrabotka
{
    public partial class Form1 : Form
    {
       
        List<BaseObject> objects = new List<BaseObject>();   // что бы обьекты отрисовывались
        Player player; // ссылки на обьекты 23
        Marker marker;
        Krug krug;
        Krug krug1;
        int scores = 0;
        Random r = new Random();
        public Form1()
        {
            InitializeComponent();
            player = new Player(pbMain.Width / 2, pbMain.Height / 2, 0);  // плэйер присоеденяется к классу плэйер 
            // добавляю реакцию на пересечение
            player.onOverlap += (p, obj) =>
            {
                txtLog.Text = $"[{DateTime.Now:HH:mm:ss:ff}] Игрок пересекся с {obj}\n" + txtLog.Text;  // делегаты    событие - это момент, который фиксируется после происхождения работы программы, обработки
            };
            player.onMarkerOverlap += (m) =>
            {
                objects.Remove(m); // делегат на проверку маркера пересечения с игроком
                marker = null;
            };
            player.onKrugOverlap += (m) =>
            {
                GenerateKRUSHOK(m); // генерация кружков
               
                scores++;
                label1.Text = $"Счёт: "+ scores; //  cчетчик очков пересекших

            };
            marker  = new Marker(pbMain.Width / 2+50, pbMain.Height / 2+50, 0); // создание новый элмент в списке, который отвечает за генерацию маркера при запуске программы 50пикс
          
       

          
          
            krug = new Krug(0, 0,0); // новый элменет в списке
            GenerateKRUSHOK(krug);
            krug1 = new Krug(0, 0, 0);
            GenerateKRUSHOK(krug1); 
            objects.Add(krug); // вывод на форму
            objects.Add(krug1);
            objects.Add(marker);
            objects.Add(player);

        }
       
        private void GenerateKRUSHOK(Krug krushok)  // генерация кружка на форме в ранд месте 
        {
            Random random = new Random();
            krushok.X = random.Next() % 600 + 40;
            krushok.Y = random.Next() % 300 + 40;

        }
        private void pbMain_Paint(object sender, PaintEventArgs e)  // передвежение обьекта и рендер обьектов
        {
            var g = e.Graphics;
            g.Clear(Color.Aquamarine);
            peredvig();
            // пересчитываем пересечения
            foreach (var obj in objects.ToList())
            {
                if (obj != player && player.Overlaps(obj, g))
                {
                    player.Overlap(obj);
                    obj.Overlap(player);
                }
               
            }

            // рендерим объекты
            foreach (var obj in objects)
            {
                g.Transform = obj.GetTransform();
                obj.Render(g);
            }

        }
        private void peredvig() // плавное передвиг
        {
            if (marker != null)
            {
                float dx = marker.X - player.X;
                float dy = marker.Y - player.Y;

                float lenght = MathF.Sqrt(dx * dx + dy * dy);
                dx /= lenght;
                dy /= lenght;

                player.vX += dx * 0.8f;
                player.vY += dy * 0.8f;
                player.Angle = 90 - MathF.Atan2(player.vX, player.vY) * 180 / MathF.PI;

            }
            player.vX += -player.vX * 0.1f;
            player.vY += -player.vY * 0.1f;

            // пересчет позиция игрока с помощью вектора скорости
            player.X += player.vX;
            player.Y += player.vY;
        }
        private void timer1_Tick(object sender, EventArgs e)  // время жизни кружочка
        {
            foreach (var obj in objects.ToList())
            {
                if (obj is Krug krug)
                {
                    krug.time--;

                    if (krug.time <= 0)
                    {
                        GenerateKRUSHOK(krug);
                        krug.time = 130 + r.Next() % 70;
                    }
                }

            }
            pbMain.Invalidate();
        }

        private void pbMain_MouseClick(object sender, MouseEventArgs e)  // маркер отрисовка
        {
           if (marker == null)
            {
               marker = new Marker(0, 0, 0);
                objects.Add(marker); 
           } 
            marker.X = e.X;
            marker.Y = e.Y;
        }

        private void pbMain_Click(object sender, EventArgs e)
        {

        }

        private void pbMain_Click_1(object sender, EventArgs e)
        {

        }

        private void txtLog_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
