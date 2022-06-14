using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystAnalys_lr1
{
    public class nodes
    {
        public List<int> v2;
        public List<int> wes;

        public nodes()
        {
            this.v2 = new List<int>();
            this.wes = new List<int>();
        }

        public void Add_edge(Edge edge)
        {
            this.v2.Add(edge.v2);
            this.wes.Add(edge.wes);
        }

        public void Add_edge(int v2, int wes)
        {
            this.v2.Add(v2);
            this.wes.Add(wes);
        }
    }
    public class graphs
    {
        public List<nodes> node;
        public List<int> was_edge;
        public List<string> paths;
        public int[,] color;
        public int V_count;
        public int diam;
        public int diam_p;

        public void printAllPaths(int s, int d)
        {
            bool[] isVisited = new bool[V_count];
            List<int> pathList = new List<int>();
            
            pathList.Add(s);
            
            printAllPathsUtil(s, d, isVisited, pathList);
        }
        public void printAllPathsUtil(int u, int d, bool[] isVisited, List<int> localPathList, int wes = 0)
        {
            if (u == d)
            {
                if (localPathList.Count <= 1)
                    return; 

                was_edge.Add(wes);
                paths.Add(string.Join(" ", localPathList));
                return;
            }
            
            isVisited[u] = true;
            
            for(int t = 0; t < node[u].v2.Count; t++)
            {
                int i = node[u].v2[t];
                if (!isVisited[i])
                { 
                    localPathList.Add(i);
                    printAllPathsUtil(i, d, isVisited,localPathList, node[u].wes[t] + wes);
                    localPathList.Remove(i);
                }
            }

            // Mark the current node
            isVisited[u] = false;
        }
        public int diamer(int V_count, List<Edge> E)
        {
            //paths = new List<List<int>>();
            this.V_count = V_count;
            List<List<List<int>>> node_all_paths = new List<List<List<int>>>();
            paths = new List<string>();
            was_edge = new List<int>();
            color = new int[V_count, V_count];
            node = new List<nodes>();
            for (int i = 0; i < V_count; i++)
            {
                nodes n = new nodes();
                node.Add(n);
            }
            foreach (Edge e in E)
            {
                node[e.v1].Add_edge(e);
            }

            for (int i = 0; i < V_count; i++)
                for (int t = 0; t < V_count; t++)
                {
                    printAllPaths(i, t);
                }


            List<List<int>> paths_int = new List<List<int>>();
            List<int> max = new List<int>();
            diam_p = 0;
            int index = 0;
            foreach (string i in paths)
            {
                string[] s_temp = i.Split(' ');
                List<int> int_temp = new List<int>();
                foreach (string a in s_temp)
                {
                    int_temp.Add(int.Parse(a));
                }
                paths_int.Add(int_temp);
                if (diam_p < was_edge[index])
                {
                    diam_p = was_edge[index];
                    max = int_temp;
                }
                index++;
            }

            int[,] count_int_mass = new int[V_count, V_count];
            int[,] wes_int_mass = new int[V_count, V_count];
            bool[,] diam_plus = new bool[V_count, V_count];
            int[,] size_mass = new int[V_count, V_count];

            for (int i = 0; i < V_count; i++)
                for (int x = 0; x < V_count; x++)
                {
                    count_int_mass[i, x] = int.MaxValue;
                    wes_int_mass[i, x] = int.MaxValue;
                }

            for (int ind = 0; ind < paths_int.Count(); ind++)
            {
                List<int> i = paths_int[ind];
                //size_mass[i[0], i[i.Count - 1]]++;
                if (count_int_mass[i[0], i[i.Count - 1]] >= i.Count && wes_int_mass[i[0], i[i.Count - 1]] >= was_edge[ind])
                {
                    count_int_mass[i[0], i[i.Count - 1]] = i.Count;
                    wes_int_mass[i[0], i[i.Count - 1]] = was_edge[ind];
                }
            }

            foreach (int i in wes_int_mass)
            {
                if (diam < i && i != int.MaxValue)
                {
                    diam = i;
                }
            }

            
            for (int ind = 0; ind < paths_int.Count(); ind++)
            {
                List<int> i = paths_int[ind];

                if(was_edge[ind] > diam)
                {
                    diam_plus[i[0], i[i.Count - 1]] = true;
                }

                if (was_edge[ind] <= diam)
                {
                    size_mass[i[0], i[i.Count - 1]]++;
                }
            }


            int temp_max = int.MaxValue;
            int i_1 = 0, i_2 = 0;
            for (int i = 0; i < V_count; i++)
                for (int t = 0; t < V_count; t++)
                    if (temp_max > size_mass[i, t] && size_mass[i, t] != 0 && diam_plus[i,t])
                    {
                        temp_max = size_mass[i, t];
                        i_1 = i;
                        i_2 = t;
                    }

            if (diam < diam_p) 
            {
                return size_mass[i_1, i_2];//size_mass[max[0], max[max.Count - 1]];
            }

            return 0;
        }

        //public int chainButton_Click(int V_count, List<Edge> E)
        //{
        //    paths.Clear();
        //    int[,] color = new int[V_count, V_count];
        //    for (int i = 0; i < V_count - 1; i++)
        //        for (int j = i + 1; j < V_count; j++)
        //        {
        //            for (int k = 0; k < V_count; k++)
        //                for (int t = 0; t < V_count; t++)
        //                    color[k, t] = 1;
        //            //List<int> s = new List<int>();
        //            //s.Add(i + 1);
        //            //DFSchain(i, j, E, color, (i + 1).ToString(), 0);
        //        }

        //    List<List<int>> paths_int = new List<List<int>>();
        //    List<int> max = new List<int>();
        //    diam_p = 0;
        //    int index = 0;
        //    foreach (string i in paths)
        //    {
        //        string[] s_temp = i.Split(' ');
        //        List<int> int_temp = new List<int>();
        //        foreach (string a in s_temp)
        //        {
        //            int_temp.Add(int.Parse(a));
        //        }
        //        paths_int.Add(int_temp);
        //        if (diam_p < was_edge[index])
        //        {
        //            diam_p = was_edge[index];
        //            max = int_temp;
        //        }
        //        index++;
        //    }
        //    

        //    for (int ind = 0; ind < paths_int.Count(); ind++)
        //    {
        //        List<int> i = paths_int[ind];
        //        if (was_edge[ind] > diam)
        //        {
        //            size_mass[i[0] - 1, i[i.Count - 1] - 1]--;
        //        }
        //    }

        //    if (diam < diam_p)
        //    {
        //        return size_mass[max[0] - 1, max[max.Count - 1] - 1];
        //    }
        //    return 0;
        //}

       // public void DFSchain(int u, int endV, List<Edge> E, int[,] color, string s, int wess)//List<int> s)
       // {
       //     //вершину не следует перекрашивать, если u == endV (возможно в нее есть несколько путей)
       //     if (u != endV)
       //         color[u, 0] = 2;
       //     else
       //     {
       //         was_edge.Add(wess);
       //         paths.Add(s);
       //         return;
       //     }
       //     for (int w = 0; w < E.Count; w++)
       //     {
       //         if (color[E[w].v2, 0] == 1 && E[w].v1 == u && !E[w].to)
       //         {
       //             DFSchain(E[w].v2, endV, E, color, s + "-" + (E[w].v2 + 1).ToString(), E[w].wes + wess);
       //             color[E[w].v2, 0] = 1;
       //         }
       //         else if (color[E[w].v1, 0] == 1 && E[w].v2 == u && E[w].to)
       //         {
       //             DFSchain(E[w].v1, endV, E, color, (E[w].v1 + 1).ToString() + "-" + s, E[w].wes + wess);
       //             color[E[w].v1, 0] = 1;
       //         }
       //         //if (!E[w].to)
       //         //{
       //         //    if (color[E[w].v1, E[w].v2] == 1 && E[w].v1 == u)
       //         //    {
       //         //        DFSchain(E[w].v2, endV, E, color, s + " " + (E[w].v2 + 1).ToString(), E[w].wes + wess);
       //         //    }
       //         //}
       //         //else
       //         //{
       //         //    if (color[E[w].v1, E[w].v2] == 1 && E[w].v1 == u)
       //         //        DFSchain(E[w].v2, endV, E, color, (E[w].v1 + 1).ToString() + " " + s, E[w].wes + wess);
       //         //}
       //         //color[E[w].v1, E[w].v2] = 1;
       //     }
       // }
    }
}