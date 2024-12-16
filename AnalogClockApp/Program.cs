using System;
using System.Drawing;
using System.Windows.Forms;

class AnalogClock : Form
{
    private Timer timer;

    public AnalogClock()
    {
        // Initialize the form
        this.Text = "Analog Clock";
        this.Width = 400;
        this.Height = 400;
        this.DoubleBuffered = true;

        // Initialize and start the timer
        timer = new Timer();
        timer.Interval = 1000; // 1 second
        timer.Tick += (s, e) => this.Invalidate(); // Redraw on each tick
        timer.Start();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        // Get the current time
        DateTime now = DateTime.Now;
        int hours = now.Hour % 12;
        int minutes = now.Minute;
        int seconds = now.Second;

        // Graphics setup
        Graphics g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        // Center and radius
        Point center = new Point(ClientSize.Width / 2, ClientSize.Height / 2);
        int radius = Math.Min(ClientSize.Width, ClientSize.Height) / 2 - 20;

        // Draw clock face
        g.DrawEllipse(Pens.Black, center.X - radius, center.Y - radius, radius * 2, radius * 2);

        // Draw clock numbers
        for (int i = 1; i <= 12; i++)
        {
            double angle = Math.PI / 6 * i; // 30 degrees per hour
            int x = center.X + (int)((radius - 20) * Math.Sin(angle));
            int y = center.Y - (int)((radius - 20) * Math.Cos(angle));
            string number = i.ToString();
            SizeF size = g.MeasureString(number, this.Font);
            g.DrawString(number, this.Font, Brushes.Black, x - size.Width / 2, y - size.Height / 2);
        }

        // Draw hour hand with numbers
        double hourAngle = (Math.PI / 6) * (hours + minutes / 60.0);
        DrawNumberHand(g, center, hourAngle, radius * 0.5f, hours.ToString());

        // Draw second hand with numbers
        double secondAngle = (Math.PI / 30) * seconds;
        DrawNumberHand(g, center, secondAngle, radius * 0.8f, seconds.ToString());

        // Draw clock center
        g.FillEllipse(Brushes.Black, center.X - 5, center.Y - 5, 10, 10);
    }

    private void DrawNumberHand(Graphics g, Point center, double angle, float length, string number)
    {
        int steps = 10; // Number of repetitions to form the hand
        float stepLength = length / steps; // Divide the hand length into small steps

        for (int i = 1; i <= steps; i++)
        {
            int x = center.X + (int)((stepLength * i) * Math.Sin(angle));
            int y = center.Y - (int)((stepLength * i) * Math.Cos(angle));
            SizeF size = g.MeasureString(number, this.Font);
            g.DrawString(number, this.Font, Brushes.Black, x - size.Width / 2, y - size.Height / 2);
        }
    }

    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.Run(new AnalogClock());
    }
}
