using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NanogramSolver
{
    public class Nanogram
    {
        private int r, c;
        public int[,] board;
        private List<int[]> row;
        private List<int[]> col;
        private bool[] isRowSolved;
        private bool[] isColSolved;
        private List<int[,]> solutions;
        private string filepath;
        private bool finishedSolving;
        public bool allSolutions = false;
        private bool isUnsolvable = false;
        public Nanogram(string filepath)
        {
            finishedSolving = false;
            this.filepath = filepath;
            using (System.IO.StreamReader file = new System.IO.StreamReader(filepath)  )
            {
                row = new List<int[]>();
                col = new List<int[]>();
                string temp;
                temp = file.ReadLine();
                int[] bits = temp.Split(new char[] { ',', ' ', ';' ,'\t'}, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                r = bits[0];
                c = bits[1];
                board = new int[r, c];
                isRowSolved = new bool[r];
                isColSolved = new bool[c];
                for (int i = 0; i < r; i++)
                {
                    temp = file.ReadLine();
                    while(String.IsNullOrWhiteSpace(temp))
                        temp = file.ReadLine();
                    bits = temp.Split(new char[] { ',', ' ', ';', '\t' },StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                    row.Add(bits);
                    isRowSolved[i] = false;
                }
                for (int i = 0; i < c; i++)
                {
                    temp = file.ReadLine();
                    while (String.IsNullOrWhiteSpace(temp))
                        temp = file.ReadLine();
                    bits = temp.Split(new char[]{ ',',' ',';', '\t' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                    col.Add(bits);
                    isColSolved[i] = false;
                }


            }

            for (int i = 0; i < r; i++)
            {
                for (int j = 0; j < c; j++)
                {
                    board[i,j] = 2;
                }
            }
        }

        private int GetBoardState()
        {
            int sum = 0;
            for (int i = 0; i < r; i++)
            {
                for (int j = 0; j < c; j++)
                {
                    sum += board[i, j];
                }
            }
            return sum;
        }

        public bool FinishedSolving()
        {
            return finishedSolving;
        }

        public List<int[,]> GetAllSolutions()
        {
            return solutions;
        }

        //For guessing row only, DON'T USE
        public void Overwrite(int[,] newBoard,bool[] newIsRowSolved,bool [] newIsColSolved)
        {
            for (int i = 0; i < r; i++)
            {
                isRowSolved[i] = newIsRowSolved[i];
                for (int j = 0; j < c; j++)
                {
                    board[i, j] = newBoard[i,j];
                    isColSolved[j] = newIsColSolved[j];
                }
            }
        }

        public int GetNumberOfRows()
        {
            return r;
        }

        public int GetNumberOfColumns()
        {
            return c;
        }

        //public void FillRow(int x, bool row)
        //{
        //    int balls;
        //    int[] line;
        //    int[] lineNumbers;
        //    int[] boxes;
        //    int[] lineCounter;
        //    if (row)
        //    {
        //        balls = c + 1;
        //        boxes = new int[c];
        //        lineCounter = new int[c];
        //        line = new int[c];
        //        foreach (int i in this.row[x])
        //            balls -= (i + 1);
        //        for (int i = 0; i < c; i++)
        //            line[i] = board[x, i];
        //        lineNumbers = this.row[x];
        //        for (int j = 0; j < c; j++)
        //        {
        //            boxes[j] = 0;
        //            lineCounter[j] = 0;
        //        }
        //    }

        //    else
        //    {
        //        balls = r + 1;
        //        boxes = new int[r];
        //        lineCounter = new int[r];
        //        line = new int[r];
        //        foreach (int i in col[x])
        //            balls -= (i + 1);
        //        for (int i = 0; i < r; i++)
        //            line[i] = board[i,x];
        //        lineNumbers = col[x];
        //        for (int j = 0; j < r; j++)
        //        {
        //            boxes[j] = 0;
        //            lineCounter[j] = 0;
        //        }
        //    }
        //    int counter = 0;
        //    GuessPossibleRows(0, 0, balls, boxes, ref counter, lineCounter,line,lineNumbers);
        //    if (row)
        //    {
        //        for (int j = 0; j < c; j++)
        //            if (lineCounter[j] == 0)
        //                board[x, j] = 0;
        //            else if (lineCounter[j] == counter)
        //                board[x, j] = 1;
        //    }

        //    else
        //    {
        //        for (int j = 0; j < r; j++)
        //            if (lineCounter[j] == 0)
        //                board[j,x] = 0;
        //            else if (lineCounter[j] == counter)
        //                board[j,x] = 1;
        //    }

        //}

        //void GuessPossibleRows(int place, int position, int balls, int[] boxes, ref  int counter, int[] rowCounter, int[] boardRow, int[] numbersRow)
        //{
        //    if (place < numbersRow.Length)
        //    {
        //        if (place != 0)
        //            if (boardRow[position] == 1)
        //                return;
        //            else
        //            {
        //                boxes[position] = 0;
        //                position++;
        //            }

        //        for (int i = 0; i <= balls; i++)
        //        {
        //            for (int j = 0; j < i; j++)
        //                if (boardRow[position + j] == 1)
        //                    return;
        //                else
        //                    boxes[position + j] = 0;

        //            for (int j = 0; j < numbersRow[place]; j++)
        //                if (boardRow[position + i + j] == 0)
        //                    goto continueOuterLoop;
        //                else
        //                    boxes[position + i + j] = 1;

        //            GuessPossibleRows(place + 1, position + i + numbersRow[place], balls - i, boxes, ref counter, rowCounter, boardRow, numbersRow);
        //            continueOuterLoop:;
        //        }
        //    }
        //    else
        //    {
        //        for (int j = 0; j < balls; j++)
        //            if (boardRow[position + j] == 1)
        //                return;
        //            else
        //                boxes[position + j] = 0;
        //        for (int i = 0; i < rowCounter.Length; i++)
        //            rowCounter[i] += boxes[i];
        //        counter++;
        //    }
        //}

        private void FillLine(int x, bool isRow,bool isGuessing)
        {
            int cr = isRow ? c : r;

            int[] line = new int[cr];
            int[] lineNumbers;
            if (isRow)
            {
                for (int i = 0; i < c; i++)
                    line[i] = board[x, i];
                lineNumbers = row[x];
            }
            else
            {
                for (int i = 0; i < r; i++)
                    line[i] = board[i, x];
                lineNumbers = col[x];
            }
            int emptySpaces = cr + 1;
            int[] possibleSolution = new int[cr];
            possibleSolution.Fill(0);
            int[] possibleSolutionOverlap = new int[cr];
            possibleSolutionOverlap.Fill(0);
            foreach (int i in lineNumbers)
                emptySpaces -= (i + 1);
            int[] A = new int[lineNumbers.Length+1];
            A.Fill(0);

            int possibleSolutionCounter = 0;
            int j = 0;
            int posInLine = 0;
            bool zero = true;
            while (j >= 0)
            {
                if (j > (lineNumbers.Length - 1))
                {
                    for (int z = 0; z < emptySpaces; z++)
                    {
                        if (line[posInLine + z] != 1)
                        {
                            possibleSolution[posInLine + z] = 0;
                        }
                        else
                        {
                            goto skip;
                        }
                    }
                    for (int i = 0; i < possibleSolutionOverlap.Length; i++)
                        possibleSolutionOverlap[i] += possibleSolution[i];
                    possibleSolutionCounter++;
                    if (isGuessing)
                    {
                        Nanogram n = new Nanogram(filepath);
                        if (isRow)
                            isRowSolved[x] = true;
                        else
                            isColSolved[x] = true;
                        n.Overwrite(board, isRowSolved, isColSolved);
                        if (isRow)
                            for (int z = 0; z < c; z++)
                                n.board[x,z] = possibleSolution[z];
                        else
                            for (int z = 0; z < r; z++)
                                n.board[z, x] = possibleSolution[z];
                        if (allSolutions == false)
                            n.allSolutions = false;
                        if (n.Solve())
                        {
                           foreach (int[,] b in n.GetAllSolutions())
                            {
                                solutions.Add(b);
                                if (allSolutions == false)
                                    return;
                            }

                        }
                    }
                    skip:;
                    j--;
                    zero = false;
                    posInLine -= lineNumbers[j] + A[j];
                }
                if (!zero)
                {
                    A[j]++;
                    emptySpaces--;
                }
                if (emptySpaces < 0)
                {
                    emptySpaces += A[j];
                    A[j] = 0;
                    j--;
                    zero = false;
                    if(j>=0)
                    posInLine -= lineNumbers[j] + A[j] + 1;
                }
                else
                {
                    zero = false;
                    for (int z = 0; z < A[j]; z++)
                    {
                        if (line[posInLine + z] != 1)
                        {
                            possibleSolution[posInLine + z] = 0;
                        }
                        else
                        {
                            goto skipJ;
                        }
                    }
                    posInLine += A[j];
                    for (int z = 0; z < lineNumbers[j]; z++)
                    {
                        if (line[posInLine+z] != 0)
                        {
                            possibleSolution[posInLine+z] = 1;
                        }
                        else
                        {
                            posInLine -= A[j];
                            goto skipJ;
                        }
                    }
                    if (j != lineNumbers.Length - 1)
                    {
                        if (line[posInLine + lineNumbers[j]] != 1)
                        {
                            possibleSolution[posInLine + lineNumbers[j]] = 0;                 

                        }
                        else
                        {
                            posInLine -= A[j];
                            goto skipJ;
                        }
                    }
                    else
                    {
                        posInLine--;
                    }
                    zero = true;
                    posInLine += lineNumbers[j] + 1;
                    j++;            
                    skipJ:;
                }

            }
            if(possibleSolutionCounter > 0)
            {
                if (isRow)
                {
                    for (int z = 0; z < c; z++)
                        if (possibleSolutionOverlap[z] == 0)
                            board[x, z] = 0;
                        else if (possibleSolutionOverlap[z] == possibleSolutionCounter)
                            board[x, z] = 1;
                    if (possibleSolutionCounter == 1)
                        isRowSolved[x] = true;
                }
                else
                {
                    for (int z = 0; z < r; z++)
                        if (possibleSolutionOverlap[z] == 0)
                            board[z, x] = 0;
                        else if (possibleSolutionOverlap[z] == possibleSolutionCounter)
                            board[z, x] = 1;
                    if (possibleSolutionCounter == 1)
                        isColSolved[x] = true;
                }
            }
            else
            {
                isUnsolvable = true;
            }

           
        }

        public bool SolveFullOnePass()
        {          
            int boardBefore = GetBoardState();
            var allTasks = new List<Task>();
            for (int i = 0; i < r; i++)
            {
                if (!isRowSolved[i])
                {
                    int temp = i;
                    Task t = new Task(() => FillLine(temp, true, false));
                    t.Start();
                    allTasks.Add(t);
                }
            }
            Task.WaitAll(allTasks.ToArray());
            allTasks.Clear();
            for (int i = 0; i < c; i++)
            {
                if (!isColSolved[i])
                {
                    int temp = i;
                    Task t = new Task(() => FillLine(temp, false, false));
                    t.Start();
                    allTasks.Add(t);
                }
            }
            Task.WaitAll(allTasks.ToArray());
            finishedSolving = true;
            for (int i = 0; i < r; i++)
                if (isRowSolved[i] == false)
                    finishedSolving = false;
            for (int i = 0; i < c; i++)
                if (isColSolved[i] == false)
                    finishedSolving = false;
            return boardBefore != GetBoardState();
        }

        public bool SolveLeftRigthOnePass()
        {
            int boardBefore = GetBoardState();
            var allTasks = new List<Task>();
            for (int i = 0; i < r; i++)
            {
                if (!isRowSolved[i])
                {
                    int temp = i;
                    Task t = new Task(() => LeftRightLineFill(temp, true));
                    t.Start();
                    allTasks.Add(t);
                }

            }
            Task.WaitAll(allTasks.ToArray());
            allTasks.Clear();
            for (int i = 0; i < c; i++)
            {
                if (!isColSolved[i])
                {
                    int temp = i;
                    Task t = new Task(() => LeftRightLineFill(temp, false));
                    t.Start();
                    allTasks.Add(t);
                }
            }
            Task.WaitAll(allTasks.ToArray());

            return boardBefore != GetBoardState();
        }

        private void TryGuessing()
        {     
            for (int i = 0; i < r; i++)
            {
                if (isRowSolved[i] == false)
                {
                    FillLine(i, true, true);
                    break;
                }
            }
        }

        public bool Solve()
        {
            if (!finishedSolving)
            {
                solutions = new List<int[,]>();
                while ((SolveLeftRigthOnePass()|| SolveFullOnePass())&&!isUnsolvable) ;
                finishedSolving = true;
                for (int i = 0; i < r; i++)
                    if (isRowSolved[i] == false)
                        finishedSolving = false;
                for (int i = 0; i < c; i++)
                    if (isColSolved[i] == false)
                        finishedSolving = false;
                if (isUnsolvable)
                {
                    finishedSolving = true;
                    return solutions.Any();
                }

                if (finishedSolving)
                {
                    solutions.Add(board);
                }
                else
                {
                    TryGuessing();
                }
                finishedSolving = true;
            }
            return solutions.Any();
        }

        //Solves and return true if is solvable, returns false otherwise
        public bool SolveAsMuchPossibleWithoutGuessing()
        {
            if (!finishedSolving)
            {
                while (SolveLeftRigthOnePass() || SolveFullOnePass()) ;
                finishedSolving = true;
                for (int i = 0; i < r; i++)
                    if (isRowSolved[i] == false)
                        finishedSolving = false;
                for (int i = 0; i < c; i++)
                    if (isColSolved[i] == false)
                        finishedSolving = false;
                if (finishedSolving)
                {
                    solutions = new List<int[,]>
                    {
                        board
                    };
                }
                return finishedSolving;
            }
            return solutions.Any();
            
        }

        public List<int[]> GetRows()
        {
            return row;
        }
        public List<int[]> GetCols()
        {
            return col;

        }

        private int[] GetLeftMostPossibleSolution(int x, bool isRow)
        {
            int cr = isRow ? c : r;
            int[] line = new int[cr];
            int[] lineNumbers;
            if (isRow)
            {
                for (int i = 0; i < c; i++)
                    line[i] = board[x, i];
                lineNumbers = row[x];
            }
            else
            {
                for (int i = 0; i < r; i++)
                    line[i] = board[i, x];
                lineNumbers = col[x];
            }
            int emptySpaces = cr + 1;
            int[] possibleSolution = new int[cr];
            foreach (int i in lineNumbers)
                emptySpaces -= (i + 1);

            int[] A = new int[lineNumbers.Length + 1];
            A.Fill(0);
            int j = 0;
            int posInLine = 0;
            bool zero = true;
            while (j >= 0)
            {
                if (j > (lineNumbers.Length - 1))
                {
                    for (int z = 0; z < emptySpaces; z++)
                    {
                        if (line[posInLine + z] != 1)
                        {
                            possibleSolution[posInLine + z] = 0;
                        }
                        else
                        {
                            goto skip;
                        }
                    }
                    possibleSolution.NumberArray();
                    return possibleSolution;
                    skip:;
                    j--;
                    zero = false;
                    posInLine -= lineNumbers[j] + A[j];
                }
                if (!zero)
                {
                    A[j]++;
                    emptySpaces--;
                }
                if (emptySpaces < 0)
                {
                    emptySpaces += A[j];
                    A[j] = 0;
                    j--;
                    zero = false;
                    if (j >= 0)
                        posInLine -= lineNumbers[j] + A[j] + 1;
                }
                else
                {
                    zero = false;
                    for (int z = 0; z < A[j]; z++)
                    {
                        if (line[posInLine + z] != 1)
                        {
                            possibleSolution[posInLine + z] = 0;
                        }
                        else
                        {
                            goto skipJ;
                        }
                    }
                    posInLine += A[j];
                    for (int z = 0; z < lineNumbers[j]; z++)
                    {
                        if (line[posInLine + z] != 0)
                        {
                            possibleSolution[posInLine + z] = 1;
                        }
                        else
                        {
                            posInLine -= A[j];
                            goto skipJ;
                        }
                    }
                    if (j != lineNumbers.Length - 1)
                    {
                        if (line[posInLine + lineNumbers[j]] != 1)
                        {
                            possibleSolution[posInLine + lineNumbers[j]] = 0;

                        }
                        else
                        {
                            posInLine -= A[j];
                            goto skipJ;
                        }
                    }
                    else
                    {
                        posInLine--;
                    }
                    zero = true;
                    posInLine += lineNumbers[j] + 1;
                    j++;
                    skipJ:;
                }

            }
            possibleSolution[0] = -1;
            return possibleSolution;
        }

        private int[] GetRightMostPossibleSolution(int x, bool isRow)
        {
            int cr = isRow ? c : r;
            int[] line = new int[cr];
            
            int[] lineNumbers;
            if (isRow)
            {
                lineNumbers = new int[row[x].Length];
                row[x].CopyTo(lineNumbers, 0);
                for (int i = 0; i < c; i++)
                    line[i] = board[x, i];

            }
            else
            {
                lineNumbers = new int[col[x].Length];
                for (int i = 0; i < r; i++)
                    line[i] = board[i, x];
                col[x].CopyTo(lineNumbers, 0);
            }
            line.ReverseArray();
            lineNumbers.ReverseArray();
            int emptySpaces = cr + 1;
            int[] possibleSolution = new int[cr];
            possibleSolution.Fill(0);
            foreach (int i in lineNumbers)
                emptySpaces -= (i + 1);
            int[] A = new int[lineNumbers.Length + 1];
            A.Fill(0);
            int j = 0;
            int posInLine = 0;
            bool zero = true;
            while (j >= 0)
            {
                if (j > (lineNumbers.Length - 1))
                {
                    for (int z = 0; z < emptySpaces; z++)
                    {
                        if (line[posInLine + z] != 1)
                        {
                            possibleSolution[posInLine + z] = 0;
                        }
                        else
                        {
                            goto skip;
                        }
                    }
                    possibleSolution.ReverseArray();
                    possibleSolution.NumberArray();
                    return possibleSolution;
                    skip:;
                    j--;
                    zero = false;
                    posInLine -= lineNumbers[j] + A[j];
                }
                if (!zero)
                {
                    A[j]++;
                    emptySpaces--;
                }
                if (emptySpaces < 0)
                {
                    emptySpaces += A[j];
                    A[j] = 0;
                    j--;
                    zero = false;
                    if (j >= 0)
                        posInLine -= lineNumbers[j] + A[j] + 1;
                }
                else
                {
                    zero = false;
                    for (int z = 0; z < A[j]; z++)
                    {
                        if (line[posInLine + z] != 1)
                        {
                            possibleSolution[posInLine + z] = 0;
                        }
                        else
                        {
                            goto skipJ;
                        }
                    }
                    posInLine += A[j];
                    for (int z = 0; z < lineNumbers[j]; z++)
                    {
                        if (line[posInLine + z] != 0)
                        {
                            possibleSolution[posInLine + z] = 1;
                        }
                        else
                        {
                            posInLine -= A[j];
                            goto skipJ;
                        }
                    }
                    if (j != lineNumbers.Length - 1)
                    {
                        if (line[posInLine + lineNumbers[j]] != 1)
                        {
                            possibleSolution[posInLine + lineNumbers[j]] = 0;

                        }
                        else
                        {
                            posInLine -= A[j];
                            goto skipJ;
                        }
                    }
                    else
                    {
                        posInLine--;
                    }
                    zero = true;
                    posInLine += lineNumbers[j] + 1;
                    j++;
                    skipJ:;
                }

            }
            
            return possibleSolution;
        }

        private void LeftRightLineFill(int x, bool isRow)
        {
            int cr = isRow ? c : r;
            int[] l = GetLeftMostPossibleSolution(x, isRow);
            if (l[0] != -1)
            {
                int[] p = GetRightMostPossibleSolution(x, isRow);
                if (isRow)
                {
                    for (int z = 0; z < c; z++)
                        if (l[z] == p[z])
                            if (l[z] <= 0)
                                board[x, z] = 0;
                            else
                                board[x, z] = 1;
                    isRowSolved[x] = true;
                    for (int z = 0; z < c; z++)
                        if (board[x, z] == 2)
                            isRowSolved[x] = false;
                }
                else
                {
                    for (int z = 0; z < r; z++)
                        if (l[z] == p[z])
                            if (l[z] <= 0)
                                board[z, x] = 0;
                            else
                                board[z, x] = 1;
                    isColSolved[x] = true;
                    for (int z = 0; z < r; z++)
                        if (board[z, x] == 2)
                            isColSolved[x] = false;
                }
            }
            else
            {
                isUnsolvable = true;
            }
        }
    
    }
}
