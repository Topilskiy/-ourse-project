using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SystAnalys_lr1
{
    public partial class Form1 : Form
    {
        public bool gaph = true;
        public List<int> color;
        public DrawGraph G;
        public List<Vertex> V;
        public List<Edge> E;
        public List<int> color_1;
        public DrawGraph G_1;
        public List<Vertex> V_1;
        public List<Edge> E_1;
        public int[,] AMatrix; //матрица смежности
        public int[,] IMatrix; //матрица инцидентности

        public int selected1; //выбранные вершины, для соединения линиями
        public int selected2;

        public Form1()
        {
            InitializeComponent();
            V = new List<Vertex>();
            G = new DrawGraph(sheet.Width, sheet.Height);
            E = new List<Edge>();
            color = new List<int>();
            color_1 = new List<int>();
            G_1 = new DrawGraph(sheet.Width, sheet.Height);
            V_1 = new List<Vertex>();
            E_1 = new List<Edge>();
            sheet.Image = G.GetBitmap();
        }

        internal Edge Edge
        {
            get => default(Edge);
            set
            {
            }
        }

        internal Vertex Vsada
        {
            get => default(Vertex);
            set
            {
            }
        }

        //кнопка - выбрать вершину
        public void selectButton_Click(object sender, EventArgs e)
        {
            selectButton.Enabled = false;
            drawVertexButton.Enabled = true;
            drawEdgeButton.Enabled = true;
            deleteButton.Enabled = true;
            if (gaph)
            {
                G.clearSheet();
                G.drawALLGraph(V, E);
                sheet.Image = G.GetBitmap();
            }
            else
            {
                G_1.clearSheet();
                G_1.drawALLGraph(V_1, E_1);
                sheet.Image = G_1.GetBitmap();
            }
            selected1 = -1;
        }

        //кнопка - рисовать вершину
        public void drawVertexButton_Click(object sender, EventArgs e)
        {
            drawVertexButton.Enabled = false;
            selectButton.Enabled = true;
            drawEdgeButton.Enabled = true;
            deleteButton.Enabled = true;
            if (gaph)
            {
                G.clearSheet();
                G.drawALLGraph(V, E);
                sheet.Image = G.GetBitmap();
            }
            else
            {
                G_1.clearSheet();
                G_1.drawALLGraph(V_1, E_1);
                sheet.Image = G_1.GetBitmap();
            }
        }

        //кнопка - рисовать ребро
        public void drawEdgeButton_Click(object sender, EventArgs e)
        {
            drawEdgeButton.Enabled = false;
            selectButton.Enabled = true;
            drawVertexButton.Enabled = true;
            deleteButton.Enabled = true;
            if (gaph)
            {
                G.clearSheet();
                G.drawALLGraph(V, E);
                sheet.Image = G.GetBitmap();
            }
            else
            {
                G_1.clearSheet();
                G_1.drawALLGraph(V_1, E_1);
                sheet.Image = G_1.GetBitmap();
            }
            selected1 = -1;
            selected2 = -1;
        }

        //кнопка - удалить элемент
        public void deleteButton_Click(object sender, EventArgs e)
        {
            deleteButton.Enabled = false;
            selectButton.Enabled = true;
            drawVertexButton.Enabled = true;
            drawEdgeButton.Enabled = true;
            if (gaph)
            {
                G.clearSheet();
                G.drawALLGraph(V, E);
                sheet.Image = G.GetBitmap();
            }
            else
            {
                G_1.clearSheet();
                G_1.drawALLGraph(V_1, E_1);
                sheet.Image = G_1.GetBitmap();
            }
        }

        //кнопка - удалить граф
        public void deleteALLButton_Click(object sender, EventArgs e)
        {
            selectButton.Enabled = true;
            drawVertexButton.Enabled = true;
            drawEdgeButton.Enabled = true;
            deleteButton.Enabled = true;
            const string message = "Вы действительно хотите полностью удалить граф?";
            const string caption = "Удаление";
            var MBSave = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (MBSave == DialogResult.Yes)
            {
                if (gaph)
                {
                    color.Clear();
                    V.Clear();
                    E.Clear();
                    G.clearSheet();
                    sheet.Image = G.GetBitmap();
                }
                else
                {
                    color_1.Clear();
                    V_1.Clear();
                    E_1.Clear();

                    G_1.clearSheet();
                    sheet.Image = G_1.GetBitmap();
                }
            }
        }

        //кнопка - матрица смежности
        public void buttonAdj_Click(object sender, EventArgs e)
        {
            createAdjAndOut(gaph);
        }

        //кнопка - матрица инцидентности 
        public void buttonInc_Click(object sender, EventArgs e)
        {
            createIncAndOut(gaph);
        }

        public void sheet_MouseClick(object sender, MouseEventArgs e)
        {
            //нажата кнопка "выбрать вершину", ищем степень вершины
            if (selectButton.Enabled == false)
            {
                if (gaph)
                {
                    for (int i = 0; i < V.Count; i++)
                    {
                        if (Math.Pow((V[i].x - e.X), 2) + Math.Pow((V[i].y - e.Y), 2) <= G.R * G.R)
                        {
                            if (selected1 != -1)
                            {
                                selected1 = -1;
                                G.clearSheet();
                                G.drawALLGraph(V, E);
                                sheet.Image = G.GetBitmap();
                            }
                            if (selected1 == -1)
                            {
                                G.drawSelectedVertex(V[i].x, V[i].y);
                                selected1 = i;
                                sheet.Image = G.GetBitmap();
                                createAdjAndOut(gaph);
                                listBoxMatrix.Items.Clear();
                                int degree = 0;
                                for (int j = 0; j < V.Count; j++)
                                    degree += AMatrix[selected1, j];
                                listBoxMatrix.Items.Add("Степень вершины №" + (selected1 + 1) + " равна " + degree);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < V_1.Count; i++)
                    {
                        if (Math.Pow((V_1[i].x - e.X), 2) + Math.Pow((V_1[i].y - e.Y), 2) <= G_1.R * G_1.R)
                        {
                            if (selected1 != -1)
                            {
                                selected1 = -1;
                                G_1.clearSheet();
                                G_1.drawALLGraph(V_1, E_1);
                                sheet.Image = G_1.GetBitmap();
                            }
                            if (selected1 == -1)
                            {
                                G_1.drawSelectedVertex(V_1[i].x, V_1[i].y);
                                selected1 = i;
                                sheet.Image = G_1.GetBitmap();
                                createAdjAndOut(gaph);
                                listBoxMatrix.Items.Clear();
                                int degree = 0;
                                for (int j = 0; j < V_1.Count; j++)
                                    degree += AMatrix[selected1, j];
                                listBoxMatrix.Items.Add("Степень вершины №" + (selected1 + 1) + " равна " + degree);
                                break;
                            }
                        }
                    }
                }
            }
            //нажата кнопка "рисовать вершину"
            if (drawVertexButton.Enabled == false)
            {
                if (gaph && V.Count < 20)
                {
                    color.Add(-1);
                    V.Add(new Vertex(e.X, e.Y));
                    G.drawVertex(e.X, e.Y, V.Count.ToString());
                    sheet.Image = G.GetBitmap();
                }
                else if (!gaph && V_1.Count < 20)
                {
                    color_1.Add(-1);
                    V_1.Add(new Vertex(e.X, e.Y));
                    G_1.drawVertex(e.X, e.Y, V.Count.ToString());
                    sheet.Image = G_1.GetBitmap();
                }
            }
            //нажата кнопка "рисовать ребро"
            if (drawEdgeButton.Enabled == false)
            {
                if (gaph)
                {
                    if (E.Count < 50)
                    {
                        if (e.Button == MouseButtons.Left)
                        {
                            for (int i = 0; i < V.Count; i++)
                            {
                                if (Math.Pow((V[i].x - e.X), 2) + Math.Pow((V[i].y - e.Y), 2) <= G.R * G.R)
                                {
                                    if (selected1 == -1)
                                    {
                                        G.drawSelectedVertex(V[i].x, V[i].y);
                                        selected1 = i;
                                        sheet.Image = G.GetBitmap();
                                        break;
                                    }
                                    if (selected2 == -1)
                                    {
                                        G.drawSelectedVertex(V[i].x, V[i].y);
                                        selected2 = i;
                                        E.Add(new Edge(selected1, selected2, dialog_menu()));
                                        G.drawEdge(V[selected1], V[selected2], E[E.Count - 1], E.Count - 1);
                                        selected1 = -1;
                                        selected2 = -1;
                                        sheet.Image = G.GetBitmap();
                                        break;
                                    }
                                }
                            }
                        }
                        if (e.Button == MouseButtons.Right)
                        {
                            if ((selected1 != -1) &&
                                (Math.Pow((V[selected1].x - e.X), 2) + Math.Pow((V[selected1].y - e.Y), 2) <= G.R * G.R))
                            {
                                G.drawVertex(V[selected1].x, V[selected1].y, (selected1 + 1).ToString());
                                selected1 = -1;
                                sheet.Image = G.GetBitmap();
                            }
                        }
                    }
                }
                else 
                {
                    if (E_1.Count < 50)
                    {
                        if (e.Button == MouseButtons.Left)
                        {
                            for (int i = 0; i < V_1.Count; i++)
                            {
                                if (Math.Pow((V_1[i].x - e.X), 2) + Math.Pow((V_1[i].y - e.Y), 2) <= G_1.R * G_1.R)
                                {
                                    if (selected1 == -1)
                                    {
                                        G_1.drawSelectedVertex(V_1[i].x, V_1[i].y);
                                        selected1 = i;
                                        sheet.Image = G_1.GetBitmap();
                                        break;
                                    }
                                    if (selected2 == -1)
                                    {
                                        G_1.drawSelectedVertex(V_1[i].x, V_1[i].y);
                                        selected2 = i;
                                        E_1.Add(new Edge(selected1, selected2, dialog_menu()));
                                        G_1.drawEdge(V_1[selected1], V_1[selected2], E_1[E_1.Count - 1], E_1.Count - 1);
                                        selected1 = -1;
                                        selected2 = -1;
                                        sheet.Image = G_1.GetBitmap();
                                        break;
                                    }
                                }
                            }
                        }
                        if (e.Button == MouseButtons.Right)
                        {
                            if ((selected1 != -1) &&
                                (Math.Pow((V_1[selected1].x - e.X), 2) + Math.Pow((V_1[selected1].y - e.Y), 2) <= G_1.R * G_1.R))
                            {
                                G_1.drawVertex(V_1[selected1].x, V_1[selected1].y, (selected1 + 1).ToString());
                                selected1 = -1;
                                sheet.Image = G_1.GetBitmap();
                            }
                        }
                    }
                }
            }
            //нажата кнопка "удалить элемент"
            if (deleteButton.Enabled == false)
            {
                if (gaph)
                {

                    bool flag = false; //удалили ли что-нибудь по ЭТОМУ клику
                                       //ищем, возможно была нажата вершина
                    for (int i = 0; i < V.Count; i++)
                    {
                        if (Math.Pow((V[i].x - e.X), 2) + Math.Pow((V[i].y - e.Y), 2) <= G.R * G.R)
                        {
                            for (int j = 0; j < E.Count; j++)
                            {
                                if ((E[j].v1 == i) || (E[j].v2 == i))
                                {
                                    E.RemoveAt(j);
                                    j--;
                                }
                                else
                                {
                                    if (E[j].v1 > i) E[j].v1--;
                                    if (E[j].v2 > i) E[j].v2--;
                                }
                            }
                            V.RemoveAt(i);
                            flag = true;
                            break;
                        }
                    }
                    //ищем, возможно было нажато ребро
                    if (!flag)
                    {
                        for (int i = 0; i < E.Count; i++)
                        {
                            if (E[i].v1 == E[i].v2) //если это петля
                            {
                                if ((Math.Pow((V[E[i].v1].x - G.R - e.X), 2) + Math.Pow((V[E[i].v1].y - G.R - e.Y), 2) <= ((G.R + 2) * (G.R + 2))) &&
                                    (Math.Pow((V[E[i].v1].x - G.R - e.X), 2) + Math.Pow((V[E[i].v1].y - G.R - e.Y), 2) >= ((G.R - 2) * (G.R - 2))))
                                {
                                    E.RemoveAt(i);
                                    flag = true;
                                    break;
                                }
                            }
                            else //не петля
                            {
                                if (((e.X - V[E[i].v1].x) * (V[E[i].v2].y - V[E[i].v1].y) / (V[E[i].v2].x - V[E[i].v1].x) + V[E[i].v1].y) <= (e.Y + 4) &&
                                    ((e.X - V[E[i].v1].x) * (V[E[i].v2].y - V[E[i].v1].y) / (V[E[i].v2].x - V[E[i].v1].x) + V[E[i].v1].y) >= (e.Y - 4))
                                {
                                    if ((V[E[i].v1].x <= V[E[i].v2].x && V[E[i].v1].x <= e.X && e.X <= V[E[i].v2].x) ||
                                        (V[E[i].v1].x >= V[E[i].v2].x && V[E[i].v1].x >= e.X && e.X >= V[E[i].v2].x))
                                    {
                                        E.RemoveAt(i);
                                        flag = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    //если что-то было удалено, то обновляем граф на экране
                    if (flag)
                    {
                        G.clearSheet();
                        if (gaph)
                        {
                            G.drawALLGraph(V, E);
                        }
                        else
                        {
                            G.drawALLGraph(V_1, E_1);
                        }
                        sheet.Image = G.GetBitmap();
                    }
                }
                else
                {
                    bool flag = false; //удалили ли что-нибудь по ЭТОМУ клику
                                       //ищем, возможно была нажата вершина
                    for (int i = 0; i < V_1.Count; i++)
                    {
                        if (Math.Pow((V_1[i].x - e.X), 2) + Math.Pow((V_1[i].y - e.Y), 2) <= G_1.R * G_1.R)
                        {
                            for (int j = 0; j < E_1.Count; j++)
                            {
                                if ((E_1[j].v1 == i) || (E_1[j].v2 == i))
                                {
                                    E_1.RemoveAt(j);
                                    j--;
                                }
                                else
                                {
                                    if (E_1[j].v1 > i) E_1[j].v1--;
                                    if (E_1[j].v2 > i) E_1[j].v2--;
                                }
                            }
                            V_1.RemoveAt(i);
                            flag = true;
                            break;
                        }
                    }
                    //ищем, возможно было нажато ребро
                    if (!flag)
                    {
                        for (int i = 0; i < E.Count; i++)
                        {
                            if (E[i].v1 == E[i].v2) //если это петля
                            {
                                if ((Math.Pow((V_1[E[i].v1].x - G_1.R - e.X), 2) + Math.Pow((V_1[E[i].v1].y - G_1.R - e.Y), 2) <= ((G_1.R + 2) * (G_1.R + 2))) &&
                                    (Math.Pow((V_1[E[i].v1].x - G_1.R - e.X), 2) + Math.Pow((V_1[E[i].v1].y - G_1.R - e.Y), 2) >= ((G_1.R - 2) * (G_1.R - 2))))
                                {
                                    E_1.RemoveAt(i);
                                    flag = true;
                                    break;
                                }
                            }
                            else //не петля
                            {
                                if (((e.X - V_1[E[i].v1].x) * (V_1[E[i].v2].y - V_1[E[i].v1].y) / (V_1[E[i].v2].x - V_1[E[i].v1].x) + V_1[E[i].v1].y) <= (e.Y + 4) &&
                                    ((e.X - V_1[E[i].v1].x) * (V_1[E[i].v2].y - V_1[E[i].v1].y) / (V_1[E[i].v2].x - V_1[E[i].v1].x) + V_1[E[i].v1].y) >= (e.Y - 4))
                                {
                                    if ((V_1[E[i].v1].x <= V_1[E[i].v2].x && V_1[E[i].v1].x <= e.X && e.X <= V_1[E[i].v2].x) ||
                                        (V_1[E[i].v1].x >= V_1[E[i].v2].x && V_1[E[i].v1].x >= e.X && e.X >= V_1[E[i].v2].x))
                                    {
                                        E_1.RemoveAt(i);
                                        flag = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    //если что-то было удалено, то обновляем граф на экране
                    if (flag)
                    {
                        G_1.clearSheet();
                        if (gaph)
                        {
                            G_1.drawALLGraph(V, E);
                        }
                        else
                        {
                            G_1.drawALLGraph(V_1, E_1);
                        }
                        sheet.Image = G_1.GetBitmap();
                    }
                }
            }
        }
        public int dialog_menu()
        {
            int waaaaas = 0;
            Dialog wasDialog;
            using (wasDialog = new Dialog())
            {
                if (wasDialog.ShowDialog() == DialogResult.OK)
                {
                    waaaaas = wasDialog.was;
                }
            }
            return waaaaas;
        }

        //создание матрицы смежности и вывод в листбокс
        public void createAdjAndOut(bool m_graph)
        {
            if (m_graph)
            {
                AMatrix = new int[V.Count, V.Count];
                G.fillAdjacencyMatrix(V.Count, E, AMatrix);
                listBoxMatrix.Items.Clear();
                string sOut = "    ";
                for (int i = 0; i < V.Count; i++)
                    sOut += (i + 1) + " ";
                listBoxMatrix.Items.Add(sOut);
                for (int i = 0; i < V.Count; i++)
                {
                    sOut = (i + 1) + " | ";
                    for (int j = 0; j < V.Count; j++)
                        sOut += AMatrix[i, j] + " ";
                    listBoxMatrix.Items.Add(sOut);
                }
            }
            else
            {
                AMatrix = new int[V_1.Count, V_1.Count];
                G_1.fillAdjacencyMatrix(V_1.Count, E_1, AMatrix);
                listBoxMatrix.Items.Clear();
                string sOut = "    ";
                for (int i = 0; i < V_1.Count; i++)
                    sOut += (i + 1) + " ";
                listBoxMatrix.Items.Add(sOut);
                for (int i = 0; i < V_1.Count; i++)
                {
                    sOut = (i + 1) + " | ";
                    for (int j = 0; j < V_1.Count; j++)
                        sOut += AMatrix[i, j] + " ";
                    listBoxMatrix.Items.Add(sOut);
                }
            }
        }

        //создание матрицы инцидентности и вывод в листбокс
        public void createIncAndOut(bool m_graph)
        {
            if (m_graph)
            {
                if (E.Count > 0)
                {
                    IMatrix = new int[V.Count, E.Count];
                    G.fillIncidenceMatrix(V.Count, E, IMatrix);
                    listBoxMatrix.Items.Clear();
                    string sOut = "    ";
                    for (int i = 0; i < E.Count; i++)
                        sOut += (char)('a' + i) + " ";
                    listBoxMatrix.Items.Add(sOut);
                    for (int i = 0; i < V.Count; i++)
                    {
                        sOut = (i + 1) + " | ";
                        for (int j = 0; j < E.Count; j++)
                            sOut += IMatrix[i, j] + " ";
                        listBoxMatrix.Items.Add(sOut);
                    }
                }
                else
                    listBoxMatrix.Items.Clear();
            }
            else
            {
                if (E_1.Count > 0)
                {
                    IMatrix = new int[V_1.Count, E_1.Count];
                    G_1.fillIncidenceMatrix(V_1.Count, E_1, IMatrix);
                    listBoxMatrix.Items.Clear();
                    string sOut = "    ";
                    for (int i = 0; i < E_1.Count; i++)
                        sOut += (char)('a' + i) + " ";
                    listBoxMatrix.Items.Add(sOut);
                    for (int i = 0; i < V_1.Count; i++)
                    {
                        sOut = (i + 1) + " | ";
                        for (int j = 0; j < E_1.Count; j++)
                            sOut += IMatrix[i, j] + " ";
                        listBoxMatrix.Items.Add(sOut);
                    }
                }
                else
                    listBoxMatrix.Items.Clear();
            }
        }

        //поиск элементарных цепей
        public void chainButton_Click(object sender, EventArgs e)
        {
            if (gaph)
            {
                listBoxMatrix.Items.Clear();
                //1-white 2-black
                int[] color = new int[V.Count];
                for (int i = 0; i < V.Count - 1; i++)
                    for (int j = i + 1; j < V.Count; j++)
                    {
                        for (int k = 0; k < V.Count; k++)
                                color[k] = 1;
                        DFSchain(i, j, E, color, (i + 1).ToString());
                        DFSchain(j-1, i+1, E_1, color, (i + 1).ToString());
                    }
            }
            else
            {
                listBoxMatrix.Items.Clear();
                //1-white 2-black
                int[] color = new int[V.Count];
                for (int i = 0; i < V.Count - 1; i++)
                    for (int j = 1; j < V.Count; j++)
                    {
                        for (int k = 0; k < V.Count; k++)
                                color[k] = 1;
                        DFSchain(i, j, E_1, color, (i + 1).ToString());
                    }
            }
        }

        //обход в глубину. поиск элементарных цепей. (1-white 2-black)

        public void DFSchain(int u, int endV, List<Edge> E, int[] color, string s)
        {
            //вершину не следует перекрашивать, если u == endV (возможно в нее есть несколько путей)
            if (u != endV)
                color[u] = 2;
            else
            {
                listBoxMatrix.Items.Add(s);
                return;
            }
            for (int w = 0; w < E.Count; w++)
            {
                if (color[E[w].v2] == 1 && E[w].v1 == u)
                {
                    DFSchain(E[w].v2, endV, E, color, s + "-" + (E[w].v2 + 1).ToString());
                    color[E[w].v2] = 1;
                }
                else if (color[E[w].v1] == 1 && E[w].v2 == u)
                {
                    DFSchain(E[w].v1, endV, E, color, (E[w].v1 + 1).ToString() + "-" + s);
                    //DFSchain(E[w].v1, endV, E, color, s + "-" + (E[w].v1 + 1).ToString());
                    color[E[w].v1] = 1;
                }
            }
        }

        public void cycleButton_Click(object sender, EventArgs e)
        {
            if (gaph)
            {
                listBoxMatrix.Items.Clear();
                //1-white 2-black
                int[] color = new int[V.Count];
                for (int i = 0; i < V.Count; i++)
                {
                    for (int k = 0; k < V.Count; k++)
                        color[k] = 1;
                    List<int> cycle = new List<int>();
                    cycle.Add(i + 1);
                    DFScycle(i, i, E, color, -1, cycle);
                }
            }
            else
            {
                listBoxMatrix.Items.Clear();
                //1-white 2-black
                int[] color = new int[V_1.Count];
                for (int i = 0; i < V_1.Count; i++)
                {
                    for (int k = 0; k < V_1.Count; k++)
                        color[k] = 1;
                    List<int> cycle = new List<int>();
                    cycle.Add(i + 1);
                    DFScycle(i, i, E_1, color, -1, cycle);
                }
            }
        }

        //обход в глубину. поиск элементарных циклов. (1-white 2-black)
        //Вершину, для которой ищем цикл, перекрашивать в черный не будем. Поэтому, для избежания неправильной
        //работы программы, введем переменную unavailableEdge, в которой будет хранится номер ребра, исключаемый
        //из рассмотрения при обходе графа. В действительности это необходимо только на первом уровне рекурсии,
        //чтобы избежать вывода некорректных циклов вида: 1-2-1, при наличии, например, всего двух вершин.

        public void DFScycle(int u, int endV, List<Edge> E, int[] color, int unavailableEdge, List<int> cycle)
        {
            //если u == endV, то эту вершину перекрашивать не нужно, иначе мы в нее не вернемся, а вернуться необходимо
            if (u != endV)
                color[u] = 2;
            else
            {
                if (cycle.Count >= 2)
                {
                    cycle.Reverse();
                    string s = cycle[0].ToString();
                    for (int i = 1; i < cycle.Count; i++)
                        s += "-" + cycle[i].ToString();
                    bool flag = false; //есть ли палиндром для этого цикла графа в листбоксе?
                    for (int i = 0; i < listBoxMatrix.Items.Count; i++)
                        if (listBoxMatrix.Items[i].ToString() == s)
                        {
                            flag = true;
                            break;
                        }
                    if (!flag)
                    {
                        cycle.Reverse();
                        s = cycle[0].ToString();
                        for (int i = 1; i < cycle.Count; i++)
                            s += "-" + cycle[i].ToString();
                        listBoxMatrix.Items.Add(s);
                    }
                    return;
                }
            }
            for (int w = 0; w < E.Count; w++)
            {
                if (w == unavailableEdge)
                    continue;
                if (color[E[w].v2] == 1 && E[w].v1 == u)
                {
                    List<int> cycleNEW = new List<int>(cycle);
                    cycleNEW.Add(E[w].v2 + 1);
                    DFScycle(E[w].v2, endV, E, color, w, cycleNEW);
                    color[E[w].v2] = 1;
                }
                //else if (color[E[w].v1] == 1 && E[w].v2 == u)
                //{
                //    List<int> cycleNEW = new List<int>(cycle);
                //    cycleNEW.Add(E[w].v1 + 1);
                //    DFScycle(E[w].v1, endV, E, color, w, cycleNEW);
                //    color[E[w].v1] = 1;
                //}
            }
        }

        public void saveButton_Click(object sender, EventArgs e)
        {
            if (sheet.Image != null)
            {
                SaveFileDialog savedialog = new SaveFileDialog();
                savedialog.Title = "Сохранить картинку как...";
                savedialog.OverwritePrompt = true;
                savedialog.CheckPathExists = true;
                savedialog.Filter = "Image Files(*.JPG)|*.JPG|All files (*.*)|*.*";
                savedialog.ShowHelp = true;
                if (savedialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        sheet.Image.Save(savedialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                    catch
                    {
                        MessageBox.Show("Невозможно сохранить изображение", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        public void button2_Click(object sender, EventArgs e)
        {
            gaph = !gaph;

            G.clearSheet();
            if (gaph)
            {
                G.drawALLGraph(V, E);
            }
            else
            {
                G.drawALLGraph(V_1, E_1);
            }

            sheet.Image = G.GetBitmap();
        }
        public void button3_Click(object sender, EventArgs e)
        {

            graphs a = new graphs();
            if(V.Count == 1 && V_1.Count == 1)
            {
                textBox1.Text = "Граф тревиален";
                return;
            }
            if(a.diamer(V.Count, E) == a.diamer(V_1.Count, E_1))
            {
                textBox1.Text = $"Графы равны по характеристике. Граф А = {a.diamer(V.Count, E)}, Граф B = {a.diamer(V_1.Count, E_1)}";
                return;
            }
            else
            {
                textBox1.Text = $"Графы не равны по характеристике. Граф А = {a.diamer(V.Count, E)}, Граф B = {a.diamer(V_1.Count, E_1)}";
                return;
            }
            ////1-white 2-black
            //int[] color = new int[V.Count];
            //for (int i = 0; i < V.Count - 1; i++)
            //    for (int j = i + 1; j < V.Count; j++)
            //    {
            //        for (int k = 0; k < V.Count; k++)
            //            color[k] = 1;
            //        DFSchain(i, j, E, color, (i + 1).ToString());
            //    }
        }
        public void button1_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Чтение графa...";
            openFileDialog.CheckPathExists = true;
            openFileDialog.Filter = "Text Files(*.TXT)|*.txt|All files (*.*)|*.*";
            openFileDialog.ShowHelp = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {

                using (StreamReader reader = new StreamReader(openFileDialog.FileName))
                {
                    string line;
                    List<List<int>> position1 = new List<List<int>>();
                    List<List<int>> position2 = new List<List<int>>();
                    List<int> mass_g1 = new List<int>();
                    List<int> mass_g2 = new List<int>();
                    while ((line = reader.ReadLine()) != null)
                    {
                        List<int> temp = new List<int>();
                        List<string> s_temp = new List<string>(line.Split(' '));
                        switch (s_temp[0])
                        {
                            case "G1":
                                s_temp.RemoveAt(0);
                                foreach (string s in s_temp)
                                {
                                    mass_g1.Add(int.Parse(s));
                                }
                                break;
                            case "G2":
                                s_temp.RemoveAt(0);
                                foreach (string s in s_temp)
                                {
                                    mass_g2.Add(int.Parse(s));
                                }
                                break;
                            case "POS1":
                                s_temp.RemoveAt(0);
                                foreach (string s in s_temp)
                                {
                                    temp.Add(int.Parse(s));
                                }
                                position1.Add(temp);
                                break;
                            case "POS2":
                                s_temp.RemoveAt(0);
                                foreach (string s in s_temp)
                                {
                                    temp.Add(int.Parse(s));
                                }
                                position2.Add(temp);
                                break;
                        }
                    }
                    V.Clear();
                    E.Clear();
                    color.Clear();
                    G.clearSheet();
                    V_1.Clear();
                    E_1.Clear();
                    color_1.Clear();
                    G_1.clearSheet();
                    foreach (List<int> p1 in position1)
                    {
                        color.Add(-1);
                        V.Add(new Vertex(p1[0], p1[1]));
                    }
                    foreach (List<int> p2 in position2)
                    {
                        color_1.Add(-1);
                        V_1.Add(new Vertex(p2[0], p2[1]));
                    }

                    int index = 0;
                    foreach (var i in mass_g1)
                    {
                        if(i == 0)
                        {
                            index++;
                        }
                        else
                        {
                            E.Add(new Edge(index, i-1));
                        }
                    }
                    index = 0;
                    foreach (var i in mass_g2)
                    {
                        if(i == 0)
                        {
                            index++;
                        }
                        else
                        {
                            E_1.Add(new Edge(index, i-1));
                        }
                    }
                    //int i = 0;
                    //foreach (int p in mass_p1)
                    //{
                    //    for (; i < p; i++)
                    //    {
                    //        E.Add(new Edge(index, mass_g1[i] - 1));
                    //    }
                    //    index++;
                    //}
                    //i = 0;
                    //index = 0;
                    //foreach (int p in mass_p2)
                    //{
                    //    for (; i < p; i++)
                    //    {
                    //        E_1.Add(new Edge(index, mass_g2[i] - 1));
                    //    }
                    //    index++;
                    //}

                    if (gaph)
                    {
                        G.drawALLGraph(V, E);
                        sheet.Image = G.GetBitmap();
                    }
                    else
                    {
                        G_1.drawALLGraph(V_1, E_1);
                        sheet.Image = G_1.GetBitmap();
                    }
                }
            }//
        }
    }
}